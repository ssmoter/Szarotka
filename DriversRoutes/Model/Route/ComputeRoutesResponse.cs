
using DriversRoutes.Model.Address;

namespace DriversRoutes.Model.Route
{
    public class Response
    {
        /// <summary>
        /// Zawiera tablicę obliczonych tras (maksymalnie 3), jeśli została określona jako compute_alternatives_routes, oraz zawiera tylko jedną trasę, jeśli nie jest.
        /// Jeśli ta tablica zawiera wiele wpisów, pierwsza z nich jest najbardziej zalecaną trasą. Jeśli tablica jest pusta, oznacza to, że nie udało się znaleźć trasy.
        /// </summary>
        public Route[] Routes { get; set; }
        /// <summary>
        /// W niektórych przypadkach, gdy serwer nie jest w stanie obliczyć wyników trasy przy wszystkich podanych preferencjach wejściowych, może użyć innego sposobu obliczeń.
        /// Gdy używany jest tryb zastępczy, to pole zawiera szczegółowe informacje o odpowiedzi kreacji zastępczej.
        /// W przeciwnym razie to pole jest nieskonfigurowane.
        /// </summary>
        public FallbackInfo FallbackInfo { get; set; }
        /// <summary>
        /// Zawiera informacje o odpowiedzi na geokodowanie dla punktów pośrednich określonych jako adresy.
        /// </summary>
        public GeocodingResults GeocodingResults { get; set; }

        public Response()
        {
            Routes = [];
            FallbackInfo = new();
            GeocodingResults = new();
        }
    }

    public class GeocodingResults
    {
        /// <summary>
        /// Geokodowany punkt pośredni wyznaczający punkt początkowy.
        /// </summary>
        public GeocodedWaypoint Origin { get; set; }
        /// <summary>
        /// Geokodowany punkt pośredni miejsca docelowego.
        /// </summary>
        public GeocodedWaypoint Destination { get; set; }
        /// <summary>
        /// Lista pośrednich, geokodowanych punktów pośrednich zawierających pole indeksu odpowiadające pozycji punktu pośredniego (od zera) w kolejności, w jakiej zostały określone w żądaniu.
        /// </summary>
        public GeocodedWaypoint[] Intermediates { get; set; }

        public GeocodingResults()
        {
            Origin = new();
            Destination = new();
            Intermediates = [];
        }
    }

    public class GeocodedWaypoint
    {
        /// <summary>
        /// Wskazuje kod stanu wynikający z operacji geokodowania.
        /// </summary>
        public Status GeocoderStatus { get; set; }
        /// <summary>
        /// Typy wyniku w postaci zero lub większej liczby tagów typu. Obsługiwane typy: Typy adresów i typy komponentów adresu.
        /// </summary>
        public string[] Type { get; set; }
        /// <summary>
        /// Wskazuje, że geokoder nie zwrócił dokładnego dopasowania do pierwotnego żądania, chociaż udało mu się dopasować część żądanego adresu.
        /// Możesz sprawdzić, czy pierwotne zgłoszenie nie zawiera błędów pisowni ani niekompletnych adresów.
        /// </summary>
        public bool PartialMatch { get; set; }
        /// <summary>
        /// Identyfikator miejsca powiązany z tym wynikiem.
        /// </summary>
        public string PlaceId { get; set; }
        /// <summary>
        /// Indeks odpowiedniego pośredniego punktu pośredniego w żądaniu. Wartość podawana tylko wtedy, gdy odpowiedni punkt na trasie jest pośrednim punktem pośrednim.
        /// </summary>
        public int IntermediateWaypointRequestIndex { get; set; }
        public GeocodedWaypoint()
        {
            GeocoderStatus = new();
            Type = [];
        }
    }

