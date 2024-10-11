
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;
using DriversRoutes.Pages.Popups.MoveTimeOnCustomers;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

using MudBlazor;

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

        [ObservableProperty]
        bool isTrafficEnabled;

        public SelectedDayOfWeekRoutes LastSelectedDayOfWeek { get; set; }
        public SelectedDayOfWeekRoutes LastSelectedDayOfWeekWhenNavigation { get; set; }

        public Routes Routes { get; set; }
        public bool AddLocationIs { get; set; } = false;

        const string enable = "Dostępne";
        const string block = "Zablokowane";

        public Action<MapSpan> GoToLocation;
        public Microsoft.Maui.Controls.Maps.Map GetMap { get; set; }

        public readonly MapSpan szarotka = CurrentLocation.Szarotka;

        readonly DataBase.Data.AccessDataBase _db;
        readonly Service.ISelectRoutes _selectRoutes;
        readonly Service.ISaveRoutes _saveRoutes;

        private static List<ImageSource> pinImage = [];

        #endregion
        public MapsVM(DataBase.Data.AccessDataBase db, Service.ISelectRoutes selectRoutes, Service.ISaveRoutes saveRoutes)
        {
            _db = db;
            MapType = MapType.Street;
            AllPoints ??= [];
            _selectRoutes = selectRoutes;
            _saveRoutes = saveRoutes;
            //for (int i = 0; i < 200; i++)
            //{
            //    AllPoints.Add(new MapsM().CreateRandomPoint(i));
            //}

            //AllPoints.FirstOrDefault().Pin.Location = new Location(49.7488002173044, 20.408379427432106);
            // MapsPoint = AllPoints.ToArray();
        }

        public void Dispose()
        {
            AllPoints.Clear();
            MapsPoint = null;
            Geolocation.LocationChanged -= StartListening;
            _db.Dispose();
            pinImage.Clear();

            GC.SuppressFinalize(this);
        }

        #region Method
        public async Task<MapSpan> GetCurrentLocation()
        {
            try
            {
                return await CurrentLocation.Get();
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
        public async void StartListeningLocation(Microsoft.Maui.Controls.Maps.Map map)
        {
            try
            {
                var result = await Geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(10)));
                Geolocation.LocationChanged += StartListening;
                // Geolocation.LocationChanged += (sender, e) =>
                // {
                //     var location = e.Location;
                //     var radius = map.VisibleRegion.Radius;
                //     map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), radius));
                // };
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
                var first = AllPoints.FirstOrDefault();
                if (first is not null)
                {
                    MapsPoint = [first];
                }
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
                        SkiaBitmapExportContext skiaBitmapExportContext = new(width, height, 1);
                        ICanvas canvas = skiaBitmapExportContext.Canvas;
                        DrawIconOnMap drawIconOnMap = new()
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
            int index;
            if (MapsPoint is null)
            {
                return;
            }

            if (MapsPoint.Length <= 0)
            {
                return;
            }

            index = MapsPoint.FirstOrDefault().Index + direction;

            if (index > AllPoints.Count)
            {
                index = 1;
            }
            else if (index < 1)
            {
                index = AllPoints.Count;
            }

            MapsPoint = AllPoints.Where(x => x.Index == index).ToArray();
            if (MapsPoint.Length <= 0)
            {
                return;
            }
            var mapSpan = new MapSpan(MapsPoint.FirstOrDefault().Pin.Location, 0.05, 0.05);
            OnGoToLocation(mapSpan);
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
                    AllPoints = await GetSelectedDays(LastSelectedDayOfWeek);
                    var first = AllPoints.FirstOrDefault();
                    if (first is not null)
                    {
                        MapsPoint = [first];
                    }
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


        #endregion

    }
}
