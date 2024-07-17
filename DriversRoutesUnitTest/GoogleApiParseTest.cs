using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;
using DriversRoutes.Model;

using FluentAssertions;

using System.Net.Http.Json;

namespace DriversRoutesUnitTest
{
    public class GoogleApiParseTest
    {
        public static async Task<GoogleApiAddress> FindAddressFromCoordinates(double latitude, double longitude)
        {
            try
            {
                var lat = latitude.ToString().Replace(',', '.');
                var lon = longitude.ToString().Replace(',', '.');

                var uri = new Uri($"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lon}&key=AIzaSyDMfTC47bnsNBAK8S4xKk7Mhb_aiSqnCYU");
                HttpClient _client = new()
                {
                    BaseAddress = uri
                };

                var result = await _client.GetFromJsonAsync<GoogleApiAddress>(uri);

                if (result is not null)
                {
                    if (result.Status == "OK")
                    {
                        return result;
                    }
                }
                throw new Exception("Nie znaleziono adresu");
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Xunit.Fact]
        public async void TestParsingDecimal()
        {
            var req =await FindAddressFromCoordinates(49.74942715620574, 20.40880945691556);

            List<ResidentialAddress> obj = [];

            for (int i = 0; i < req.Results.Count; i++)
            {
                obj.Add(req.Results[i].FromGoogleToAddress());


                obj[i].Should().NotBeNull();
            }

        }



    }
}