    public class Route
    {
        /// <summary>
        /// Etykiety obiektu Route, które służą do identyfikowania określonych właściwości trasy w celu porównania jej z innymi.
        /// </summary>
        public RouteLabel[] RouteLabels { get; set; }
        /// <summary>
        /// Zbiór nóg (odcinków ścieżki między punktami pośrednimi), z których składa się trasa.
        /// Każdy odcinek odpowiada podróży między dwoma drogami innymi niż via Waypoints.
        /// Na przykład trasa bez pośrednich punktów pośrednich ma tylko jeden etap.
        /// Trasa obejmująca jeden punkt pośredni inny niż via ma dwie nogi.
        /// Trasa obejmująca 1 pośredni punkt na trasie via, ma 1 etap.
        /// Kolejność etapów odpowiada kolejności punktów na trasie od origin do intermediates do destination.
        /// </summary>
        public RouteLeg[] Legs { get; set; }
        /// <summary>
        /// Odległość przebyta trasa w metrach.
        /// </summary>
        public int DistanceMeters { get; set; }
        /// <summary>
        /// Czas potrzebny na nawigowanie po trasie.
        /// Jeśli ustawisz routingPreference na TRAFFIC_UNAWARE, ta wartość będzie taka sama jak staticDuration.
        /// Jeśli ustawisz routingPreference na TRAFFIC_AWARE lub TRAFFIC_AWARE_OPTIMAL, ta wartość jest obliczana z uwzględnieniem warunków na drodze.
        ///  Czas trwania w sekundach składający się z maksymalnie 9 cyfr po przecinku, kończący się cyfrą „s”. Przykład: "3.5s".
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Czas podróży na trasie bez uwzględnienia warunków drogowych.
        ///  Czas trwania w sekundach składający się z maksymalnie 9 cyfr po przecinku, kończący się cyfrą „s”. Przykład: "3.5s".
        /// </summary>
        public string StaticDuration { get; set; }
        /// <summary>
        /// Linia łamana całej trasy. Ta linia łamana to połączona linia łamana ze wszystkich elementów typu legs.
        /// </summary>
        public Polyline Polyline { get; set; }
        /// <summary>
        /// Opis trasy.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Tablica ostrzeżeń wyświetlanych przy wyświetlaniu trasy.
        /// </summary>
        public string[] Warnings { get; set; }
        /// <summary>
        /// Ramka ograniczająca widoczny obszar linii łamanej.
        /// </summary>
        public Viewport Viewport { get; set; }
        /// <summary>
        /// Jeśli ustawisz optimizeWaypointOrder na wartość true, to pole zawiera zoptymalizowaną kolejność pośrednich punktów pośrednich.
        /// W przeciwnym razie to pole jest puste.
        /// Jeśli na przykład podasz dane pochodzenia: LA; Punkty pośrednie: Dallas, Bangor, Phoenix; Miejsce docelowe: Nowy Jork; a zoptymalizowana kolejność pośrednich punktów pośrednich to Phoenix, Dallas, Bangor.
        /// To pole zawiera wartości [2, 0, 1]. Indeks rozpoczyna się od 0 dla pierwszego pośredniego punktu pośredniego podanego w danych wejściowych.
        /// </summary>
        public int[] OptimizedIntermediateWaypointIndex { get; set; }
        /// <summary>
        /// Tekstowe przedstawienie właściwości elementu Route.
        /// </summary>
        public RouteLocalizedValues RouteLocalizedValues { get; set; }
        /// <summary>
        /// Zakodowany w Internecie token trasy zakodowany w base64, który można przekazać do pakietu Navigation SDK, który umożliwia pakietowi Navigation SDK zrekonstruowanie trasy podczas nawigacji, a w przypadku jej ponownego wyznaczania może być zgodny z intencją jej utworzenia przez wywołanie v2.computeRoutes.
        /// Klienci powinni traktować ten token jako nieprzezroczysty obiekt blob. Nie porównuj jego wartości w poszczególnych żądaniach – ten token może się zmienić, nawet jeśli zwrócona zostanie dokładnie ta sama trasa.
        /// UWAGA: funkcja Route.route_token jest dostępna tylko w przypadku żądań, w których pole ComputeRoutesRequest.routing_preference ma wartość TRAFFIC_AWARE lub TRAFFIC_AWARE_OPTIMAL. Pole Route.route_token nie jest obsługiwane w przypadku żądań z punktami pośrednimi Via.
        /// </summary>
        public string RouteToken { get; set; }

