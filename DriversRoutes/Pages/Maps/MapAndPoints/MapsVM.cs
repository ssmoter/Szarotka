
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.CustomControls;
using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;
using DriversRoutes.Pages.Popups.MoveTimeOnCustomers;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

using System.Collections.ObjectModel;

using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace DriversRoutes.Pages.Maps.MapAndPoints
{
    [QueryProperty(nameof(Routes), nameof(DataBase.Model.EntitiesRoutes.Routes))]
    [QueryProperty(nameof(AllPoints), nameof(MapsM))]
    [QueryProperty(nameof(LastSelectedDayOfWeek), nameof(SelectedDayOfWeekRoutes))]

    public partial class MapsVM : ObservableObject, IDisposable
    {
        #region Variable
        [ObservableProperty]
        ObservableCollection<MapsM> allPoints;

        [ObservableProperty]
        MapsM selectedPoint;

        [ObservableProperty]
        string driversRoutesName = "Trasa kierowcy: ";

        [ObservableProperty]
        string selectedDayName = "";

        [ObservableProperty]
        bool isVisibleTypeOfMap;

        [ObservableProperty]
        MapType mapType;

        [ObservableProperty]
        string addLocationIsText = block;

        [ObservableProperty]
        bool isTrafficEnabled;

        [ObservableProperty]
        StepSelected stepSelected = StepSelected.One;

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        bool routeIsVisible = false;

        public SelectedDayOfWeekRoutes LastSelectedDayOfWeek { get; set; }
        public SelectedDayOfWeekRoutes LastSelectedDayOfWeekWhenNavigation { get; set; }

        public Routes Routes { get; set; }
        public bool AddLocationIs { get; set; } = false;

        const string enable = "Dostępne";
        const string block = "Zablokowane";


        public Action<MapSpan> GoToLocationAction;
        public Action<Polyline> AddRoutesPolilineAction;
        public Action ClearRoutesPolilineAction;
        public Microsoft.Maui.Controls.Maps.Map GetMap { get; set; }

        public readonly MapSpan szarotka = CurrentLocation.Szarotka;

        private readonly DataBase.Data.AccessDataBase _db;
        private readonly Service.ISelectRoutes _selectRoutes;
        private readonly Service.ISaveRoutes _saveRoutes;
        private readonly Data.GoogleApi.IRoutes _routes;
        private static List<ImageSource> pinImage = [];

        #endregion
        public MapsVM(DataBase.Data.AccessDataBase db, Service.ISelectRoutes selectRoutes, Service.ISaveRoutes saveRoutes, Data.GoogleApi.IRoutes routes)
        {
            _db = db;
            MapType = MapType.Street;
            AllPoints ??= [];
            _selectRoutes = selectRoutes;
            _saveRoutes = saveRoutes;
            _routes = routes;
        }

        public void Dispose()
        {
            AllPoints.Clear();
            Geolocation.LocationChanged -= StartListening;
            _db.Dispose();
            for (int i = 0; i < pinImage.Count; i++)
            {
                pinImage[i] = null;
            }
            pinImage.Clear();
        }

        #region Method
        public async Task<MapSpan> GetCurrentLocation()
        {
            try
            {
                return await CurrentLocation.Get(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                return szarotka;
            }
        }

        public void StopListeningLocation()
        {
            Geolocation.StopListeningForeground();
            Geolocation.LocationChanged -= StartListening;
        }
        public async void StartListeningLocation()
        {
            try
            {
                var result = await Geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10)));
                Geolocation.LocationChanged += StartListening;
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

        private void StartListening(object sender, GeolocationLocationChangedEventArgs e)
        {
            var location = e.Location;
            var radius = GetMap.VisibleRegion.Radius;
            GetMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), radius));
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
            GoToLocationAction?.Invoke(mapSpan);
        }
        public void OnSetRoutesPolyline(Polyline poluline)
        {
            AddRoutesPolilineAction?.Invoke(poluline);
        }
        public void OpenMoreDetail(Pin pin)
        {
            var index = MapsM.GetIndex(pin.Label);
            if (index == -1)
                return;

            SelectedPoint = AllPoints.FirstOrDefault(x => x.CustomerRoutes.QueueNumber == index);
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
                    CustomerRoutes = new()
                    {
                        Id = new Guid(customer.Id.ToByteArray()),
                        RoutesId = new Guid(customer.RoutesId.ToByteArray()),
                        QueueNumber = customer.QueueNumber,
                        Name = customer.Name,
                        Description = customer.Description,
                        PhoneNumber = customer.PhoneNumber,
                        Created = customer.Created,
                        DayOfWeek = customer.DayOfWeek,
                        Longitude = customer.Longitude,
                        Latitude = customer.Latitude,
                    }
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
        public void GetSelectedDaysAndForget(SelectedDayOfWeekRoutes week)
        {
            try
            {
                AllPoints.Clear();

                Task.Run(async () =>
                {
                    AllPoints = await GetSelectedDays(week);
                    var first = AllPoints.FirstOrDefault();
                    if (first is not null)
                    {
                        SelectedPoint = first;
                    }
                });
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

                int scaleX = (int)DeviceDisplay.Current.MainDisplayInfo.Density;
                int scaleY = scaleX;

                var width = 40 * scaleX;
                var height = 58 * scaleY;

                var result = await _selectRoutes.GetCustomerRoutesQueryAsync(Routes, week);
                for (int i = 0; i < result.Length; i++)
                {
                    points.Add(result[i].ParseAsCustomerM());
#if !DEBUG
                    if (pinImage.Count <= i)
                    {
                        Microsoft.Maui.Graphics.Skia.SkiaBitmapExportContext skiaBitmapExportContext = new(width, height, 1);
                        ICanvas canvas = skiaBitmapExportContext.Canvas;
                        Data.DrawIconOnMap drawIconOnMap = new()
                        {
                            Number = i + 1,
                            ScaleX = scaleX,
                            ScaleY = scaleY,
                        };
                        drawIconOnMap.Draw(canvas, new RectF(0, 0, skiaBitmapExportContext.Width, skiaBitmapExportContext.Height));

                        var image = ImageSource.FromStream(() => skiaBitmapExportContext.Image.AsStream());
                        pinImage.Add(image);
                        points[i].Pin.ImageSource = image;
                    }
                    else
                    {
                        points[i].Pin.ImageSource = pinImage[i];
                    }
#endif
                }
                SelectedDayName = week.ToString();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
            return points;
        }
        private void DescriptionOfPreviousPoint(int direction)
        {
            if (SelectedPoint is null)
            {
                return;
            }
            int index;
            index = SelectedPoint.CustomerRoutes.QueueNumber + direction;
            if (index > AllPoints.Count)
            {
                index = 1;
            }
            else if (index < 1)
            {
                index = AllPoints.Count;
            }

            SelectedPoint = AllPoints.FirstOrDefault(x => x.CustomerRoutes.QueueNumber == index);
            if (SelectedPoint is null)
            {
                return;
            }
            var mapSpan = new MapSpan(SelectedPoint.Pin.Location, 0.05, 0.05);
            OnGoToLocation(mapSpan);
        }

        private void RefreshingStart()
        {
            Task.Run(() =>
            {
                IsRefreshing = true;
            });
        }
        private void RefreshingEnd()
        {
            Task.Run(() =>
            {
                IsRefreshing = false;
            });
        }
        private async Task CalculateRoutes(CustomerRoutes customer, CancellationToken token = default)
        {
            try
            {
                RefreshingStart();
                ClearRoutesPolilineAction?.Invoke();
                var current = await Helper.CurrentLocation.Get(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1), token);

                var request = new Model.Route.ComputeRoutesRequest()
                {
                    Origin = new Model.Route.Waypoint()
                    {
                        Location = new Model.Route.Location()
                        {
                            LatLng = new Model.Route.LatLng()
                            {
                                Latitude = customer.Latitude,
                                Longitude = customer.Longitude
                            }
                        }
                    },
                    Destination = new Model.Route.Waypoint()
                    {
                        Location = new Model.Route.Location()
                        {
                            LatLng = new Model.Route.LatLng()
                            {
                                Longitude = current.Center.Longitude,
                                Latitude = current.Center.Latitude
                            }
                        }
                    }
                };

                var response = await _routes.Compute("*", request, token);

                for (int i = 0; i < response.Routes.Length; i++)
                {
                    for (int j = 0; j < response.Routes[i].Legs.Length; j++)
                    {
                        for (int k = 0; k < response.Routes[i].Legs[j].Steps.Length; k++)
                        {
                            var decode = DriversRoutes.Data.GoogleApi.DecodeRoutes.DecodePolyline(response.Routes[i].Legs[j].Steps[k].Polyline.EncodedPolyline);
                            var poly = new Polyline()
                            {
                                StrokeColor = Colors.Blue,
                                StrokeWidth = 5,
                            };
                            for (int l = 0; l < decode.Count; l++)
                            {
                                poly.Geopath.Add(decode[l]);
                            }
                            OnSetRoutesPolyline(poly);
                        }
                    }
                }
                RouteIsVisible = true;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
            finally
            {
                RefreshingEnd();
            }
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
                    GetSelectedDaysAndForget(day);
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
                        [nameof(CustomerRoutes)] = point.CustomerRoutes
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

        [RelayCommand]
        void DisplayDescriptionOfNextPoint()
        {
            DescriptionOfPreviousPoint(1);
        }
        [RelayCommand]
        void DisplayDescriptionOfPreviousPoint()
        {
            DescriptionOfPreviousPoint(-1);
        }

        [RelayCommand]
        async Task MoveTimeOnPoints(SelectedDayOfWeekRoutes selectDayMs)
        {
            try
            {
                var result = await MoveTimeOnCustomersV.ShowPopUp(Routes, selectDayMs, _selectRoutes, _saveRoutes);
                if (result)
                {
                    GetSelectedDaysAndForget(LastSelectedDayOfWeek);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        [RelayCommand]
        void ShowMovingView()
        {
            StepSelected = MovingViewInSteps.StepUp(StepSelected);
        }
        [RelayCommand]
        void HideMovingView()
        {
            StepSelected = MovingViewInSteps.StepDown(StepSelected);
        }

        [RelayCommand]
        async Task NavigationToRoutes(CustomerRoutes customer)
        {
            try
            {
                if (customer is null)
                {
                    return;
                }
                var points = new ObservableCollection<CustomerRoutes>(AllPoints.Select(x => x.CustomerRoutes));
                await Shell.Current.GoToAsync($"{nameof(Pages.Maps.Navigate.NavigateV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(ObservableCollection<CustomerRoutes>)] = points,
                        [nameof(CustomerRoutes)] = customer
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        void RefreshView()
        {
            RefreshingEnd();
        }

        [RelayCommand]
        async Task GetRoutes(CustomerRoutes customer)
        {
            if (customer is null)
            {
                return;
            }

            await CalculateRoutes(customer);

        }
        [RelayCommand]
        async Task GetRoutesMapsM(MapsM customer)
        {
            if (customer is null)
            {
                return;
            }
            await CalculateRoutes(customer.CustomerRoutes);
        }

        [RelayCommand]
        void ClearRoutes()
        {
            ClearRoutesPolilineAction?.Invoke();
            RouteIsVisible = false;
        }

        #endregion

    }
}
