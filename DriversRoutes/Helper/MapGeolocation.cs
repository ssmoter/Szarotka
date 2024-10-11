namespace DriversRoutes.Helper
{
    public static class MapGeolocation
    {

        private static Action<Location> _setLocation;
        private static Location _LastKnownLocation;

        public static async Task OnStartListeningLocation(Action<Location> setLocation)
        {
            _setLocation = setLocation;
            var result = await Geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest(GeolocationAccuracy.High, TimeSpan.FromMicroseconds(100)));
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
            _setLocation.Invoke(e.Location);
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