        public Route()
        {
            RouteLabels = [];
            Legs = [];
            Polyline = new();
            Warnings = [];
            Viewport = new();
            OptimizedIntermediateWaypointIndex = [];
            RouteLocalizedValues = new();
        }

    }

    public class RouteLocalizedValues
    {
        /// <summary>
        /// Odległość podróży w formie tekstowej.
        /// </summary>
        public LocalizedText Distance { get; set; }
        /// <summary>
        /// Czas trwania z uwzględnieniem warunków na drodze, wyrażony w formie tekstowej.
        /// Uwaga: jeśli żądanie informacji o ruchu nie zostało wysłane, ta wartość będzie mieć taką samą wartość jak staticDuration.
        /// </summary>
        public LocalizedText Duration { get; set; }
        /// <summary>
        /// Czas trwania bez uwzględnienia warunków na drodze, wyrażony w formie tekstowej.
        /// </summary>
        public LocalizedText StaticDuration { get; set; }
        /// <summary>
        /// Cena za transport publiczny w formie tekstowej.
        /// </summary>
        public LocalizedText TransitFare { get; set; }
        public RouteLocalizedValues()
        {
            Distance = new();
            Duration = new();
            StaticDuration = new();
            TransitFare = new();
        }
    }
    public class LocalizedText
    {
        /// <summary>
        /// Zlokalizowany ciąg znaków w języku odpowiadającym languageCode.
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Kod języka BCP-47 (np. „en-US”). czy „sr-Latn”.
        /// Więcej informacji znajdziesz na stronie http://www.unicode.org/reports/tr35/#Unicode_locale_identifier.
        /// </summary>
        public string LanguageCode { get; set; }
    }


    public class Polyline
    {
        /// <summary>
        /// Kodowanie ciągu znaków w linii łamanej za pomocą algorytmu kodowania linii łamanej
        /// </summary>
        public string EncodedPolyline { get; set; }
        /// <summary>
        /// Określa linię łamaną w formacie wiersza GeoJSON.
        /// </summary>
        public object GeoJsonLinestring { get; set; }
    }

    public class RouteLeg
    {
        /// <summary>
        /// Odległość pokonanego odcinka trasy w metrach.
        /// </summary>
        public int DistanceMeters { get; set; }
        /// <summary>
        /// Czas potrzebny na poruszanie się po nodze.
        /// Jeśli route_preference ma wartość TRAFFIC_UNAWARE, ta wartość jest taka sama jak staticDuration.
        /// Jeśli route_preference to TRAFFIC_AWARE lub TRAFFIC_AWARE_OPTIMAL, ta wartość jest obliczana z uwzględnieniem warunków na drodze.
        /// Czas trwania w sekundach składający się z maksymalnie 9 cyfr po przecinku, kończący się cyfrą „s”. Przykład: "3.5s".
        /// </summary>
        public string Duration { get; set; }
        /// <summary>
        /// Długość odcinka obliczona bez uwzględniania warunków na drodze.
        /// Czas trwania w sekundach składający się z maksymalnie 9 cyfr po przecinku, kończący się cyfrą „s”. Przykład: "3.5s".
        /// </summary>
        public string StaticDuration { get; set; }
        /// <summary>
        /// Ogólna linia łamana dla tego odcinka, która zawiera linię łamaną każdej step.
        /// </summary>
        public Polyline Polyline { get; set; }
        /// <summary>
        /// Lokalizacja początkowa tego odcinka.
        /// Ta lokalizacja może się różnić od lokalizacji wskazanej w lokalizacji origin.
        /// Jeśli na przykład podany obiekt origin nie znajduje się w pobliżu drogi, jest to punkt na drodze
        /// </summary>
        public Location StartLocation { get; set; }
        /// <summary>
        /// Lokalizacja końcowa tego odcinka.
        /// Ta lokalizacja może się różnić od lokalizacji wskazanej w lokalizacji destination.
        /// Jeśli na przykład podany obiekt destination nie znajduje się w pobliżu drogi, jest to punkt na drodze.
        /// </summary>
        public Location EndLocation { get; set; }
        /// <summary>
        /// Tablica kroków oznaczająca segmenty w tym etapie. 
        /// Każdy krok odpowiada jednej instrukcji nawigacji.
        /// </summary>
        public RouteLegStep[] Steps { get; set; }
        /// <summary>
        /// Zawiera dodatkowe informacje, o których należy poinformować użytkownika, np. o ewentualnych ograniczeniach dotyczących strefy ruchu na danym etapie trasy.
        /// </summary>
        public RouteLegTravelAdvisory TravelAdvisory { get; set; }
        /// <summary>
        /// Tekstowe przedstawienie właściwości elementu RouteLeg.
        /// </summary>
        public RouteLegLocalizedValues LocalizedValues { get; set; }
        /// <summary>
        /// Omówienie kroków w tym RouteLeg. To pole jest wypełniane tylko w przypadku tras TRANSIT.
        /// </summary>
        public StepsOverview StepsOverview { get; set; }
        public RouteLeg()
        {
            Polyline = new();
            StartLocation = new();
            EndLocation = new();
            Steps = [];
            TravelAdvisory = new();
            LocalizedValues = new();
            StepsOverview = new();
        }
    }

