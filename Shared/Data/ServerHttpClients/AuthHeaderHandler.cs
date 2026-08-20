using DataBase.Model.EntitiesServer;
using DataBase.Model.SourceGenerator;

using Shared.Helper;

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
namespace Shared.Data.ServerHttpClients;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _serviceProvider;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthHeaderHandler(IAuthService authService, IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
    {
        _authService = authService;
        _serviceProvider = serviceProvider; // Potrzebne, aby uniknąć cyklicznej zależności przy odświeżaniu
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

        // 3. Jeśli serwer zwrócił 401 Unauthorized (Access Token wygasł)
        if (response.StatusCode == HttpStatusCode.Unauthorized)
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
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);

                // Zamykamy starą odpowiedź i wysyłamy żądanie ponownie
                response.Dispose();
                response = await base.SendAsync(request, cancellationToken);
            }
            else
            {
                // Jeśli odświeżenie się NIE udało (np. Refresh Token też wygasł w bazie) -> wyloguj użytkownika
                _authService.ClearTokens();
                // Tutaj warto przekierować użytkownika do widoku logowania, np. przez Shell.Current.GoToAsync("//LoginPage");
            }
        }

        return response;
    }

    private async Task<bool> TryRefreshTokensAsync()
    {
        try
        {
            Shared.Service.AndroidPermissionService.InternetCheck();

            // Zmieniamy endpoint na czysty URL (bez doklejania tokenu)
            string url = "user/refresh-token";

            var baseAddress = _httpClientFactory.CreateClient(Shared.Service.MyHttpClientsType.Szarotka).BaseAddress;
            var refreshTokenObj = await _authService.GetRefreshTokenAsync();

            // Poprawnie sprawdzamy .Value
            if (string.IsNullOrEmpty(refreshTokenObj.Value)) return false;

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = baseAddress;



            // Jeśli używasz Source Generatora, do PostAsJsonAsync też możesz przekazać kontekst, 
            // lub wysłać to klasycznie, jeśli serwer przyjmuje prosty obiekt.
            var response = await client.PostAsJsonAsync(url, refreshTokenObj, SzarotkaJsonSerializerContext.Default.RefreshToken);

            if (response.IsSuccessStatusCode)
            {
                // Wydajna, bezpośrednia deserializacja ze strumienia z użyciem Twojego Contextu
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
