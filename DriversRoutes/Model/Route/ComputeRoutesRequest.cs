namespace DriversRoutes.Model.Route
{
    public class ComputeRoutesRequest
    {
        /// <summary>
        /// Wymagane. Punkt pośredni na trasie.
        /// </summary>
        public Waypoint Origin { get; set; }
        /// <summary>
        /// Wymagane. Docelowy punkt na trasie.
        /// </summary>
        public Waypoint Destination { get; set; }
        /// <summary>
        /// Opcjonalnie: Zestaw punktów na trasie (z wyłączeniem punktów końcowych) do zatrzymania się na lub przejechania.
        /// Obsługiwane jest maksymalnie 25 pośrednich punktów pośrednich.
        /// </summary>
        public Waypoint[] Intermediates { get; set; }

        /// <summary>
        /// Opcjonalnie: Określa środek transportu.
        /// </summary>
        public RouteTravelMode TravelMode { get; set; }
        /// <summary>
        /// Opcjonalnie: Określa sposób obliczania trasy.
        /// Serwer próbuje użyć wybranego ustawienia routingu do obliczenia trasy.
        /// Jeśli ustawienie routingu powoduje błąd lub bardzo długie opóźnienie, zwracany jest błąd.
        /// Tę opcję możesz określić tylko wtedy, gdy travelMode ma wartość DRIVE lub TWO_WHEELER. W przeciwnym razie żądanie nie zostanie zrealizowane.
        /// </summary>
        public RoutingPreference RoutingPreference { get; set; }
        /// <summary>
        /// Opcjonalnie: Określa preferencję dotyczącą jakości linii łamanej.
        /// </summary>
        public PolylineQuality PolylineQuality { get; set; }

        /// <summary>
        /// Opcjonalnie: Określa preferowane kodowanie linii łamanej.
        /// </summary>
        public PolylineEncoding PolylineEncoding { get; set; }
        /// <summary>
        /// Opcjonalnie: Godzina odjazdu. Jeśli nie ustawisz tej wartości, domyślnie będzie przyjęta data przesłania żądania.
        /// UWAGA: pole departureTime z przeszłości możesz określić tylko wtedy, gdy opcja RouteTravelMode ma wartość TRANSIT.
        /// Podróże transportem publicznym mogą obejmować maksymalnie 7 dni wstecz lub 100 dni do przodu.
        ///Sygnatura czasowa w RFC3339 UTC „Zulu” z rozdzielczością nanosekundową i maksymalnie 9 cyframi po przecinku.Przykłady: "2014-10-02T15:01:23Z" i "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public string DepartureTime { get; set; }
        /// <summary>
        /// Opcjonalnie: Godzina przyjazdu. UWAGA: ten parametr można ustawić tylko wtedy, gdy zasada RouteTravelMode ma wartość TRANSIT.
        /// Możesz określić departureTime lub arrivalTime, ale nie oba jednocześnie. Podróże transportem publicznym mogą obejmować maksymalnie 7 dni wstecz lub 100 dni do przodu.
        ///Sygnatura czasowa w RFC3339 UTC „Zulu” z rozdzielczością nanosekundową i maksymalnie 9 cyframi po przecinku.Przykłady: "2014-10-02T15:01:23Z" i "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public string ArrivalTime { get; set; }
        /// <summary>
        /// Opcjonalnie: Określa, czy oprócz trasy obliczać trasy alternatywne.
        /// W przypadku żądań z pośrednimi punktami pośrednimi nie są zwracane żadne alternatywne trasy.
        /// </summary>
        public bool ComputeAlternativeRoutes { get; set; }
        /// <summary>
        /// Opcjonalnie: Zestaw warunków, które wpływają na sposób obliczania tras.
        /// </summary>
        public RouteModifiers RouteModifiers { get; set; }
        /// <summary>
        /// Opcjonalnie: Kod języka BCP-47, na przykład „en-US”. czy „sr-Latn”.
        /// Więcej informacji znajdziesz w sekcji Identyfikator języka Unicode. Listę obsługiwanych języków znajdziesz w sekcji Obsługa języków.
        /// Jeśli nie podasz tej wartości, język wyświetlania będzie ustalany na podstawie lokalizacji żądania trasy.
        /// </summary>
        public string LanguageCode { get; set; }
        /// <summary>
        /// Opcjonalnie: Kod regionu określony jako dwuznakowa wartość domeny ccTLD („domeny najwyższego poziomu”). Więcej informacji znajdziesz w artykule Domeny krajowe najwyższego poziomu.
        /// </summary>
        public string RegionCode { get; set; }
        /// <summary>
        /// Opcjonalnie: Określa jednostki miary dla pól wyświetlanych.
        /// Te pola obejmują pole instruction w elemencie NavigationInstruction.
        /// Ta wartość nie ma wpływu na jednostki miary stosowane dla trasy, odcinka, dystansu ani czasu trwania.
        /// Jeśli nie podasz tej wartości, wyświetlane jednostki zostaną ustalone na podstawie lokalizacji pierwszego punktu początkowego.
        /// </summary>
        public Units Units { get; set; }
        /// <summary>
        /// Opcjonalnie: Jeśli zasada ma wartość true (prawda), usługa próbuje zminimalizować ogólny koszt trasy przez zmianę kolejności określonych pośrednich punktów pośrednich.
        /// Żądanie nie powiedzie się, jeśli dowolny z pośrednich punktów pośrednich jest punktem pośrednim via.
        /// Aby znaleźć nowe kolejność, użyj funkcji ComputeRoutesResponse.Routes.optimized_intermediate_waypoint_index.
        /// Jeśli w nagłówku X-Goog-FieldMask nie jest żądane ComputeRoutesResponseRoutes.optimized_intermediate_waypoint_index, żądanie się nie powiedzie.
        /// Jeśli optimizeWaypointOrder ma wartość Fałsz, pole ComputeRoutesResponse.optimized_intermediate_waypoint_index jest puste.
        /// </summary>
        public bool OptimizeWaypointOrder { get; set; }
        /// <summary>
        /// Opcjonalnie: Określa, które trasy referencyjne mają zostać obliczone w ramach żądania, oprócz trasy domyślnej.
        /// Trasa referencyjna to trasa z innym celem obliczenia trasy niż domyślna.
        /// Na przykład podczas obliczania trasy referencyjnej FUEL_EFFICIENT uwzględniane są różne parametry, które mogą wygenerować optymalną trasę z najniższym spalaniem.
        /// </summary>
        public ReferenceRoute[] RequestedReferenceRoutes { get; set; }
        /// <summary>
        /// Opcjonalnie: Lista dodatkowych obliczeń, które mogą zostać wykorzystane do wykonania żądania.
        /// Uwaga: te dodatkowe obliczenia mogą zwrócić dodatkowe pola w odpowiedzi.
        /// Te dodatkowe pola muszą być również określone w masce pola, aby została zwrócona w odpowiedzi.
        /// </summary>
        public ExtraComputation[] ExtraComputations { get; set; }
        /// <summary>
        /// Opcjonalnie: Określa założenia, które należy zastosować przy obliczaniu czasu w ruchu.
        /// To ustawienie wpływa na wartość zwracaną w polu czasu trwania w polach Route i RouteLeg, które zawierają przewidywany czas ruchu określony na podstawie średnich wartości historycznych.
        /// Funkcja TrafficModel jest dostępna tylko w przypadku żądań, w których wartość RoutingPreference ma wartość TRAFFIC_AWARE_OPTIMAL, a RouteTravelMode na DRIVE.
        /// Domyślna wartość to BEST_GUESS, jeśli żądanie dotyczy ruchu, a TrafficModel nie jest określony.
        /// </summary>
        public TrafficModel TrafficModel { get; set; }
        /// <summary>
        /// Opcjonalnie: Określa preferencje, które mają wpływ na trasę zwracaną dla TRANSIT tras.
        /// UWAGA: pole transitPreferences możesz określić tylko wtedy, gdy pole RouteTravelMode ma wartość TRANSIT.
        /// </summary>
        public TransitPreferences TransitPreferences { get; set; }


        public ComputeRoutesRequest()
        {
            Origin = new();
            Destination = new();
            Intermediates = [];
            RouteModifiers = new()
            {
                AvoidFerries = true,
                AvoidHighways = true,
                AvoidTolls = true,
                VehicleInfo = new()
                {
                    EmissionType = VehicleEmissionType.DIESEL,
                },
            };
            RequestedReferenceRoutes = [];
            ExtraComputations = [];
            TransitPreferences = new();
            TravelMode = RouteTravelMode.DRIVE;
            RoutingPreference = RoutingPreference.TRAFFIC_AWARE;
            ComputeAlternativeRoutes = false;
            Units = Units.METRIC;
            LanguageCode = "pl-pl";
        }
    }

    public enum PolylineQuality
    {
        /// <summary>
        /// Nie określono preferencji jakości linii łamanej. Domyślna wartość to OVERVIEW.
        /// </summary>
        POLYLINE_QUALITY_UNSPECIFIED,
        /// <summary>
        /// Określa wysokiej jakości linię łamaną, która składa się z większej liczby punktów niż OVERVIEW, ale kosztem zwiększonego rozmiaru odpowiedzi.
        /// Użyj tej wartości, jeśli potrzebujesz większej precyzji.
        /// </summary>
        HIGH_QUALITY,
        /// <summary>
        /// Określa linię łamaną przeglądu, która składa się z niewielkiej liczby punktów. Użyj tej wartości do wyświetlania ogólnego widoku trasy.
        /// Ta opcja wiąże się z krótszym czasem oczekiwania na żądanie w porównaniu z opcją HIGH_QUALITY.
        /// </summary>
        OVERVIEW,
    }

    public enum ReferenceRoute
    {
        /// <summary>
        /// Nieużywane. Żądania zawierające tę wartość kończą się niepowodzeniem.
        /// </summary>
        REFERENCE_ROUTE_UNSPECIFIED,
        /// <summary>
        /// Trasa z najniższym spalaniem. Trasy oznaczone tą wartością są uznawane za zoptymalizowane pod kątem takich parametrów jak zużycie paliwa.
        /// </summary>
        FUEL_EFFICIENT,
    }

    public enum PolylineEncoding
    {
        /// <summary>
        ///Nie określono preferowanego typu linii łamanej. Domyślna wartość to ENCODED_POLYLINE. 
        /// </summary>
        POLYLINE_ENCODING_UNSPECIFIED,
        /// <summary>
        /// Określa linię łamaną zakodowaną przy użyciu algorytmu kodowania linii łamanej.
        /// </summary>
        ENCODED_POLYLINE,
        /// <summary>
        /// Określa linię łamaną w formacie wiersza GeoJSON
        /// </summary>
        GEO_JSON_LINESTRING,
    }
}
