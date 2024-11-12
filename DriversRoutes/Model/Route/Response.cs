namespace DriversRoutes.Model.Route
{
    public class ResponseMatrix
    {
        /// <summary>
        /// Kod stanu błędu tego elementu.
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Wskazuje, czy trasa została znaleziona czy nie. Niezależny od stanu.
        /// </summary>
        public RouteMatrixElementCondition Condition { get; set; }
        /// <summary>
        /// Odległość przebyta trasa w metrach.
        /// </summary>
        public int DistanceMeters { get; set; }
        /// <summary>
        /// Czas potrzebny na nawigowanie po trasie.
        /// Jeśli ustawisz routingPreference na TRAFFIC_UNAWARE, ta wartość będzie taka sama jak staticDuration.
        /// Jeśli ustawisz routingPreference na TRAFFIC_AWARE lub TRAFFIC_AWARE_OPTIMAL, ta wartość jest obliczana z uwzględnieniem warunków na drodze.
        ///Czas trwania w sekundach składający się z maksymalnie 9 cyfr po przecinku, kończący się cyfrą „s”. Przykład: "3.5s".
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Długość trasy bez uwzględnienia warunków drogowych.
        /// Czas trwania w sekundach składający się z maksymalnie 9 cyfr po przecinku, kończący się cyfrą „s”. Przykład: "3.5s".
        /// </summary>
        public string StaticDuration { get; set; }
        /// <summary>
        /// Dodatkowe informacje o trasie. Na przykład: informacje o ograniczeniach i opłatach
        /// </summary>
        public RouteTravelAdvisory TravelAdvisory { get; set; }
        /// <summary>
        /// W niektórych przypadkach, gdy serwer nie jest w stanie obliczyć trasy przy określonych preferencjach dla danej pary punktu początkowego i docelowego, może użyć innego trybu obliczeń.
        /// Gdy używany jest tryb zastępczy, to pole zawiera szczegółowe informacje o odpowiedzi kreacji zastępczej. 
        /// przeciwnym razie to pole jest nieskonfigurowane
        /// </summary>
        public FallbackInfo FallbackInfo { get; set; }
        /// <summary>
        /// Tekstowe przedstawienie właściwości elementu RouteMatrixElement.
        /// </summary>
        public LocalizedValues LocalizedValues { get; set; }
        /// <summary>
        /// Indeks liczony od zera dla punktu początkowego w żądaniu.
        /// </summary>
        public int OriginIndex { get; set; }
        /// <summary>
        /// Indeks liczony od zera dla miejsca docelowego w żądaniu.
        /// </summary>
        public int DestinationIndex { get; set; }


        public ResponseMatrix()
        {
            Status = new();
            TravelAdvisory = new();
            FallbackInfo = new();
            LocalizedValues = new();
        }
    }

    public class LocalizedValues
    {
    }

    public class FallbackInfo
    {
    }

    public class RouteTravelAdvisory
    {
    }

    public enum RouteMatrixElementCondition
    {
    }

    public class Status
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<Details> Details { get; set; }
        public Status()
        {
            Details = [];
        }
    }

    public class Details
    {
    }
}