    public class StepsOverview
    {
        /// <summary>
        /// Podsumowanie informacji o różnych segmentach multimodalnych: RouteLeg.steps.
        /// To pole nie jest wypełniane, jeśli w krokach RouteLeg nie zawiera żadnych segmentów multimodalnych.
        /// </summary>
        public MultiModalSegment[] MultiModalSegment { get; set; }
        public StepsOverview()
        {
            MultiModalSegment = [];
        }
    }

    public class MultiModalSegment
    {
        /// <summary>
        /// Instrukcja nawigacji dla segmentu multimodalnego.
        /// </summary>
        public NavigationInstruction NavigationInstruction { get; set; }
        /// <summary>
        /// Środek transportu segmentu multimodalnego.
        /// </summary>
        public RouteTravelMode TravelMode { get; set; }
        /// <summary>
        /// Odpowiedni indeks RouteLegStep, który jest początkiem segmentu multimodalnego.
        /// </summary>
        public int StepStarIndex { get; set; }
        /// <summary>
        /// Odpowiedni indeks RouteLegStep, który jest końcem segmentu multimodalnego.
        /// </summary>
        public int StepEndIndex { get; set; }
        public MultiModalSegment()
        {
            NavigationInstruction = new();
        }
    }

    public class NavigationInstruction
    {
        /// <summary>
        /// Obejmuje instrukcje nawigacji dotyczące bieżącego kroku (np. skręć w lewo, scal lub prosto).
        /// To pole określa, która ikona ma być wyświetlana.
        /// </summary>
        public Maneuver Maneuver { get; set; }
        /// <summary>
        /// Instrukcje dotyczące poruszania się po tym kroku.
        /// </summary>
        public string Instructions { get; set; }
    }

    public enum Maneuver
    {
        MANEUVER_UNSPECIFIED,
        TURN_SLIGHT_LEFT,
        TURN_SHARP_LEFT,
        UTURN_LEFT,
        TURN_LEFT,
        TURN_SLIGHT_RIGHT,
        TURN_SHARP_RIGHT,
        UTURN_RIGHT,
        TURN_RIGHT,
        STRAIGHT,
        RAMP_LEFT,
        RAMP_RIGHT,
        MERGE,
        FORK_LEFT,
        FORK_RIGHT,
        FERRY,
        FERRY_TRAIN,
        ROUNDABOUT_LEFT,
        ROUNDABOUT_RIGHT,
        DEPART,
        NAME_CHANGE,
    }

