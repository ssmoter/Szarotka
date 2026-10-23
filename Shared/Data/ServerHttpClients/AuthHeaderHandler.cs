using DataBase.Model.EntitiesServer;
using DataBase.Model.SourceGenerator;

using Shared.Helper;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Shared.Data.ServerHttpClients
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly IHttpClientFactory _httpClientFactory;

        // Klucz opcji chroniący przed nieskończoną pętlą zapytań 401
        private static readonly HttpRequestOptionsKey<bool> IsRetryKey = new("IsHttpRequestRetry");

        public AuthHeaderHandler(IAuthService authService, IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // 1. Pobierz aktualny Access Token i doklej go do nagłówka
            var accessToken = await _authService.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            // 2. Wyślij zapytanie do Twojego API
            var response = await base.SendAsync(request, cancellationToken);

            // Sprawdzamy, czy to zapytanie nie jest już ponowną próbą (Loop Guard)
            request.Options.TryGetValue(IsRetryKey, out bool isRetry);

            // 3. Jeśli serwer zwrócił 401 Unauthorized oraz zapytanie NIE było jeszcze ponawiane
            if (response.StatusCode == HttpStatusCode.Unauthorized && !isRetry)
            {
                bool isRefreshed = false;

                // Używamy semafora, aby w przypadku wielu równoległych zapytań tylko JEDNO wysłało żądanie o odświeżenie
                await _semaphore.WaitAsync(cancellationToken);
                try
                {
                    // Sprawdzamy, czy inny wątek przed chwilą nie odświeżył już tokenu
                    var currentToken = await _authService.GetAccessTokenAsync();
                    if (currentToken != accessToken)
                    {
                        isRefreshed = true; // Ktoś już odświeżył, możemy ponowić próbę
                    }
                    else
                    {
                        // Wywołujemy procedurę odświeżenia tokenu
                        isRefreshed = await TryRefreshTokensAsync();
                    }
                }
                finally
                {
                    _semaphore.Release();
                }

                // 4. Jeśli odświeżenie się udało, ponawiamy oryginalne zapytanie z nowym tokenem
                if (isRefreshed)
                {
                    var newAccessToken = await _authService.GetAccessTokenAsync();

                    // Klonujemy żądanie, ponieważ stare zostało już zużyte przez HttpClient
                    using (var clonedRequest = await CloneHttpRequestMessageAsync(request))
                    {
                        clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);

                        // Oznaczamy sklonowane żądanie flagą ponowienia, aby zapobiec pętli w razie awarii serwera
                        clonedRequest.Options.Set(IsRetryKey, true);

                        // Zwalniamy starą odpowiedź 401
                        response.Dispose();

                        // Zwalniamy zużyty Content ze starego żądania w celu optymalizacji GC
                        request.Content?.Dispose();

                        // Wysyłamy klon żądania ponownie
                        response = await base.SendAsync(clonedRequest, cancellationToken);
                    }
                }
                else
                {
                    // Jeśli odświeżenie się NIE udało -> wyloguj użytkownika
                    _authService.ClearTokens();
                }
            }

            return response;
        }

        // Pomocnicza metoda do głębokiego klonowania obiektów HttpRequestMessage i resetowania strumieni
        private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req)
        {
            var clone = new HttpRequestMessage(req.Method, req.RequestUri)
            {
                Version = req.Version
            };

            if (req.Content != null)
            {
                // POBRANIE BAJTÓW - Najbardziej odporna metoda na Native AOT.
                // Unikamy problemów z zablokowanymi strumieniami i błędami linkera przy CopyToAsync.
                byte[] contentBytes = await req.Content.ReadAsByteArrayAsync();

                // Tworzymy zawartość jako ByteArrayContent (zamiast StreamContent), 
                // co eliminuje błąd "Error while copying content to a stream."
                clone.Content = new ByteArrayContent(contentBytes);

                // Kopiujemy nagłówki zawartości (np. Content-Type)
                foreach (var h in req.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
                }
            }

            // Kopiujemy ogólne nagłówki żądania (oprócz autoryzacji)
            foreach (var h in req.Headers)
            {
                if (h.Key != "Authorization")
                {
                    clone.Headers.TryAddWithoutValidation(h.Key, h.Value);
                }
            }

            // Bezpieczne kopiowanie słownika właściwości żądania pod Native AOT (.NET 10)
            var cloneOptionsDict = (IDictionary<string, object?>)clone.Options;
            foreach (var kvp in (IDictionary<string, object?>)req.Options)
            {
                cloneOptionsDict[kvp.Key] = kvp.Value;
            }

            return clone;
        }

        private async Task<bool> TryRefreshTokensAsync()
        {
            try
            {
                Shared.Service.AndroidPermissionService.InternetCheck();

                string url = "user/refresh-token";

                var baseAddress = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka).BaseAddress;
                var refreshTokenObj = await _authService.GetRefreshTokenAsync();

                if (string.IsNullOrEmpty(refreshTokenObj.Value)) return false;

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = baseAddress;

                var response = await client.PostAsJsonAsync(url, refreshTokenObj, SzarotkaJsonSerializerContext.Default.RefreshToken);

                if (response.IsSuccessStatusCode)
                {
                    var tokens = await response.Content.ReadFromJsonAsync<User>(SzarotkaJsonSerializerContext.Default.User);

                    if (tokens != null)
                    {
                        await _authService.SaveTokensAsync(tokens.AccessToken, tokens.RefreshToken);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
