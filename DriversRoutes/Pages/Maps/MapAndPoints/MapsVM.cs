using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
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


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        bool isLoaded = false;

        if (query.TryGetValue(nameof(Routes), out var routes))
        {
            if (routes is Routes _routes)
            {
                Routes = _routes;
                if (LastSelectedDayOfWeek is not null && Routes is not null)
                {
                    GetSelectedDaysAndForget(LastSelectedDayOfWeek);
                    isLoaded = true;
                }
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
                if (LastSelectedDayOfWeek is not null && Routes is not null && !isLoaded)
                {
                    GetSelectedDaysAndForget(LastSelectedDayOfWeek);
                }
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

    public Routes Routes { get; set; }
    private bool addLocationIs = false;
    public bool AddLocationIs
    {
        get => addLocationIs;
        set
        {
            if (SetProperty(ref addLocationIs, value, nameof(AddLocationIs))) { }
        }
    }

    const string _enable = "Dostępne";
    const string _block = "Zablokowane";
    private int _previousCustomerRoute = -1;

    public Action<MapSpan> GoToLocationAction;
    public Action<Polyline> AddRoutesPolylineAction;
    public Action ClearRoutesPolylineAction;
    public Microsoft.Maui.Controls.Maps.Map GetMap { get; set; }
    private readonly IAccessDataBase _db;
    private readonly DataBase.Data.Get.IGetDriverRoutesAoT _get;
    private readonly DataBase.Data.Save.ISaveDriverRoutesAoT _save;
    private readonly Data.GoogleApi.IRoutes _routes;
    private readonly DataBase.Service.IUpdateLogService _update;
    private readonly IPopupService _popupService;


    public MapsVM(IAccessDataBase db,
                  Data.GoogleApi.IRoutes routes,
                  DataBase.Data.Get.IGetDriverRoutesAoT get,
                  DataBase.Data.Save.ISaveDriverRoutesAoT save,
                  DataBase.Service.IUpdateLogService update,
                  IPopupService popupService)
    {
        _db = db;
        MapType = MapType.Street;
        AllPoints ??= [];
        _routes = routes;
        _get = get;
        _save = save;
        _update = update;
        _popupService = popupService;
    }

    public void Dispose()
    {
        AllPoints.Clear();
        _db.Dispose();
    }



    private void UpdatePinNumber()
    {
        if (AllPoints is null)
        {
            return;
        }
        bool update = false;
        int number = 0;
        for (int i = 0; i < AllPoints.Count; i++)
        {
            number = i + 1;
            if (AllPoints[i].CustomerRoutes.QueueNumber != number)
            {
                update = true;
                break;
            }
        }
        if (!update)
        {
            return;
        }

        number = 0;
        var sorted = AllPoints.Select(x => x.CustomerRoutes).SortByDays(LastSelectedDayOfWeek.GetDayOfWeeks());
        AllPoints.Clear();
        foreach (var item in sorted)
        {
            number++;
            item.QueueNumber = number;
            var image = Data.DrawIconOnMap.GetImagePin(number);
            AllPoints.Add(item.ParseAsCustomerM(image));
#if !DEBUG
#endif
        }
    }

    public void AutomaticUpdateLocation(Location location)
    {
        var radius = GetMap.VisibleRegion.Radius;
        GetMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, radius));
    }

    public void OnGoToLocation(MapSpan mapSpan)
    {
        GoToLocationAction?.Invoke(mapSpan);
    }
    public void OnSetRoutesPolyline(Polyline polyline)
    {
        AddRoutesPolylineAction?.Invoke(polyline);
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
                if (first is not null && SelectedPoint is null)
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
            var result = await _get.CustomerRoutes(Routes.Id, week.GetDayOfWeeks());
            result = [.. result.SortByDays(week.GetDayOfWeeks())];
            for (int i = 0; i < result.Count; i++)
            {
                int number = i + 1;
                result[i].QueueNumber = number;
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
            ClearRoutesPolylineAction?.Invoke();
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
                    var seconds = int.Parse(duration[..^1]);
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






    [RelayCommand]
    async Task ChangeDay()
    {
        try
        {
            if (Routes is null)
            {
                await Shell.Current.DisplayAlertAsync("Brak trasy", "Zapisywanie jest dostępne tylko po wybraniu trasy konkretnego kierowcy", "Ok");
                return;
            }
            var response = await _popupService.ShowPopupAsync<Popups.SelectDay.SelectDayVM, SelectedDayOfWeekRoutes>(Shell.Current);

            if (response is null)
            {
                return;
            }
            if (response.Result is SelectedDayOfWeekRoutes day)
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
                await Shell.Current.DisplayAlertAsync("Brak trasy", "Wczytywanie jest dostępne tylko po wybraniu trasy konkretnego kierowcy", "Ok");
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
            var result = await MoveTimeOnCustomersV.ShowPopUp(Routes, selectDayMs, _get, _save, _update, _popupService);
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
        ClearRoutesPolylineAction?.Invoke();
        RouteIsVisible = false;
        Data.ActionLocation.MapGeolocation.OnStopListeningLocation();
    }



    [RelayCommand]
    static void RotateAnimation(VisualElement visualElement)
    {
        void StartRotation()
        {
            visualElement.Rotation = 0;
            visualElement.Animate("RotateIcon", new Animation(
                callback: d => visualElement.Rotation = d,
                start: 0,
                end: 360
            ), length: 1000, easing: Easing.Linear, finished: (v, c) =>
            {
                if (!c) StartRotation();
            });
        }

        var isRotate = visualElement.AnimationIsRunning("RotateIcon");
        if (isRotate)
        {
            visualElement.AbortAnimation("RotateIcon");
        }
        else
        {
            StartRotation();
        }

    }

}