    public class RouteLegLocalizedValues
    {
        /// <summary>
        /// Odległość podróży w formie tekstowej.
        /// </summary>
        public LocalizedText Distance { get; set; }
        /// <summary>
        /// Czas trwania z uwzględnieniem warunków na drodze wyrażony w formie tekstowej.
        /// Uwaga: jeśli żądanie informacji o ruchu nie zostało wysłane, ta wartość będzie mieć taką samą wartość jak staticDuration.
        /// </summary>
        public LocalizedText Duration { get; set; }
        /// <summary>
        /// Czas trwania bez uwzględnienia warunków na drodze, wyrażony w formie tekstowej.
        /// </summary>
        public LocalizedText StaticDuration { get; set; }
        public RouteLegLocalizedValues()
        {
            Distance = new();
            Duration = new();
            StaticDuration = new();
        }

    }

    public class RouteLegTravelAdvisory
    {
        /// <summary>
        /// Zawiera informacje o opłatach na konkretnym: RouteLeg.
        /// To pole jest wypełnione tylko wtedy, gdy przewidujemy, że na: RouteLeg będą naliczane opłaty.
        /// Jeśli to pole jest skonfigurowane, ale pole podrzędne szacowanej ceny nie jest wypełnione, spodziewamy się, że dana droga będzie zawierać płatne drogi, ale nie znamy szacunkowej ceny.
        /// Jeśli to pole nie istnieje, RouteLeg nie pobiera opłat.
        /// </summary>
        public ToolInfo ToolInfo { get; set; }
        /// <summary>
        /// Interwały szybkiego odczytywania z informacjami o gęstości ruchu. Ma zastosowanie w przypadku preferencji routingu TRAFFIC_AWARE i TRAFFIC_AWARE_OPTIMAL.
        /// Przedziały obejmują całą linię łamaną RouteLeg bez nakładania się.
        /// Punkt początkowy określonego interwału jest taki sam jak punkt końcowy poprzedniego interwału.
        /// </summary>
        public SpeedReadingInterval[] SpeedReadingIntervals { get; set; }
        public RouteLegTravelAdvisory()
        {
            ToolInfo = new();
            SpeedReadingIntervals = [];
        }
    }

    public class SpeedReadingInterval
    {
        /// <summary>
        /// Indeks początkowy tego przedziału w linii łamanej.
        /// </summary>
        public int StartPolylinePointIndex { get; set; }
        /// <summary>
        /// Indeks końcowy tego przedziału na linii łamanej.
        /// </summary>
        public int EndPolylinePointIndex { get; set; }
        /// <summary>
        /// Prędkość ruchu w tym przedziale czasu.
        /// </summary>
        public Speed Speed { get; set; }

    }

    public enum Speed
    {
        SPEED_UNSPECIFIED,
        NORMAL,
        SLOW,
        TRAFFIC_JAM,
    }

    public class ToolInfo
    {
        /// <summary>
        /// Kwota opłat drogowych za: Route lub RouteLeg. Ta lista zawiera kwotę pieniężną w każdej walucie, która ma być pobierana przez stacje poboru opłat.
        /// Zwykle lista zawiera tylko jeden element w przypadku tras z opłatami za przejazd w jednej walucie.
        /// W przypadku podróży międzynarodowych ta lista może zawierać wiele pozycji w odniesieniu do opłat drogowych w różnych walutach.
        /// </summary>
        public Money[] EstimatedPrice { get; set; }
        public ToolInfo()
        {
            EstimatedPrice = [];
        }
    }

    public class Money
    {
        /// <summary>
        /// Trzyliterowy kod waluty zdefiniowany w normie ISO 4217.
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// Całkowita jednostka kwoty.
        /// Jeśli na przykład currencyCode to "USD", 1 jednostka to 1 zł.
        /// </summary>
        public string Units { get; set; }
        /// <summary>
        /// Liczba jednostek nano (10^-9) ilości.
        /// Wartość musi mieścić się w zakresie od -999 999 999 do +999 999 999 włącznie.
        /// Jeśli units ma wartość dodatnią, nanos musi być liczbą dodatnią lub zero.
        /// Jeśli units ma wartość 0, nanos może być liczbą dodatnią, zerową lub ujemną.
        /// Jeśli units ma wartość ujemną, nanos musi mieć wartość ujemną lub zero.
        /// Na przykład -1,75 zł jest przedstawione jako units=-1 i nanos=-750 000 000.
        /// </summary>
        public int Nanos { get; set; }
    }

