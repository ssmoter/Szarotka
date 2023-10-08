using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DriversRoutes.Model;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace DriversRoutes.Pages.Maps
{
    public partial class MapsVM : ObservableObject
    {
        #region Variable

        MapsM[] allPoints;

        [ObservableProperty]
        MapsM[] mapsPoint;

        [ObservableProperty]
        string driversRoutesName = "Trasa kierowcy";

        [ObservableProperty]
        string selectedDayName = "";

        [ObservableProperty]
        bool isVisibleTypeOfMap;

        [ObservableProperty]
        bool isVisibleList;

        [ObservableProperty]
        MapType mapType;

        [ObservableProperty]
        string addLocationIsText = block;

        public bool AddLocationIs { get; set; } = false;

        const string enable = "Dostępne";
        const string block = "Zablokowane";

        public Action<MapSpan> GoToLocation;

        readonly MapSpan szarotka = new(new Location(49.74918684945343, 20.40889755094227), 0.1, 0.1);

        readonly DataBase.Data.AccessDataBase _db;
        #endregion
        public MapsVM(DataBase.Data.AccessDataBase db)
        {
            _db = db;
            SelectedDayName = DisplaySelectedDayName(DateTime.Today.DayOfWeek);
            MapType = MapType.Street;
            allPoints = new MapsM[500];
            for (int i = 0; i < 500; i++)
            {
                allPoints[i] = (new MapsM().CreateRandomPoint(i));
            }

            allPoints.FirstOrDefault().Pin.Location = new Location(49.7488002173044, 20.408379427432106);
            MapsPoint = allPoints;
        }


        #region Method
        public async Task<MapSpan> GetCurrentLocation()
        {
            try
            {
                GeolocationRequest request = new(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));

                var _cancelTokenSource = new CancellationTokenSource();
                var location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                location ??= await Geolocation.Default.GetLastKnownLocationAsync();
                if (location is null || location.IsFromMockProvider)
                {
                    return szarotka;
                }

                MapSpan mapSpan = new(location, 0.1, 0.1);
                return mapSpan;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                return szarotka;
            }

        }


        static string DisplaySelectedDayName(DayOfWeek day)
        {
            string name = $"Dzień:{Environment.NewLine}";
            switch (day)
            {
                case DayOfWeek.Sunday:
                    name += "Niedziela";
                    break;
                case DayOfWeek.Monday:
                    name += "Poniedziałek";
                    break;
                case DayOfWeek.Tuesday:
                    name += "Wtorek";
                    break;
                case DayOfWeek.Wednesday:
                    name += "Środa";
                    break;
                case DayOfWeek.Thursday:
                    name += "Czwartek";
                    break;
                case DayOfWeek.Friday:
                    name += "Piątek";
                    break;
                case DayOfWeek.Saturday:
                    name += "Sobota";
                    break;
                default:
                    name = "Nie wybrano dnia";
                    break;
            }
            return name;
        }

        public void OnGoToLocation(MapSpan mapSpan)
        {
            GoToLocation?.Invoke(mapSpan);
        }

        public async Task SavePoint(Customer customer)
        {

        }

        public async Task AddToArray()
        {

        }

        public void OpenMoreDetail(Pin pin)
        {
            SwipeViewGesture();
            MapsPoint = allPoints.Where(x => pin.Label.Contains(x.Name)).ToArray();
        }


        #endregion


        #region Command

        [RelayCommand]
        async Task ChangeDay()
        {
            var popup = new Popups.SelectDay.SelectDayV();
            var response = await Shell.Current.ShowPopupAsync(popup);
            if (response is null)
            {
                return;
            }
            if (response is DayOfWeek day)
            {
                SelectedDayName = DisplaySelectedDayName(day);
            }







        }


        [RelayCommand]
        void DisplayTypeOfMap() => IsVisibleTypeOfMap = !IsVisibleTypeOfMap;

        [RelayCommand]
        void ChangeTypeOfMap(string type)
        {
            if (int.TryParse(type, result: out int result))
            {
                if (result >= 0 && result <= 2)
                {
                    MapType = (MapType)result;
                }
            }
        }

        [RelayCommand]
        void SwipeViewGesture()
        {
            if (MapsPoint.Length < allPoints.Length)
            {
                MapsPoint = allPoints;
            }
            IsVisibleList = !IsVisibleList;
        }

        [RelayCommand]
        void DisplayLocationOfPin(MapsM mapsM)
        {
            if (mapsM is null)
                return;

            var mapSpan = new MapSpan(mapsM.Pin.Location, 0.1, 0.1);
            OnGoToLocation(mapSpan);
        }

        [RelayCommand]
        void ChangeAddLocationIs()
        {
            AddLocationIs = !AddLocationIs;
            if (AddLocationIs)
            {
                AddLocationIsText = enable;
            }
            if (!AddLocationIs)
            {
                AddLocationIsText = block;
            }
        }




        #endregion

    }
}
