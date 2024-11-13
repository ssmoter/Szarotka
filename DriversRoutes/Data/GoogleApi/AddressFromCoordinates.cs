using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;
using DriversRoutes.Model.Address;

using System.Net.Http.Json;

namespace DriversRoutes.Data.GoogleApi
{
    public interface IAddressFromCoordinates
    {
        Task<GoogleApiAddress> FindGoogleApiAddress(double latitude, double longitude, CancellationToken token = default);
        Task<ResidentialAddress[]> FindResidentialAddress(double latitude, double longitude, CancellationToken token = default);
    }

    public class AddressFromCoordinates : IAddressFromCoordinates
    {
        private readonly HttpClient _httpClient;

        private readonly string key;

        public AddressFromCoordinates(HttpClient httpClient)
        {
            _httpClient = httpClient;
            key = DataBase.Key.GoogleApi.Key;
            //key = DataBase.Helper.Manifest.GetManifestValue("com.google.android.geo.API_KEY");
        }
        public AddressFromCoordinates(HttpClient httpClient, string _key)
        {
            _httpClient = httpClient;
            key = _key;
        }

        public async Task<GoogleApiAddress> FindGoogleApiAddress(double latitude, double longitude, CancellationToken token = default)
        {
            try
            {
                var lat = latitude.ToString().Replace(',', '.');
                var lon = longitude.ToString().Replace(',', '.');

                var uri = new Uri($"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lon}&_key={key}");
                _httpClient.BaseAddress = uri;

                var result = await _httpClient.GetFromJsonAsync<GoogleApiAddress>(uri, token) ?? throw new Exception("Wystąpił nieznany błąd przy odwróconej geolokalizacji");

                return result.Status switch
                {
                    "OK" => result,
                    "ZERO_RESULTS" => throw new ArgumentNullException("Nie znaleziono dopasowanego adresu"),
                    "OVER_QUERY_LIMIT" => throw new Exception("Limit zapytań został wykorzystany"),
                    "REQUEST_DENIED" => throw new UnauthorizedAccessException("Zapytanie zostało odrzucone"),
                    "INVALID_REQUEST" => throw new Exception("Błędne zapytanie, brakuje parametru"),
                    "UNKNOWN_ERROR" => throw new Exception("Nie znany błąd"),
                    _ => throw new Exception("Nie znany błąd"),
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResidentialAddress[]> FindResidentialAddress(double latitude, double longitude, CancellationToken token = default)
        {
            var googleAddress = await FindGoogleApiAddress(latitude, longitude, token);

            ResidentialAddress[] residentialAddress = new ResidentialAddress[googleAddress.Results.Count];

            for (int i = 0; i < googleAddress.Results.Count; i++)
            {
                residentialAddress[i] = googleAddress.Results[i].FromGoogleToAddress();
            }
            return residentialAddress;
        }

    }
}
