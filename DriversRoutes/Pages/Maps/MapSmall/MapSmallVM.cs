using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesRoutes;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;


namespace DriversRoutes.Pages.Maps.MapSmall;

public partial class MapSmallVM : ObservableObject
{
    [ObservableProperty]
    MapSmallM mapSmallM;

    [ObservableProperty]
    bool routeIsVisible;

    [ObservableProperty]
    string routeDistance;

    [ObservableProperty]
    TimeSpan routeDuration;

    [ObservableProperty]
    bool isFullscreen;

    public Action<MapSpan> MoveToRegion;
    public Action<Microsoft.Maui.Controls.Maps.Pin> AddPin;
    public Func<Microsoft.Maui.Controls.Maps.Pin, bool> RemovePin;
    public Func<MapSpan> GetCurrentLocation;
    public Action<Microsoft.Maui.Devices.Sensors.Location> EditCustomerLocation;
    public Action RemoveRoute;
    public Action<Polyline> AddRoute;
    public Func<MapSpan> VisibleRegion;

    private readonly DataBase.Data.AccessDataBase _db;
    private readonly Data.GoogleApi.IRoutes _routes;
    private Pin _pin;
    CancellationTokenSource _tokenSource;
    public MapSmallVM(DataBase.Data.AccessDataBase db, Data.GoogleApi.IRoutes routes)
    {
        MapSmallM = new();
        _tokenSource = new();
        _db = db;

        _pin = new Pin()
        {
            Label = "Nowa lokalizacja"
        };
        _routes = routes;
    }


    public void OnAddPin(Microsoft.Maui.Controls.Maps.Pin pin)
    {
        AddPin?.Invoke(pin);
    }
    public bool OnRemovePin(Microsoft.Maui.Controls.Maps.Pin pin)
    {
        return (bool)RemovePin?.Invoke(pin);
    }
    public void OnGoToLocation(MapSpan mapSpan)
    {
        MoveToRegion?.Invoke(mapSpan);
    }
    public MapSpan OnGetCurrentLocation()
    {
        return GetCurrentLocation?.Invoke();
    }
    public void OnEditCustomerLocation(Microsoft.Maui.Devices.Sensors.Location location)
    {
        EditCustomerLocation?.Invoke(location);
    }
    public void OnRemoveRoute()
    {
        RemoveRoute?.Invoke();
    }
    public void OnAddRoute(Polyline polyline)
    {
        AddRoute?.Invoke(polyline);
    }


    private async Task CalculateRoutes(CustomerRoutes customer, CancellationToken token = default)
    {
        try
        {
            OnRemoveRoute();
            Data.ActionLocation.MapGeolocation.OnStopListeningLocation();

            var snackBar = new Snackbar()
            {
                Text = "Anuluj pobieranie trasy",
                Action = () =>
                {
                    _tokenSource?.Cancel();
                },
                ActionButtonText = "Anuluj"
            }.Show(_tokenSource.Token);


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
                            OnAddRoute(poly);
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
            }
        }
        catch (OperationCanceledException)
        {
            await Toast.Make("Pobieranie trasy anulowane").Show(default);
        }
        catch (Exception ex)
        {
            _db.SaveLog(ex);
        }
    }
    public void AutomaticUpdateLocation(Location location)
    {
        var radius = OnGetCurrentLocation().Radius;
        OnGoToLocation(MapSpan.FromCenterAndRadius(location, radius));
    }

    [RelayCommand]
    void SetOldLocation(CustomerRoutes customer)
    {
        if (customer is null)
        {
            return;
        }

        OnRemovePin(_pin);

        customer.Latitude = MapSmallM.OldLatitude;
        customer.Longitude = MapSmallM.OldLongitude;

        OnGoToLocation(new MapSpan(
            new Microsoft.Maui.Devices.Sensors.Location(
                customer.Latitude, customer.Longitude)
            , 0.01, 0.01));

        MapSmallM.SaveLocation = false;
        MapSmallM.ChangeLocation = true;
    }

    [RelayCommand]
    async Task SaveNewLocation(CustomerRoutes customer)
    {
        try
        {
            var result = await Shell.Current.DisplayAlert("Nowa lokalizacja", "Czy chcesz nadpisać lokalizacje", "Tak", "Nie");
            if (!result)
            {
                return;
            }

            var location = OnGetCurrentLocation().Center;

            OnRemovePin(_pin);
            _pin.Location = location;
            OnAddPin(_pin);
            customer.Latitude = location.Latitude;
            customer.Longitude = location.Longitude;

            MapSmallM.SaveLocation = false;
            MapSmallM.ChangeLocation = true;
            await _db.DataBaseAsync.UpdateAsync(customer);

        }
        catch (Exception ex)
        {
            _db.SaveLog(ex);
        }
    }
    [RelayCommand]
    async Task ChangeLocation(CustomerRoutes customer)
    {
        try
        {
            if (customer is null)
            {
                return;
            }

            MapSmallM.SaveLocation = true;
            MapSmallM.ChangeLocation = false;

            MapSmallM.OldLatitude = customer.Latitude;
            MapSmallM.OldLongitude = customer.Longitude;

            while (MapSmallM.SaveLocation)
            {
                var location = OnGetCurrentLocation().Center;
                OnEditCustomerLocation(location);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }


        }
        catch (Exception ex)
        {
            _db.SaveLog(ex);
        }
    }
    [RelayCommand]
    void CenterOldLocation(CustomerRoutes customer)
    {
        if (customer is null)
            return;

        OnGoToLocation(new MapSpan(
            new Microsoft.Maui.Devices.Sensors.Location(
                MapSmallM.OldLatitude, MapSmallM.OldLongitude)
            , 0.01, 0.01));
    }
    [RelayCommand]
    void CloseMap(CustomerRoutes customer)
    {
        MapSmallM.SaveLocation = false;
        MapSmallM.ChangeLocation = true;
    }

    [RelayCommand]
    async Task GetRoutes(CustomerRoutes customer)
    {
        if (customer is null)
        {
            return;
        }
        _tokenSource = new();
        await CalculateRoutes(customer, _tokenSource.Token);
    }

    [RelayCommand]
    void ClearRoutes()
    {
        OnRemoveRoute();
        RouteIsVisible = false;
        Data.ActionLocation.MapGeolocation.OnStopListeningLocation();
    }
}
