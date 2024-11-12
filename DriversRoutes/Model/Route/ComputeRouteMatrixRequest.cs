namespace DriversRoutes.Model.Route
{
    public class ComputeRouteMatrixRequest
    {
        public List<RouteMatrixOrigin> Origins { get; set; }
        public List<RouteMatrixDestination> Destinations { get; set; }
        public RouteTravelMode TravelMode { get; set; }
        public RoutingPreference RoutingPreference { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string LanguageCode { get; set; }
        public string RegionCode { get; set; }
        public Units Units { get; set; }
        public List<ExtraComputation> ExtraComputations { get; set; }
        public TrafficModel TrafficModel { get; set; }
        public TransitPreferences TransitPreferences { get; set; }

        public ComputeRouteMatrixRequest()
        {
            this.Origins = [];
            this.Destinations = [];
            this.ExtraComputations = [];
            this.TransitPreferences = new();
        }
    }

    public class TransitPreferences
    {
        /// <summary>
        /// Zestaw środków transportu do wykorzystania przy wyznaczaniu trasy TRANSIT.
        /// Domyślnie wszystkie obsługiwane środki transportu.
        /// </summary>
        public List<TransitTravelMode> AllowedTravelModes { get; set; }
        /// <summary>
        /// Preferencje routingu, które po określeniu wpływają na zwróconą trasę TRANSIT.
        /// </summary>
        public TransitRoutingPreference RoutingPreference { get; set; }
        public TransitPreferences()
        {
            AllowedTravelModes ??= [];
        }
        public enum TransitTravelMode
        {
            /// <summary>
            /// Nie określono środka transportu publicznego.
            /// </summary>
            TRANSIT_TRAVEL_MODE_UNSPECIFIED,
            BUS,
            SUBWAY,
            TRAIN,
            LIGHT_RAIL,
        }
        public enum TransitRoutingPreference
        {
            /// <summary>
            /// Nie określono preferencji.
            /// </summary>
            TRANSIT_ROUTING_PREFERENCE_UNSPECIFIED,
            LESS_WALKING,
            FEWER_TRANSFERS,
        }
    }
    public enum TrafficModel
    {
        /// <summary>
        /// Nieużywana. Jeśli zostanie podany, domyślnie przyjęta zostanie wartość BEST_GUESS.
        /// </summary>
        TRAFFIC_MODEL_UNSPECIFIED,
        /// <summary>
        /// Zwraca wartość duration, która powinna być szacowanym czasem podróży, biorąc pod uwagę zarówno informacje o warunkach historycznych, jak i natężeniu ruchu.
        /// Ruch na żywo staje się ważniejszy, gdy departureTime jest bliżej.
        /// </summary>
        BEST_GUESS,
        /// <summary>
        /// Zwraca on informację, że w większości dni zwrócony czas podróży powinien być dłuższy niż rzeczywisty czas podróży, ale czasami dni o szczególnie złych warunkach drogowych mogą przekraczać tę wartość.
        /// </summary>
        PESSIMISTIC,
        /// <summary>
        /// Zwraca on informacje o tym, że w większości dni zwrócony czas podróży powinien być krótszy niż w rzeczywistości, chociaż w przypadku dni, w których warunki na drodze są wyjątkowo atrakcyjne, te wartości mogą być krótsze.
        /// </summary>
        OPTIMISTIC,
    }

    public enum ExtraComputation
    {
        /// <summary>
        /// Nieużywane. Żądania zawierające tę wartość zakończą się niepowodzeniem.
        /// </summary>
        EXTRA_COMPUTATION_UNSPECIFIED,
        /// <summary>
        /// Informacje o opłatach na trasach.
        /// </summary>
        TOLLS,
        /// <summary>
        /// Szacowane zużycie paliwa na trasach.
        /// </summary>
        FUEL_CONSUMPTION,
        /// <summary>
        /// Na trasach występują linie łamane z informacją o natężeniu ruchu.
        /// </summary>
        TRAFFIC_ON_POLYLINE,
        /// <summary>
        /// NavigationInstructions w postaci sformatowanego ciągu tekstowego HTML.
        /// Tę treść należy czytać w takiej postaci, w jakiej jest.
        /// Ta treść jest przeznaczona tylko do wyświetlania. Nie analizuj go automatycznie.
        /// </summary>
        HTML_FORMATTED_NAVIGATION_INSTRUCTIONS,
    }

    public enum Units
    {
        UNITS_UNSPECIFIED,
        METRIC,
        IMPERIAL,
    }

    public enum RoutingPreference
    {
        /// <summary>
        /// Nie określono preferencji dotyczących routingu. Wartość domyślna to TRAFFIC_UNAWARE.
        /// </summary>
        ROUTING_PREFERENCE_UNSPECIFIED,
        /// <summary>
        /// Oblicza trasy bez uwzględniania bieżących warunków drogowych.
        /// Odpowiednie, gdy warunki na drodze nie mają znaczenia lub nie mają zastosowania.
        /// Użycie tej wartości daje najniższy czas oczekiwania. Uwaga: w przypadku aplikacji RouteTravelMode DRIVE i TWO_WHEELER wybrana trasa i czas jej trwania zależą od sieci drogowej i średniej wartości natężenia ruchu niezależnego od czasu, a nie bieżących warunków na drodze.
        /// Dlatego też trasy mogą obejmować drogi, które są tymczasowo zamknięte.
        /// Wyniki dla danego żądania mogą się z czasem zmieniać w wyniku zmian w sieci dróg, aktualnych warunków średniej ruchu oraz rozproszonego charakteru usługi.
        /// Wyniki mogą się też zmieniać w dowolnej chwili i z częstotliwością niemal identycznych tras.
        /// </summary>
        TRAFFIC_UNAWARE,
        /// <summary>
        /// Oblicza trasy z uwzględnieniem bieżących warunków drogowych.
        /// W przeciwieństwie do TRAFFIC_AWARE_OPTIMAL niektóre optymalizacje mają zastosowanie w celu znacznego skrócenia czasu oczekiwania.
        /// </summary>
        TRAFFIC_AWARE,
        /// <summary>
        /// Oblicza trasy z uwzględnieniem bieżących warunków drogowych, bez zastosowania większości optymalizacji skuteczności.
        /// Użycie tej wartości powoduje najdłuższe opóźnienie.
        /// </summary>
        TRAFFIC_AWARE_OPTIMAL,
    }


    public enum RouteTravelMode
    {
        TRAVEL_MODE_UNSPECIFIED,
        DRIVE,
        BICYCLE,
        WALK,
        TWO_WHEELER,
        TRANSIT,
    }

    public class RouteMatrixDestination
    {
        /// <summary>
        /// Wymagane. Docelowy punkt na trasie
        /// </summary>
        public Waypoint Waypoint { get; set; }
        public RouteMatrixDestination()
        {
            this.Waypoint = new();
        }
    }

    public class Waypoint
    {
        /// <summary>
        /// Oznacza ten punkt pośredni jako punkt pośredni, a nie etap milowy.
        /// W przypadku każdego punktu pośredniego innego w żądaniu odpowiedź dołącza wpis do tablicy legs ze szczegółowymi informacjami o przystankach na danym etapie podróży.
        /// Ustaw tę wartość na „true” (prawda), jeśli trasa ma przechodzić przez ten punkt pośredni bez zatrzymywania się.
        /// Punkty pośrednie nie powodują dodania wpisu do tablicy legs, ale prowadzą trasę przez punkt pośredni.
        /// Tę wartość możesz ustawić tylko dla punktów pośrednich, które są pośrednimi. Żądanie nie powiedzie się, jeśli ustawisz to pole na punktach pośrednich terminala.
        /// Jeśli ComputeRoutesRequest.optimize_waypoint_order ma wartość true (prawda), to pole nie może mieć wartości true (prawda). w przeciwnym razie żądanie nie powiedzie się.
        /// </summary>
        public bool Via { get; set; }
        /// <summary>
        /// Wskazuje, że punkt pośredni jest przeznaczony dla pojazdów, na których się zatrzymują, gdzie celem jest wysyłkę lub wyjazd.
        /// Gdy ustawisz tę wartość, obliczona trasa nie będzie uwzględniać punktów na drogach innych niż via na drogach, które nie nadają się do wsiąść i wysiąść.
        /// Ta opcja działa tylko w przypadku środków transportu DRIVE i TWO_WHEELER, a locationType ma wartość Location.
        /// </summary>
        public bool VehicleStopover { get; set; }
        /// <summary>
        /// Wskazuje, że umiejscowienie tego punktu pośredniego powinno umożliwiać zatrzymanie pojazdu po określonej stronie drogi.
        /// Po ustawieniu tej wartości trasa będzie przechodzić przez lokalizację, tak aby pojazd mógł się zatrzymać na poboczu drogi, w kierunku której lokalizacja jest przekierowana od środka drogi.
        /// Ta opcja działa tylko w przypadku DRIVE i TWO_WHEELER RouteTravelMode.
        /// </summary>
        public bool SideOfRoad { get; set; }
        public Location Location { get; set; }
        /// <summary>
        /// Identyfikator miejsca POI powiązany z punktem pośrednim.
        /// </summary>
        public string PlaceId { get; set; }
        /// <summary>
        /// Zrozumiały dla człowieka adres lub kod plus. Szczegółowe informacje znajdziesz na stronie https://plus.codes.
        /// </summary>
        public string Address { get; set; }
        public Waypoint()
        {
            this.Location = new();
        }
    }
    public class Location
    {
        /// <summary>
        /// Współrzędne geograficzne punktu pośredniego.
        /// </summary>
        public LatLng LatLng { get; set; }
        /// <summary>
        /// Nagłówek kompasu powiązany z kierunkiem ruchu.
        /// Ta wartość określa po stronie drogi, po której na miejscu będzie można wsiąść i wsiąść.
        /// Wartości nagłówka mogą należeć do zakresu od 0 do 360, gdzie 0 oznacza kierunek północny, 90 kierunek kierunku wschodu itd.
        /// Możesz użyć tego pola tylko w przypadku pól DRIVE i TWO_WHEELER RouteTravelMode.
        /// </summary>
        public int Heading { get; set; }
        public Location()
        {
            this.LatLng = new();
        }
    }
    public class LatLng
    {
        /// <summary>
        /// Szerokość geograficzna w stopniach. Musi mieścić się w zakresie [-90,0, +90,0].
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Długość geograficzna w stopniach. Musi mieścić się w zakresie od -180,0 do +180,0].
        /// </summary>
        public double Longitude { get; set; }
    }

    public class RouteMatrixOrigin
    {
        /// <summary>
        /// Wymagane. Punkt początkowy
        /// </summary>
        public Waypoint Waypoint { get; set; }
        public RouteModifiers RouteModifiers { get; set; }
        public RouteMatrixOrigin()
        {
            this.Waypoint = new();
            this.RouteModifiers = new();
        }
    }

    public class RouteModifiers
    {
        /// <summary>
        /// Jeśli zasada ma wartość Prawda, w miarę możliwości omija drogi płatne, traktując priorytetowo trasy, które nie zawierają dróg płatnych.
        /// Ma zastosowanie tylko do DRIVE i TWO_WHEELER RouteTravelMode.
        /// </summary>
        public bool AvoidTolls { get; set; }
        /// <summary>
        /// Jeśli zasada ma wartość Prawda, w miarę możliwości omija autostrady i ma pierwszeństwo przed trasami, które nie zawierają autostrad.
        /// Ma zastosowanie tylko do DRIVE i TWO_WHEELER RouteTravelMode.
        /// </summary>
        public bool AvoidHighways { get; set; }
        /// <summary>
        /// Jeśli zasada ma wartość Prawda, w miarę możliwości omija przeprawy promowe, traktując priorytetowo trasy niezawierające promów.
        /// Ma zastosowanie tylko do DRIVE i TWO_WHEELER RouteTravelMode.
        /// </summary>
        public bool AvoidFerries { get; set; }
        /// <summary>
        /// Jeśli zasada ma wartość Prawda, w miarę możliwości pomijane jest poruszanie się po wnętrzach, lecz preferowane są trasy, które nie zawierają nawigacji wewnątrz budynków.
        /// Dotyczy tylko WALK RouteTravelMode.
        /// </summary>
        public bool AvoidIndoor { get; set; }
        public VehicleInfo VehicleInfo { get; set; }
        public List<TollPasse> TollPasses { get; set; }
        public RouteModifiers()
        {
            this.VehicleInfo = new();
            this.TollPasses = [];
        }
    }

    public enum TollPasse
    {
    }

    public class VehicleInfo
    {
        public VehicleEmissionType EmissionType { get; set; }
    }
    public enum VehicleEmissionType
    {
        VEHICLE_EMISSION_TYPE_UNSPECIFIED,
        GASOLINE,
        ELECTRIC,
        HYBRID,
        DIESEL,
    }
}
