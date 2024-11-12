using Microsoft.Maui.Maps;

namespace DriversRoutes.Data.ActionLocation
{
    public class CurrentLocation
    {
        public static readonly MapSpan Szarotka = new(new Location(49.74918622300343, 20.40891067705071), 0.1, 0.1);

        public static async Task<MapSpan> Get(GeolocationAccuracy geolocationAccuracy, TimeSpan timeout, CancellationToken token = default)
        {
            try
            {
                GeolocationRequest request = new(geolocationAccuracy, timeout);

                var location = await Geolocation.Default.GetLocationAsync(request, token);
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