    public class RouteLegStep
    {
        /// <summary>
        /// Długość tego kroku w metrach. W niektórych przypadkach to pole może nie mieć wartości.
        /// </summary>
        public int DistanceMeters { get; set; }
        /// <summary>
        /// Długość tej drogi bez uwzględnienia warunków drogowych.
        /// W niektórych przypadkach to pole może nie mieć wartości.
        ///Czas trwania w sekundach składający się z maksymalnie 9 cyfr po przecinku, kończący się cyfrą „s”. Przykład: "3.5s".
        /// </summary>
        public string StaticDuration { get; set; }
        /// <summary>
        /// Linia łamana powiązana z tym krokiem.
        /// </summary>
        public Polyline Polyline { get; set; }
        /// <summary>
        /// Lokalizacja początkowa tego kroku.
        /// </summary>
        public Location StartLocation { get; set; }
        /// <summary>
        /// Lokalizacja końcowa tego kroku.
        /// </summary>
        public Location EndLocation { get; set; }
        /// <summary>
        /// Instrukcja nawigacji
        /// </summary>
        public NavigationInstruction NavigationInstruction { get; set; }
        /// <summary>
        /// Zawiera dodatkowe informacje, o których należy poinformować użytkownika, np. o ewentualnych ograniczeniach związanych z ruchem drogowym, na etapie schodów.
        /// </summary>
        public RouteLegStepTravelAdvisory TravelAdvisory { get; set; }
        /// <summary>
        /// Tekstowe przedstawienie właściwości elementu RouteLegStep.
        /// </summary>
        public RouteLegStepLocalizedValues LocalizedValues { get; set; }
        /// <summary>
        /// Szczegóły dotyczące tego kroku, jeśli tryb podróży to TRANSIT.
        /// </summary>
        public RouteLegStepTransitDetails TransitDetails { get; set; }
        /// <summary>
        /// Tryb podróży użyty w tym kroku.
        /// </summary>
        public RouteTravelMode TravelMode { get; set; }

        public RouteLegStep()
        {
            Polyline = new();
            StartLocation = new();
            EndLocation = new();
            NavigationInstruction = new();
            TravelAdvisory = new();
            LocalizedValues = new();
            TransitDetails = new();
        }
    }

    public class RouteLegStepLocalizedValues
    {
        /// <summary>
        /// Odległość podróży w formie tekstowej.
        /// </summary>
        public LocalizedText Distance { get; set; }
        /// <summary>
        /// Czas trwania bez uwzględnienia warunków na drodze, wyrażony w formie tekstowej.
        /// </summary>
        public LocalizedText StaticDuration { get; set; }
        public RouteLegStepLocalizedValues()
        {
            Distance = new();
            StaticDuration = new();
        }
    }

