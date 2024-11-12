namespace DriversRoutes.Data.GoogleApi
{
    public class DecodeRoutes
    {
        public static List<Location> DecodePolyline(string encodedPoints)
        {
            if (string.IsNullOrWhiteSpace(encodedPoints))
                return null;

            var poly = new List<Location>();
            var polylinechars = encodedPoints.ToCharArray();
            var index = 0;
            var currentLat = 0;
            var currentLng = 0;

            while (index < polylinechars.Length)
            {
                var sum = 0;
                var shifter = 0;
                int next5bits;

                do
                {
                    next5bits = polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += ((sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1));

                sum = 0;
                shifter = 0;

                do
                {
                    next5bits = polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32)
                    break;

                currentLng += ((sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1));
                var point = new Location
                    ((Convert.ToDouble(currentLat) / 1E5), (Convert.ToDouble(currentLng) / 1E5));
                poly.Add(point);
            }
            return poly;
        }



    }
}
