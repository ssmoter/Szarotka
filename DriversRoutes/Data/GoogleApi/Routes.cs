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
        void SetKey(HttpClient httpClient, string key);
    }

    public class Routes : IRoutes
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _key;
        private static string Uri => "https://routes.googleapis.com/directions/v2:computeRoutes";

        public Routes(IHttpClientFactory httpClient)
        {
            _httpClientFactory = httpClient;
            _key = Shared.Key.GoogleApi.Key;
        }

        public void SetKey(HttpClient httpClient, string key)
        {
            httpClient.DefaultRequestHeaders.Remove("X-Goog-Api-Key");
            httpClient.DefaultRequestHeaders.Add("X-Goog-Api-Key", key);
        }

        private void SetFieldMask(HttpClient httpClient, string fieldMask)
        {
            httpClient.DefaultRequestHeaders.Remove("X-Goog-FieldMask");
            httpClient.DefaultRequestHeaders.Add("X-Goog-FieldMask", fieldMask);
        }

        /// <summary>
        /// Metoda do której ręcznie można dodać odpowiednie FieldMask
        /// </summary>
        /// <param name="fieldMask">Lista pól które ma zwrócić end point. Dodawane po przecinku
        ///  Przykład: routes.duration,routes.distanceMeters </param>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<string> ComputeAsString(string fieldMask, Model.Route.ComputeRoutesRequest request, CancellationToken token = default)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            SetKey(httpClient, _key);
            SetFieldMask(httpClient, fieldMask);


            var result = await httpClient.PostAsJsonAsync(Uri, request, ComputeRoutesRequestJsonSerializerContext.Default.ComputeRoutesRequest, token);

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
        /// <param name="fieldMask">Lista pól które ma zwrócić end point. Dodawane po przecinku
        ///  Przykład: routes.duration,routes.distanceMeters </param>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<DriversRoutes.Model.Route.Response> Compute(string fieldMask, Model.Route.ComputeRoutesRequest request, CancellationToken token = default)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            SetKey(httpClient, _key);
            SetFieldMask(httpClient, fieldMask);

            var result = await httpClient.PostAsJsonAsync(Uri, request, ComputeRoutesRequestJsonSerializerContext.Default.ComputeRoutesRequest, token);

            if (!result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync(token);
                throw new HttpRequestException(message: $"Błąd przy pobieraniu trasy{Environment.NewLine}{json}");
            }

            var stream = await result.Content.ReadAsStreamAsync(token);


            var response = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(stream, ResponseJsonSerializerContext.Default.Response, cancellationToken: token);

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