    public class RouteLegStepTransitDetails
    {
        /// <summary>
        /// Informacje o przystankach na tym etapie.
        /// </summary>
        public TransitStopDetails StopDetails { get; set; }
        /// <summary>
        /// Tekstowe przedstawienie właściwości elementu RouteLegStepTransitDetails.
        /// </summary>
        public TransitDetailsLocalizedValues LocalizedValues { get; set; }
        /// <summary>
        /// Określa kierunek, w jakim należy poruszać się po tej linii, zgodnie z oznaczeniem na pojeździe lub na przystanku.
        /// Kierunek ruchu często prowadzi do stacji końcowej.
        /// </summary>
        public string Headsign { get; set; }
        /// <summary>
        /// Określa oczekiwaną godzinę jako czas między odjazdami z tego samego przystanku w tym samym czasie.
        /// Jeśli na przykład spóźnisz się na autobus, czas oczekiwania wynosi 600 sekund, a przejazd może potrwać 10 minut.
        ///Czas trwania w sekundach składający się z maksymalnie 9 cyfr po przecinku, kończący się cyfrą „s”. Przykład: "3.5s".
        /// </summary>
        public string Headway { get; set; }
        /// <summary>
        /// Informacje o linii transportu publicznego użytej w tym kroku.
        /// </summary>
        public TransitLine TransitLine { get; set; }
        /// <summary>
        /// Liczba przystanków od startu do przystanku. Ta liczba uwzględnia przystanek, ale nie uwzględnia przystanku odjazdu.
        /// Jeśli na przykład trasa zjeżdża z przystanku A, mija przystanki B i C i dotrze do przystanku D, funkcja stopCount zwróci wartość 3.
        /// </summary>
        public int StopCount { get; set; }
        /// <summary>
        /// Tekst wyświetlany w rozkładach jazdy i tablicach informacyjnych, które umożliwiają identyfikację podróży tranzytem dla pasażerów.
        /// Tekst powinien jednoznacznie identyfikować podróż w danym dniu obsługi klienta. Przykład: „538”.
        /// to tripShortText pociągu Amtrak, który odjeżdża z San Jose w Kalifornii o 15:10 w dni robocze do Sacramento w Kalifornii.
        /// </summary>
        public string TripShortText { get; set; }
        public RouteLegStepTransitDetails()
        {
            StopDetails = new();
            LocalizedValues = new();
            TransitLine = new();
        }
    }

    public class TransitLine
    {
        /// <summary>
        /// Przewoźnik obsługujący tę linię transportu publicznego.
        /// </summary>
        public TransitAgency[] Agencies { get; set; }
        /// <summary>
        /// Pełna nazwa tej linii transportu publicznego, np. „ul. Główna 8”.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// identyfikator URI tej linii transportu publicznego podany przez przewoźnika.
        /// </summary>
        public string Uri { get; set; }
        /// <summary>
        /// Kolor często używany na oznakowaniu tej linii. Wartość szesnastkowa.
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// Identyfikator URI ikony powiązanej z tym wierszem.
        /// </summary>
        public string IconUri { get; set; }
        /// <summary>
        /// Krótka nazwa tej linii transportu publicznego. Najczęściej jest to numer wiersza, np. „M7”. czy „355”.
        /// </summary>
        public string NameShort { get; set; }
        /// <summary>
        /// Kolor często używany w tekście na oznakowaniu w tej linii. Wartość szesnastkowa.
        /// </summary>
        public string TextColor { get; set; }
        /// <summary>
        /// Typ pojazdu, który porusza się po tej linii transportu publicznego.
        /// </summary>
        public TransitVehicle Vehicle { get; set; }
        public TransitLine()
        {
            Agencies = [];
            Vehicle = new();
        }
    }

    public class TransitVehicle
    {
        /// <summary>
        /// Nazwa tego pojazdu pisana wielkimi literami.
        /// </summary>
        public LocalizedText Name { get; set; }
        /// <summary>
        /// Typ używanego pojazdu.
        /// </summary>
        public TransitVehicleType Type { get; set; }
        /// <summary>
        /// Identyfikator URI ikony powiązanej z tym typem pojazdu.
        /// </summary>
        public string IconUri { get; set; }
        /// <summary>
        /// Identyfikator URI ikony powiązanej z tym typem pojazdu określany na podstawie lokalnego znaku transportowego.
        /// </summary>
        public string LocalIconUri { get; set; }
        public TransitVehicle()
        {
            Name = new();
        }
    }

    public enum TransitVehicleType
    {
        TRANSIT_VEHICLE_TYPE_UNSPECIFIED,
        BUS,
        CABLE_CAR,
        COMMUTER_TRAIN,
        FERRY,
        FUNICULAR,
        GONDOLA_LIFT,
        HEAVY_RAIL,
        HIGH_SPEED_TRAIN,
        INTERCITY_BUS,
        LONG_DISTANCE_TRAIN,
        METRO_RAIL,
        MONORAIL,
        OTHER,
        RAIL,
        SHARE_TAXI,
        SUBWAY,
        TRAM,
        TROLLEYBUS,
    }

