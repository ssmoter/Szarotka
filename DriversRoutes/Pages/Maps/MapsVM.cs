using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DriversRoutes.Helper;
using DriversRoutes.Model;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Maps
{
    [QueryProperty(nameof(Routes), nameof(Model.Routes))]
    [QueryProperty(nameof(AllPoints), nameof(MapsM))]
    public partial class MapsVM : ObservableObject
    {
        #region Variable
        [ObservableProperty]
        ObservableCollection<MapsM> allPoints;

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

        public Routes Routes { get; set; }
        public bool AddLocationIs { get; set; } = false;

        const string enable = "Dostępne";
        const string block = "Zablokowane";

        public Action<MapSpan> GoToLocation;

        public readonly MapSpan szarotka = new(new Location(49.74918684945343, 20.40889755094227), 0.1, 0.1);

        readonly DataBase.Data.AccessDataBase _db;
        readonly Service.ISelectRoutes _selectRoutes;
        #endregion
        public MapsVM(DataBase.Data.AccessDataBase db, Service.ISelectRoutes selectRoutes)
        {
            _db = db;
            SelectedDayName = DisplaySelectedDayName(DateTime.Today.DayOfWeek);
            MapType = MapType.Street;
            AllPoints ??= new();
            _selectRoutes = selectRoutes;
            //for (int i = 0; i < 200; i++)
            //{
            //    AllPoints.Add(new MapsM().CreateRandomPoint(i));
            //}

            //AllPoints.FirstOrDefault().Pin.Location = new Location(49.7488002173044, 20.408379427432106);
            //MapsPoint = AllPoints.ToArray();
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
        async Task<ObservableCollection<MapsM>> DisplaySelectedDayNameAsync(DayOfWeek day)
        {
            var week = new Model.SelectedDayOfWeekRoutes();
            switch (day)
            {
                case DayOfWeek.Sunday:
                    week.Sunday = true;
                    break;
                case DayOfWeek.Monday:
                    week.Monday = true;
                    break;
                case DayOfWeek.Tuesday:
                    week.Tuesday = true;
                    break;
                case DayOfWeek.Wednesday:
                    week.Wednesday = true;
                    break;
                case DayOfWeek.Thursday:
                    week.Thursday = true;
                    break;
                case DayOfWeek.Friday:
                    week.Friday = true;
                    break;
                case DayOfWeek.Saturday:
                    week.Saturday = true;
                    break;
                default:
                    break;
            }
            var points = await GetSelectedDays(week);
            return points;
        }

        public void OnGoToLocation(MapSpan mapSpan)
        {
            GoToLocation?.Invoke(mapSpan);
        }
        public void OpenMoreDetail(Pin pin)
        {
            SwipeViewGesture();
            MapsPoint = AllPoints.Where(x => x.Description == pin.Address).ToArray();
        }

        public async Task AddNewPoint(Model.CustomerRoutes customer)
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
                if (customer.Created == 0)
                {
                    customer.CreatedDate = DateTime.Now;
                }
                var point = new MapsM
                {
                    Id = new Guid(customer.Id.ToByteArray()),
                    RoutesId = new Guid(customer.RoutesId.ToByteArray()),
                    Index = AllPoints.Count,
                    Name = customer.Name,
                    Description = customer.Description,
                    PhoneNumber = customer.PhoneNumber,
                    Created = customer.CreatedDate,
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

        async Task<ObservableCollection<MapsM>> GetSelectedDays(Model.SelectedDayOfWeekRoutes week)
        {
            var result = await _selectRoutes.GetCustomerRoutesQuery(Routes, week);
            var points = new ObservableCollection<MapsM>();
            for (int i = 0; i < result.Length; i++)
            {
                points.Add(result[i].ParseAsCustomerM());
            }
            return points;
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
                await Task.Run(async () =>
                 {
                     AllPoints = await DisplaySelectedDayNameAsync(day);

                 });
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

        [RelayCommand]
        async Task EditPin(MapsM point)
        {
            try
            {
                if (point is null)
                {
                    return;
                }

                var popup = new Popups.AddCustomer.AddCustomerV(new Model.CustomerRoutes()
                {
                    Id = new Guid(point.Id.ToByteArray()),
                    RoutesId = new Guid(point.RoutesId.ToByteArray()),
                    Index = point.Index,
                    Name = point.Name,
                    Description = point.Description,
                    PhoneNumber = point.PhoneNumber,
                    CreatedDate = point.Created,
                    DayOfWeek = point.SelectedDayOfWeek,
                    Longitude = point.Longitude,
                    Latitude = point.Latitude,
                });

                var update = await Shell.Current.ShowPopupAsync(popup);
                if (update is null)
                {
                    return;
                }
                int index = AllPoints.IndexOf(point);

                if (update is CustomerRoutes customerUpdate)
                {
                    AllPoints[index].Id = new Guid(customerUpdate.Id.ToByteArray());
                    AllPoints[index].RoutesId = new Guid(customerUpdate.RoutesId.ToByteArray());
                    AllPoints[index].Index = customerUpdate.Index;
                    AllPoints[index].Name = customerUpdate.Name;
                    AllPoints[index].Description = customerUpdate.Description;
                    AllPoints[index].PhoneNumber = customerUpdate.PhoneNumber;
                    AllPoints[index].Created = customerUpdate.CreatedDate;
                    AllPoints[index].SelectedDayOfWeek = customerUpdate.DayOfWeek;
                    AllPoints[index].SelectedDayOfWeek.Id = new Guid(customerUpdate.DayOfWeek.Id.ToByteArray());
                    AllPoints[index].SelectedDayOfWeek.CustomerId = new Guid(customerUpdate.DayOfWeek.CustomerId.ToByteArray());
                    AllPoints[index].Longitude = customerUpdate.Longitude;
                    AllPoints[index].Latitude = customerUpdate.Latitude;


                    await _db.DataBaseAsync.UpdateAsync(customerUpdate);
                    await _db.DataBaseAsync.UpdateAsync(customerUpdate.DayOfWeek);
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task CurrentLocationPin()
        {
            try
            {
                if (Routes is null)
                {
                    await Shell.Current.DisplayAlert("Brak trasy", "Zapisywanie jest dostępne tylko po wybraniu trasy konkretnego kierowcy", "Ok");
                    return;
                }
                var mapSpan = await GetCurrentLocation();
                if (mapSpan is null) { return; }
                OnGoToLocation(mapSpan);

                var popup = new Popups.AddCustomer.AddCustomerV(new CustomerRoutes()
                {
                    Longitude = mapSpan.Center.Longitude,
                    Latitude = mapSpan.Center.Latitude,
                    RoutesId = new Guid(Routes.Id.ToByteArray()),
                });

                var result = await Shell.Current.ShowPopupAsync(popup);
                if (result is Model.CustomerRoutes customerUpdate)
                {
                    await AddNewPoint(customerUpdate);
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
