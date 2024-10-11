using Microsoft.Maui.Maps;

namespace DriversRoutes.Helper
{
    public class CurrentLocation
    {
        public static readonly MapSpan Szarotka = new(new Location(49.74918622300343, 20.40891067705071), 0.1, 0.1);

        public static async Task<MapSpan> Get()
        {
            try
            {
                GeolocationRequest request = new(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                var _cancelTokenSource = new CancellationTokenSource();
                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                location ??= await Geolocation.Default.GetLastKnownLocationAsync();
                if (location is null || location.IsFromMockProvider)
                {
                    return Szarotka;
                }

                MapSpan mapSpan = new(location, 0.01, 0.01);
                return mapSpan;
            }
            catch (Exception)
            {
                return Szarotka;
            }
        }
    }
}
