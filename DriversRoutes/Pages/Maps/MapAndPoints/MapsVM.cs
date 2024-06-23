using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

using System.Collections.ObjectModel;

using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace DriversRoutes.Pages.Maps.MapAndPoints
{
    [QueryProperty(nameof(Routes), nameof(DataBase.Model.EntitiesRoutes.Routes))]
    [QueryProperty(nameof(AllPoints), nameof(MapsM))]
    [QueryProperty(nameof(LastSelectedDayOfWeek), nameof(SelectedDayOfWeekRoutes))]

    public partial class MapsVM : ObservableObject
    {
        #region Variable
        [ObservableProperty]
        ObservableCollection<MapsM> allPoints;

        [ObservableProperty]
        MapsM[] mapsPoint;

        [ObservableProperty]
        string driversRoutesName = "Trasa kierowcy: ";

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

        public SelectedDayOfWeekRoutes LastSelectedDayOfWeek { get; set; }
        public SelectedDayOfWeekRoutes LastSelectedDayOfWeekWhenNavigation { get; set; }

        public Routes Routes { get; set; }
        public bool AddLocationIs { get; set; } = false;

        const string enable = "Dostępne";
        const string block = "Zablokowane";

        public Action<MapSpan> GoToLocation;

        public readonly MapSpan szarotka = new(new Location(49.74918622300343, 20.40891067705071), 0.1, 0.1);

        readonly DataBase.Data.AccessDataBase _db;
        readonly Service.ISelectRoutes _selectRoutes;
        #endregion
        public MapsVM(DataBase.Data.AccessDataBase db, Service.ISelectRoutes selectRoutes)
        {
            _db = db;
            SelectedDayName = DisplaySelectedDayName(DateTime.Today.DayOfWeek);
            MapType = MapType.Street;
            AllPoints ??= [];
            _selectRoutes = selectRoutes;
            //for (int i = 0; i < 200; i++)
            //{
            //    AllPoints.Add(new MapsM().CreateRandomPoint(i));
            //}

            //AllPoints.FirstOrDefault().Pin.Location = new Location(49.7488002173044, 20.408379427432106);
            // MapsPoint = AllPoints.ToArray();
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

                MapSpan mapSpan = new(location, 0.01, 0.01);
                return mapSpan;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                return szarotka;
            }
        }

        public async Task StartListeningLocation(Microsoft.Maui.Controls.Maps.Map map)
        {
            try
            {
                var result = await Geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10)));
                Geolocation.LocationChanged += (sender, e) =>
                {
                    var location = e.Location;
                    var radius = map.VisibleRegion.Radius;
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), radius));
                };
                if (!result)
                {
                    throw new Exception("Can't Start Listening Foreground Async");
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
        public void OpenMoreDetail(Pin pin)
        {
            IsVisibleList = true;
            var index = MapsM.GetIndex(pin.Label);
            if (index == -1)
                return;

            MapsPoint = AllPoints.Where(x => x.Index == index).ToArray();
        }
        public async Task AddNewPoint(CustomerRoutes customer)
        {
            try
            {
                bool update = true;
                if (customer.Id == Guid.Empty)
                {
                    customer.Id = Guid.NewGuid();
                    update = false;
                }
                if (customer.DayOfWeek.Id == Guid.Empty)
                {
                    customer.DayOfWeek.Id = Guid.NewGuid();
                }
                if (customer.DayOfWeek.CustomerId == Guid.Empty)
                {
                    customer.DayOfWeek.CustomerId = customer.Id;
                }
                if (customer.RoutesId == Guid.Empty)
                {
                    customer.RoutesId = new Guid(Routes.Id.ToByteArray());
                }
                if (customer.CreatedTicks == 0)
                {
                    customer.Created = DateTime.Now;
                }

                customer.QueueNumber = _db.DataBase.Table<CustomerRoutes>().Count();

                var point = new MapsM
                {
                    Id = new Guid(customer.Id.ToByteArray()),
                    RoutesId = new Guid(customer.RoutesId.ToByteArray()),
                    Index = customer.QueueNumber,
                    Name = customer.Name,
                    Description = customer.Description,
                    PhoneNumber = customer.PhoneNumber,
                    Created = customer.Created,
                    SelectedDayOfWeek = customer.DayOfWeek,
                    Longitude = customer.Longitude,
                    Latitude = customer.Latitude,
                };


                point.SetPin();

                if (Routes is null)
                {
                    await Shell.Current.DisplayAlert("Brak trasy", "Zapisywanie jest dostępne tylko po wybraniu trasy konkretnego kierowcy", "Ok");
                    return;
                }

                if (update)
                {
                    await _db.DataBaseAsync.UpdateAsync(customer);
                    await _db.DataBaseAsync.UpdateAsync(customer.DayOfWeek);
                }
                else
                {
                    AllPoints.Add(point);
                    await _db.DataBaseAsync.InsertAsync(customer);
                    await _db.DataBaseAsync.InsertAsync(customer.DayOfWeek);
                }

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        public async void GetSelectedDaysAndForget(SelectedDayOfWeekRoutes week)
        {
            try
            {
                AllPoints.Clear();
                AllPoints = await GetSelectedDays(week);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }

        }
        public async Task<ObservableCollection<MapsM>> GetSelectedDays(SelectedDayOfWeekRoutes week)
        {
            var points = new ObservableCollection<MapsM>();
            try
            {
                var result = await _selectRoutes.GetCustomerRoutesQueryAsync(Routes, week);
                for (int i = 0; i < result.Length; i++)
                {
                    points.Add(result[i].ParseAsCustomerM());
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
            return points;
        }

        #endregion


        #region Command

        [RelayCommand]
        async Task ChangeDay()
        {
            try
            {
                if (Routes is null)
                {
                    await Shell.Current.DisplayAlert("Brak trasy", "Zapisywanie jest dostępne tylko po wybraniu trasy konkretnego kierowcy", "Ok");
                    return;
                }
                var popup = new Popups.SelectDay.SelectDayV();
                var response = await Shell.Current.ShowPopupAsync(popup);
                if (response is null)
                {
                    return;
                }
                if (response is SelectedDayOfWeekRoutes day)
                {
                    LastSelectedDayOfWeek = day;
                    SelectedDayName = day.ToString();
                    AllPoints = await GetSelectedDays(LastSelectedDayOfWeek);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
            IsVisibleList = !IsVisibleList;

            //if (MapsPoint.Length < AllPoints.Count && IsVisibleList)
            //{
            //    MapsPoint = AllPoints.ToArray();
            //}
        }

        [RelayCommand]
        void LocationOfPin(MapsM mapsM)
        {
            if (mapsM is null)
                return;

            var mapSpan = new MapSpan(mapsM.Pin.Location, 0.005, 0.005);
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

        [RelayCommand]
        async Task DisplayDescriptionPin(MapsM point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }

                await Shell.Current.GoToAsync($"{nameof(Pages.Customer.DisplayCustomer.DisplayCustomerV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(CustomerRoutes)] = new CustomerRoutes()
                        {
                            Id = new Guid(point.Id.ToByteArray()),
                            RoutesId = new Guid(point.RoutesId.ToByteArray()),
                            QueueNumber = point.Index,
                            Name = point.Name,
                            Description = point.Description,
                            PhoneNumber = point.PhoneNumber,
                            Created = point.Created,
                            DayOfWeek = point.SelectedDayOfWeek,
                            ResidentialAddress = point.ResidentialAddress,
                            Longitude = point.Longitude,
                            Latitude = point.Latitude,
                        }
                        ,
                        [nameof(SelectedDayOfWeekRoutes)] = LastSelectedDayOfWeek
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task CurrentLocationNewPin()
        {
            try
            {
                if (Routes is null)
                {
                    await Shell.Current.DisplayAlert("Brak trasy", "Wczytywanie jest dostępne tylko po wybraniu trasy konkretnego kierowcy", "Ok");
                    return;
                }
                var mapSpan = await GetCurrentLocation();
                if (mapSpan is null) { return; }
                OnGoToLocation(mapSpan);

                var customer = new CustomerRoutes()
                {
                    Created = DateTime.Now,
                    Longitude = mapSpan.Center.Longitude,
                    Latitude = mapSpan.Center.Latitude,
                    RoutesId = Routes.Id,
                };

                await Shell.Current.GoToAsync($"{nameof(Pages.Customer.AddCustomer.AddCustomerV)}"
                    , new Dictionary<string, object>
                    {
                        [nameof(CustomerRoutes)] = customer,
                        [nameof(Routes)] = Routes,
                    });

            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        #endregion

    }
}
