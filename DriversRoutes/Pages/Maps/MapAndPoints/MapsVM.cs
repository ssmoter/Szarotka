using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Model.EntitiesRoutes;

using DriversRoutes.Helper;
using DriversRoutes.Pages.Popups.MoveTimeOnCustomers;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

using Shared.CustomControls;
using Shared.Data;

using System.Collections.ObjectModel;

namespace DriversRoutes.Pages.Maps.MapAndPoints;

public partial class MapsVM : ObservableObject, IDisposable, IQueryAttributable
{
    #region Variable

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(Routes), out var routes))
        {
            if (routes is Routes _routes)
            {
                Routes = _routes;
            }
        }
        if (query.TryGetValue(nameof(MapsM), out var allPoints))
        {
            if (allPoints is ObservableCollection<MapsM> _allPoints)
            {
                AllPoints = _allPoints;
            }
        }
        if (query.TryGetValue(nameof(SelectedDayOfWeekRoutes), out var lastSelectedDayOfWeek))
        {
            if (lastSelectedDayOfWeek is SelectedDayOfWeekRoutes _lastSelectedDayOfWeek)
            {
                LastSelectedDayOfWeek = _lastSelectedDayOfWeek;
            }
        }
    }

    private ObservableCollection<MapsM> allPoints = [];
    public ObservableCollection<MapsM> AllPoints
    {
        get => allPoints;
        set
        {
            if (SetProperty(ref allPoints, value, nameof(AllPoints))) { }
        }
    }

    private MapsM selectedPoint = new();
    public MapsM SelectedPoint
    {
        get => selectedPoint;
        set
        {
            if (SetProperty(ref selectedPoint, value, nameof(SelectedPoint))) { }
        }
    }

    private string driversRoutesName = "Trasa kierowcy: ";
    public string DriversRoutesName
    {
        get => driversRoutesName;
        set
        {
            if (SetProperty(ref driversRoutesName, value, nameof(DriversRoutesName))) { }
        }
    }

    private string selectedDayName = "";
    public string SelectedDayName
    {
        get => selectedDayName;
        set
        {
            if (SetProperty(ref selectedDayName, value, nameof(SelectedDayName))) { }
        }
    }

    private bool isVisibleTypeOfMap;
    public bool IsVisibleTypeOfMap
    {
        get => isVisibleTypeOfMap;
        set
        {
            if (SetProperty(ref isVisibleTypeOfMap, value, nameof(IsVisibleTypeOfMap))) { }
        }
    }

    private MapType mapType;
    public MapType MapType
    {
        get => mapType;
        set
        {
            if (SetProperty(ref mapType, value, nameof(MapType))) { }
        }
    }

    private string addLocationIsText = _block;
    public string AddLocationIsText
    {
        get => addLocationIsText;
        set
        {
            if (SetProperty(ref addLocationIsText, value, nameof(AddLocationIsText))) { }
        }
    }

    private bool isTrafficEnabled;
    public bool IsTrafficEnabled
    {
        get => isTrafficEnabled;
        set
        {
            if (SetProperty(ref isTrafficEnabled, value, nameof(IsTrafficEnabled))) { }
        }
    }

    private StepSelected stepSelected = Shared.CustomControls.StepSelected.One;
    public StepSelected StepSelected
    {
        get => stepSelected;
        set
        {
            if (SetProperty(ref stepSelected, value, nameof(StepSelected))) { }
        }
    }

    private bool isRefreshing;
    public bool IsRefreshing
    {
        get => isRefreshing;
        set
        {
            if (SetProperty(ref isRefreshing, value, nameof(IsRefreshing))) { }
        }
    }

    private bool routeIsVisible = false;
    public bool RouteIsVisible
    {
        get => routeIsVisible;
        set
        {
            if (SetProperty(ref routeIsVisible, value, nameof(RouteIsVisible))) { }
        }
    }

    private string routeDistance;
    public string RouteDistance
    {
        get => routeDistance;
        set
        {
            if (SetProperty(ref routeDistance, value, nameof(RouteDistance))) { }
        }
    }
    private TimeSpan routeDuration;
    public TimeSpan RouteDuration
    {
        get => routeDuration;
        set
        {
            if (SetProperty(ref routeDuration, value, nameof(RouteDuration))) { }
        }
    }

    public CancellationTokenSource RoutesToken = new();
    public SelectedDayOfWeekRoutes LastSelectedDayOfWeek { get; set; }
    public SelectedDayOfWeekRoutes LastSelectedDayOfWeekWhenNavigation { get; set; }

    public Routes Routes { get; set; }
    public bool AddLocationIs { get; set; } = false;

    const string _enable = "Dostępne";
    const string _block = "Zablokowane";
    private int _previousCustomerRoute = -1;

    public Action<MapSpan> GoToLocationAction;
    public Action<Polyline> AddRoutesPolilineAction;
    public Action ClearRoutesPolilineAction;
    public Microsoft.Maui.Controls.Maps.Map GetMap { get; set; }
    private readonly IAccessDataBase _db;
    private readonly DataBase.Data.Get.IGetDriverRoutesAoT _get;
    private readonly DataBase.Data.Save.ISaveDriverRoutesAoT _save;
    private readonly Data.GoogleApi.IRoutes _routes;

    #endregion
    public MapsVM(IAccessDataBase db,
                  Data.GoogleApi.IRoutes routes,
                  DataBase.Data.Get.IGetDriverRoutesAoT get,
                  DataBase.Data.Save.ISaveDriverRoutesAoT save)
    {
        _db = db;
        MapType = MapType.Street;
        AllPoints ??= [];
        _routes = routes;
        _get = get;
        _save = save;
    }

    public void Dispose()
    {
        AllPoints.Clear();
        _db.Dispose();
    }

    #region Method

    public void AutomaticUpdateLocation(Location location)
    {
        var radius = GetMap.VisibleRegion.Radius;
        GetMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, radius));
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

    public void GetSelectedDaysAndForget(SelectedDayOfWeekRoutes week)
    {
        RefreshingStart();
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
                RefreshingEnd();
            });
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
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

            var result = await _get.CustomerRoutes(Routes.Id, week.GetDayOfWeeks());
            for (int i = 0; i < result.Count; i++)
            {
                points.Add(result[i].ParseAsCustomerM());
#if !DEBUG
                    var image = Data.DrawIconOnMap.GetImagePin(points[i].CustomerRoutes.QueueNumber);
                    points[i].Pin.ImageSource = image;
#endif
            }
            SelectedDayName = week.ToString();
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
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
        ClearRoutes();
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
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            IsRefreshing = false;
        });
    }

    private async Task CalculateRoutes(CustomerRoutes customer, CancellationToken token = default)
    {
        try
        {
            ClearRoutesPolilineAction?.Invoke();
            Data.ActionLocation.MapGeolocation.OnStopListeningLocation();

            var snackBar = new Snackbar()
            {
                Text = "Anuluj pobieranie trasy",
                Action = () =>
                {
                    RoutesToken?.Cancel();
                },
                ActionButtonText = "Anuluj"
            }.Show(RoutesToken.Token);


            var current = await Data.ActionLocation.CurrentLocation.Get(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1), token);


            var request = new Model.Route.ComputeRoutesRequest()
            {
                Origin = new Model.Route.Waypoint()
                {
                    Location = new Model.Route.Location()
                    {
                        LatLng = new Model.Route.LatLng()
                        {
                            Longitude = current.Center.Longitude,
                            Latitude = current.Center.Latitude
                        }
                    }
                },
                Destination = new Model.Route.Waypoint()
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
            };

            var response = await _routes.GetOnlyRouteStepsDurationDistance(request, token);

            var firstRoute = response.Routes.FirstOrDefault();

            if (firstRoute is not null)
            {
                if (firstRoute.DistanceMeters < 1000)
                {
                    RouteDistance = $"{firstRoute.DistanceMeters}m";
                }
                else
                {
                    var distance = firstRoute.DistanceMeters.ToString();
                    RouteDistance = $"{distance[0]}.{distance[1]}{distance[2]}";
                }
                RouteDuration = TimeSpan.FromSeconds(GetOnlySeconds(firstRoute.Duration));
                static int GetOnlySeconds(ReadOnlySpan<char> duration)
                {
                    var seconds = int.Parse(duration.Slice(0, duration.Length - 1));
                    return seconds;
                }


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
                await Data.ActionLocation.MapGeolocation.OnStartListeningLocation((
                     (location, token) =>
                     {
                         AutomaticUpdateLocation(location);
                     })
                     , GeolocationAccuracy.Best, TimeSpan.FromSeconds(1), token);
                await Toast.Make($"Droga:{RouteDistance}{Environment.NewLine}Czas:{RouteDuration}").Show(default);
            }
        }
        catch (OperationCanceledException)
        {
            await Toast.Make("Pobieranie trasy anulowane").Show(default);
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
        }
        finally
        {
            RefreshingEnd();
        }
    }
    private void SetRoutePinColor(int current)
    {
        if (_previousCustomerRoute > -1)
        {
            var customerPrevious = AllPoints.FirstOrDefault(x => x.CustomerRoutes.QueueNumber == _previousCustomerRoute);
            if (customerPrevious is not null)
            {
#if !DEBUG
                    var pinPrevious = Data.DrawIconOnMap.GetImagePin(_previousCustomerRoute);
                    customerPrevious.Pin.ImageSource = pinPrevious;
#else
                customerPrevious.Pin.ImageSource = null;
#endif

                AllPoints.Remove(customerPrevious);
                AllPoints.Add(customerPrevious);
            }
        }
        var customer = AllPoints.FirstOrDefault(x => x.CustomerRoutes.QueueNumber == current);
        if (customer is not null)
        {
            var pinCurrent = Data.DrawIconOnMap.GetImagePin(current, Colors.Blue, Colors.AliceBlue);
            customer.Pin.ImageSource = pinCurrent;
            _previousCustomerRoute = current;
            AllPoints.Remove(customer);
            AllPoints.Add(customer);
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
            _db.SaveLogExtension(ex);
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
            AddLocationIsText = _enable;
        }
        if (!AddLocationIs)
        {
            AddLocationIsText = _block;
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
            _db.SaveLogExtension(ex);
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
            var mapSpan = await Data.ActionLocation.CurrentLocation.Get(GeolocationAccuracy.Best, TimeSpan.FromSeconds(1));

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
            _db.SaveLogExtension(ex);
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
            var result = await MoveTimeOnCustomersV.ShowPopUp(Routes, selectDayMs, _get, _save);
            if (result)
            {
                GetSelectedDaysAndForget(LastSelectedDayOfWeek);
            }
        }
        catch (Exception ex)
        {
            _db.SaveLogExtension(ex);
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
            _db.SaveLogExtension(ex);
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
        RoutesToken = new();
        RefreshingStart();
        var task = CalculateRoutes(customer, RoutesToken.Token);
        await task;
        if (task.IsCompletedSuccessfully)
        {
            SetRoutePinColor(customer.QueueNumber);
        }
    }
    [RelayCommand]
    async Task GetRoutesMapsM(MapsM customer)
    {
        if (customer is null)
        {
            return;
        }
        RoutesToken = new();
        RefreshingStart();
        var task = CalculateRoutes(customer.CustomerRoutes, RoutesToken.Token);
        await task;
        if (task.IsCompletedSuccessfully)
        {
            SetRoutePinColor(customer.CustomerRoutes.QueueNumber);
        }
    }

    [RelayCommand]
    void ClearRoutes()
    {
        ClearRoutesPolilineAction?.Invoke();
        RouteIsVisible = false;
        Data.ActionLocation.MapGeolocation.OnStopListeningLocation();
    }

    #endregion

}

