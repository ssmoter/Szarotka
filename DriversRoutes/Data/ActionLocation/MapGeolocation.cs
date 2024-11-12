namespace DriversRoutes.Data.ActionLocation
{
    public static class MapGeolocation
    {

        private static Action<Location, CancellationToken> _setLocation;
        private static Location _LastKnownLocation;
        private static CancellationToken _token;
        public static async Task OnStartListeningLocation(Action<Location, CancellationToken> setLocation, GeolocationAccuracy geolocationAccuracy, TimeSpan timeout, CancellationToken token = default)
        {
            _setLocation = setLocation;
            _token = token;
            var result = await Geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest(geolocationAccuracy, timeout));
            if (!result)
            {
                throw new Exception("Can't Start Listening Foreground Async");
            }
            Geolocation.LocationChanged += StartListening;
        }

        private static void StartListening(object sender, GeolocationLocationChangedEventArgs e)
        {
            if (_LastKnownLocation != null)
            {
                e.Location.Course = CalculateBearing(_LastKnownLocation, e.Location);
            }

            _LastKnownLocation = e.Location;
            _setLocation.Invoke(e.Location, _token);
        }
        public static void OnStopListeningLocation()
        {
            Geolocation.LocationChanged -= StartListening;
            Geolocation.StopListeningForeground();
        }



        public static double CalculateBearing(Location pointA, Location pointB)
        {
            double lat1 = DegreesToRadians(pointA.Latitude);
            double lon1 = DegreesToRadians(pointA.Longitude);
            double lat2 = DegreesToRadians(pointB.Latitude);
            double lon2 = DegreesToRadians(pointB.Longitude);

            double dLon = lon2 - lon1;
            double y = Math.Sin(dLon) * Math.Cos(lat2);
            double x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            double bearing = Math.Atan2(y, x);
            return RadiansToDegrees(bearing);
        }
        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
        private static double RadiansToDegrees(double radians)
        {
            return radians * 180.0 / Math.PI;
        }


    }
}
