using DriversRoutes.Model;
using DriversRoutes.Service;

using System.Net.Http.Json;

namespace DriversRoutes.Platforms.Android
{
    public class DownloadAddress : IDownloadAddress
    {
        readonly HttpClient _client;
        public DownloadAddress()
        {
            _client = new();
        }

        public async Task<GoogleApiAddress> FindAddressFromCoordinates(double latitude, double longitude)
        {
            try
            {
                var lat = latitude.ToString().Replace(',', '.');
                var lon = longitude.ToString().Replace(',', '.');

                var uri = new Uri($"https://_maps.googleapis.com/_maps/api/geocode/json?latlng={lat},{lon}&key={DataBase.Platforms.Android.ValueFromManifest.GetValue("", "")}");

                _client.BaseAddress = uri;

                var result = await _client.GetFromJsonAsync<GoogleApiAddress>(uri);

                if (result.Status == "OK")
                {
                    return result;
                }
                throw new Exception("Nie znaleziono adresu");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