    public class TransitAgency
    {
        /// <summary>
        /// Nazwa tego przewoźnika.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Sformatowany dla języka numer telefonu przewoźnika.
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Identyfikator URI przewoźnika.
        /// </summary>
        public string Uri { get; set; }
    }

    public class TransitDetailsLocalizedValues
    {
        /// <summary>
        /// Czas w sformatowanym tekście z odpowiednią strefą czasową.
        /// </summary>
        public LocalizedTime ArrivalTime { get; set; }
        /// <summary>
        /// Czas w sformatowanym tekście z odpowiednią strefą czasową.
        /// </summary>
        public LocalizedTime DepartureTime { get; set; }
        public TransitDetailsLocalizedValues()
        {
            ArrivalTime = new();
            DepartureTime = new();
        }
    }

    public class LocalizedTime
    {
        /// <summary>
        /// Czas określony jako ciąg znaków w danej strefie czasowej.
        /// </summary>
        public LocalizedText Time { get; set; }
        /// <summary>
        /// Zawiera strefę czasową.
        /// Wartość jest nazwą strefy czasowej zgodnie z definicją w bazie danych stref czasowych IANA, np. „Ameryka/Nowy_Jork”.
        /// </summary>
        public string TimeZone { get; set; }
        public LocalizedTime()
        {
            Time = new();
        }
    }

    public class TransitStopDetails
    {
        /// <summary>
        /// Informacje o przystanku na przyjazd dla tego kroku.
        /// </summary>
        public TransitStop ArrivalStop { get; set; }
        /// <summary>
        /// Szacowany czas dotarcia na miejsce.
        ///Sygnatura czasowa w RFC3339 UTC „Zulu” z rozdzielczością nanosekundową i maksymalnie 9 cyframi po przecinku.
        ///Przykłady: "2014-10-02T15:01:23Z" i "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public string ArrivalTime { get; set; }
        /// <summary>
        /// Informacje o przystanku odjazdu na tym etapie.
        /// </summary>
        public TransitStop DepartureStop { get; set; }
        /// <summary>
        /// Przewidywany czas wyjazdu danego kroku.
        ///Sygnatura czasowa w RFC3339 UTC „Zulu” z rozdzielczością nanosekundową i maksymalnie 9 cyframi po przecinku.
        ///Przykłady: "2014-10-02T15:01:23Z" i "2014-10-02T15:01:23.045123456Z".
        /// </summary>
        public string DepartureTime { get; set; }
        public TransitStopDetails()
        {
            ArrivalStop = new();
            DepartureStop = new();
        }
    }

    public class TransitStop
    {
        /// <summary>
        /// Nazwa przystanku transportu publicznego.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Lokalizacja przystanku wyrażona we współrzędnych geograficznych.
        /// </summary>
        public Location Location { get; set; }
        public TransitStop()
        {
            Location = new();
        }
    }

    public class RouteLegStepTravelAdvisory
    {
        /// <summary>
        /// UWAGA: to pole nie jest obecnie wypełnione.
        /// </summary>
        public SpeedReadingInterval[] SpeedReadingIntervals { get; set; }
        public RouteLegStepTravelAdvisory()
        {
            SpeedReadingIntervals = [];
        }
    }

    public enum RouteLabel
    {
        /// <summary>
        /// Domyślnie – nieużywane.
        /// </summary>
        ROUTE_LABEL_UNSPECIFIED,
        /// <summary>
        /// Wartość domyślna „najlepszy”. zwracaną na potrzeby obliczenia trasy.
        /// </summary>
        DEFAULT_ROUTE,
        /// <summary>
        /// Alternatywa dla domyślnego „najlepszego” .
        /// Tego typu trasy zostaną zwrócone, gdy określisz wartość computeAlternativeRoutes.
        /// </summary>
        DEFAULT_ROUTE_ALTERNATE,
        /// <summary>
        /// Trasa z najniższym spalaniem.
        /// Trasy oznaczone tą wartością są uznane za zoptymalizowane pod kątem parametrów Eko, takich jak zużycie paliwa.
        /// </summary>
        FUEL_EFFICIENT,

    }
}
