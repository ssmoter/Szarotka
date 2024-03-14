using DataBase.Model.EntitiesRoutes;

using Microsoft.Maui.Controls.Maps;

namespace DriversRoutes.Pages.Maps.MapSmall;

public partial class MapSmallV : ContentView, IDisposable
{
    #region Variables


    public static readonly BindableProperty CustomerProperty
    = BindableProperty.Create(nameof(Customer)
        , typeof(CustomerRoutes)
        , typeof(MapSmallV)
        , defaultBindingMode: BindingMode.TwoWay
        , propertyChanged: (bindable, oldValu, newValue) =>
    {

        if (bindable is MapSmallV view)
        {
            if (newValue is CustomerRoutes customer)
            {
                var pin = new Pin
                {
                    Location = new Microsoft.Maui.Devices.Sensors.Location(customer.Latitude, customer.Longitude),
                    Label = $"{customer.QueueNumber}: {customer.Name}",
                    Address = $"{customer.Description}",
                    Type = PinType.SavedPin
                };
                view.Map.Pins.Clear();
                view.Map.Pins.Add(pin);
                view.MapSmallVM.MapSmallM.OldLatitude = customer.Latitude;
                view.MapSmallVM.MapSmallM.OldLongitude = customer.Longitude;
                view.mapSmallVM.OnGoToLocation(new Microsoft.Maui.Maps.MapSpan(pin.Location, 0.01, 0.01));
            }
        }
    });
    public CustomerRoutes Customer
    {
        get => GetValue(CustomerProperty) as CustomerRoutes;
        set => SetValue(CustomerProperty, value);
    }


    public static readonly BindableProperty EditIsVisibleProperty
    = BindableProperty.Create(nameof(EditIsVisible)
        , typeof(bool)
        , typeof(MapSmallV)
        , propertyChanged: (bindable, oldValu, newValue) =>
    {
    });
    public bool EditIsVisible
    {
        get => (bool)GetValue(EditIsVisibleProperty);
        set => SetValue(EditIsVisibleProperty, value);
    }

    public static readonly BindableProperty MapSmallVIsVisibleProperty
    = BindableProperty.Create(nameof(MapSmallVIsVisible)
        , typeof(bool), typeof(MapSmallV)
        , defaultBindingMode: BindingMode.TwoWay
        , propertyChanged: (bindable, oldValu, newValue) =>
        {
            if (bindable is MapSmallV view)
            {
                if (view.Customer is not null)
                {
                    view.MapSmallVM.OnGoToLocation(new Microsoft.Maui.Maps.MapSpan(
                        new Microsoft.Maui.Devices.Sensors.Location(
                            view.Customer.Latitude
                            , view.Customer.Longitude), 0.01, 0.01));
                }
            }
        });
    public bool MapSmallVIsVisible
    {
        get => (bool)GetValue(MapSmallVIsVisibleProperty);
        set => SetValue(MapSmallVIsVisibleProperty, value);
    }

    #endregion

    private MapSmallVM mapSmallVM;
    public MapSmallVM MapSmallVM
    {
        get => mapSmallVM;
        set
        {
            mapSmallVM = value;
            OnPropertyChanged(nameof(MapSmallVM));
        }
    }


    public MapSmallV()
    {
        InitializeComponent();
        MapSmallVM = new(new DataBase.Data.AccessDataBase());
        MapSmallVM.MoveToRegion += this.Map.MoveToRegion;
        MapSmallVM.AddPin += this.Map.Pins.Add;
        MapSmallVM.RemovePin += this.Map.Pins.Remove;
        MapSmallVM.GetCurrentLocation += GetVisibleRegionCenter;
        mapSmallVM.EditCustomerLocation += OnEditCustomerLocation;
    }


    private void OnEditCustomerLocation(Microsoft.Maui.Devices.Sensors.Location location)
    {
        Customer.Latitude = location.Latitude;
        Customer.Longitude = location.Longitude;
        OnPropertyChanged(nameof(Customer.Latitude));
        OnPropertyChanged(nameof(Customer.Longitude));
    }
    private Microsoft.Maui.Devices.Sensors.Location GetVisibleRegionCenter()
    {
        return this.Map.VisibleRegion.Center;
    }

    public void Dispose()
    {
        MapSmallVM.MoveToRegion -= this.Map.MoveToRegion;
        MapSmallVM.AddPin -= this.Map.Pins.Add;
        MapSmallVM.RemovePin -= this.Map.Pins.Remove;
        MapSmallVM.GetCurrentLocation -= GetVisibleRegionCenter;
        mapSmallVM.EditCustomerLocation -= OnEditCustomerLocation;
    }

    private void Button_Clicked_Close(object sender, EventArgs e)
    {
        MapSmallVIsVisible = !MapSmallVIsVisible;
    }

}