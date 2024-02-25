using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DriversRoutes.Model;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;


namespace DriversRoutes.Pages.Maps.MapSmall;

public partial class MapSmallVM : ObservableObject
{
    [ObservableProperty]
    MapSmallM mapSmallM;

    public Action<MapSpan> MoveToRegion;
    public Action<Microsoft.Maui.Controls.Maps.Pin> AddPin;
    public Func<Microsoft.Maui.Controls.Maps.Pin, bool> RemovePin;
    public Func<Microsoft.Maui.Devices.Sensors.Location> GetCurrentLocation;
    public Action<Microsoft.Maui.Devices.Sensors.Location> EditCustomerLocation;

    readonly DataBase.Data.AccessDataBase _db;
    Pin _pin;
    public MapSmallVM(DataBase.Data.AccessDataBase db)
    {
        MapSmallM = new();
        _db = db;
        _pin = new Pin()
        {
            Label = "Nowa lokalizacja"
        };

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
    public Microsoft.Maui.Devices.Sensors.Location OnGetCurrentLocation()
    {
        return GetCurrentLocation?.Invoke();
    }
    public void OnEditCustomerLocation(Microsoft.Maui.Devices.Sensors.Location location)
    {
        EditCustomerLocation?.Invoke(location);
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
            var result = await Shell.Current.DisplayAlert("Nowa lokalizacja", "Czy chcesz nadpisać lokalizace", "Tak", "Nie");
            if (!result)
            {
                return;
            }

            var location = OnGetCurrentLocation();

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
                var location = OnGetCurrentLocation();
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
        MapSmallM.ChangeLocation = false;
    }
}
