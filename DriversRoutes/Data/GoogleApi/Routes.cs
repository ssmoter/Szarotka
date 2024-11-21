using DataBase.Helper;

using DriversRoutes.Model.Route;

using System.Net.Http.Json;

namespace DriversRoutes.Data.GoogleApi
{
    public interface IRoutes
    {
        Task<Response> Compute(string FieldMask, ComputeRoutesRequest request, CancellationToken token = default);
        Task<string> ComputeAsString(string FieldMask, ComputeRoutesRequest request, CancellationToken token = default);
        Task<Response> GetOnlyDistanceAndDuration(ComputeRoutesRequest request, CancellationToken token = default);
        Task<Response> GetOnlyRouteStepsDurationDistance(ComputeRoutesRequest request, CancellationToken token = default);
        void SetKey(string key);
    }

    public class Routes : IRoutes
    {
        private readonly HttpClient _httpClient;

        private readonly string _key;
        private static string Uri => "https://routes.googleapis.com/directions/v2:computeRoutes";

        public Routes(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _key = DataBase.Key.GoogleApi.Key;

            if (!_httpClient.DefaultRequestHeaders.Contains("X-Goog-Api-Key"))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", _key);
            }
        }
        public Routes(HttpClient httpClient, string _key)
        {
            _httpClient = httpClient;
            this._key = _key;
            if (!_httpClient.DefaultRequestHeaders.Contains("X-Goog-Api-Key"))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", this._key);
            }
        }

        public void SetKey(string key)
        {
            _httpClient.DefaultRequestHeaders.Remove("X-Goog-Api-Key");
            _httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", key);
        }

        /// <summary>
        /// Metoda do której ręcznie można dodać odpowiednie FieldMask
        /// </summary>
        /// <param name="FieldMask">Lista pól które ma zwrócić end point. Dodawane po przecinku
        ///  Przykład: routes.duration,routes.distanceMeters </param>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<string> ComputeAsString(string FieldMask, Model.Route.ComputeRoutesRequest request, CancellationToken token = default)
        {
            _httpClient.DefaultRequestHeaders.Remove("X-Goog-FieldMask");
            _httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", FieldMask);

            var result = await _httpClient.PostAsJsonAsync(Uri, request, token);

            var json = await result.Content.ReadAsStringAsync(token);
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException(message: $"Błąd przy pobieraniu trasy{Environment.NewLine}{json}");
            }

            return json;
        }

        /// <summary>
        /// Metoda do której ręcznie można dodać odpowiednie FieldMask
        /// </summary>
        /// <param name="FieldMask">Lista pól które ma zwrócić end point. Dodawane po przecinku
        ///  Przykład: routes.duration,routes.distanceMeters </param>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<DriversRoutes.Model.Route.Response> Compute(string FieldMask, Model.Route.ComputeRoutesRequest request, CancellationToken token = default)
        {
            _httpClient.DefaultRequestHeaders.Remove("X-Goog-FieldMask");
            _httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", FieldMask);

            var result = await _httpClient.PostAsJsonAsync(Uri, request, token);

            if (!result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync(token);
                throw new HttpRequestException(message: $"Błąd przy pobieraniu trasy{Environment.NewLine}{json}");
            }

            var stream = await result.Content.ReadAsStreamAsync(token);

            var options = JsonOptions.JsonSerializeOptionsIgnoreCapitalLetters;
            options.Converters.Add(JsonOptions.JsonSerializeOptionsJsonStringEnumConverter.Converters[0]);

            var response = await System.Text.Json.JsonSerializer.DeserializeAsync<DriversRoutes.Model.Route.Response>(stream, options, cancellationToken: token);

            return response;
        }

        /// <summary>
        /// FieldMask jest ustawiony na zwrócenie kroków,czasu i drogi: routes.legs.steps,routes.duration,routes.distanceMeters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<DriversRoutes.Model.Route.Response> GetOnlyRouteStepsDurationDistance(Model.Route.ComputeRoutesRequest request, CancellationToken token = default)
        {
            var result = await Compute("routes.legs.steps,routes.duration,routes.distanceMeters", request, token);
            return result;
        }
        /// <summary>
        /// FieldMask jest ustawiony na zwrócenie czasu, drogi: routes.duration,routes.distanceMeters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<DriversRoutes.Model.Route.Response> GetOnlyDistanceAndDuration(Model.Route.ComputeRoutesRequest request, CancellationToken token = default)
        {
            var result = await Compute("routes.duration,routes.distanceMeters", request, token);
            return result;
        }



    }
}
