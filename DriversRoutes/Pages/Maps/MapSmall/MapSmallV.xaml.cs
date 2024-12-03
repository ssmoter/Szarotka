using DataBase.Model.EntitiesRoutes;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

using System.Runtime.CompilerServices;

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
                view.MapSmallVM.OnGoToLocation(new Microsoft.Maui.Maps.MapSpan(pin.Location, 0.01, 0.01));
            }
        }
    });
    public CustomerRoutes Customer
    {
        get => (CustomerRoutes)GetValue(CustomerProperty);
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

    public static readonly BindableProperty PositionOfMapProperty
        = BindableProperty.Create(nameof(PositionOfMap)
            , typeof(Shared.CustomControls.Direction), typeof(MapSmallV)
            , defaultBindingMode: BindingMode.TwoWay
            , propertyChanged: (bindable, oldValu, newValue) =>
            {
            });
    public Shared.CustomControls.Direction PositionOfMap
    {
        get => (Shared.CustomControls.Direction)GetValue(PositionOfMapProperty);
        set => SetValue(PositionOfMapProperty, value);
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
        var service = Shared.Service.AppServiceProvider.Current.GetService(typeof(MapSmallVM)) as MapSmallVM;
        InitializeComponent();
        MapSmallVM = service;

        MapSmallVM.MoveToRegion = null;
        MapSmallVM.AddPin = null;
        MapSmallVM.RemovePin = null;
        MapSmallVM.GetCurrentLocation = null;
        MapSmallVM.EditCustomerLocation = null;


        MapSmallVM.MoveToRegion = this.Map.MoveToRegion;
        MapSmallVM.AddPin = this.Map.Pins.Add;
        MapSmallVM.RemovePin = this.Map.Pins.Remove;
        MapSmallVM.GetCurrentLocation = GetVisibleRegionCenter;
        MapSmallVM.EditCustomerLocation = OnEditCustomerLocation;
        MapSmallVM.AddRoute = SetPolyline;
        MapSmallVM.RemoveRoute = ClearPolyline;

        panGesture = new PanGestureRecognizer();
        if (GestureRecognizers.Count == 0)
        {
            panGesture.PanUpdated += OnPanUpdated;
            this.GestureRecognizers.Add(panGesture);
        }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);
        bool isVisible = false;

        if (propertyName == nameof(Customer))
        {
            isVisible = true;
        }
        if (propertyName == nameof(MapSmallVIsVisible))
        {
            isVisible = true;
        }

        if (isVisible)
        {
            MapSmallVM.MoveToRegion = null;
            MapSmallVM.AddPin = null;
            MapSmallVM.RemovePin = null;
            MapSmallVM.GetCurrentLocation = null;
            MapSmallVM.EditCustomerLocation = null;

            MapSmallVM.MoveToRegion = this.Map.MoveToRegion;
            MapSmallVM.AddPin = this.Map.Pins.Add;
            MapSmallVM.RemovePin = this.Map.Pins.Remove;
            MapSmallVM.GetCurrentLocation = GetVisibleRegionCenter;
            MapSmallVM.EditCustomerLocation = OnEditCustomerLocation;
            MapSmallVM.AddRoute = SetPolyline;
            MapSmallVM.RemoveRoute = ClearPolyline;
        }
    }

    public void Dispose()
    {
        MapSmallVM.MoveToRegion = null;
        MapSmallVM.AddPin = null;
        MapSmallVM.RemovePin = null;
        MapSmallVM.GetCurrentLocation = null;
        MapSmallVM.EditCustomerLocation = null;

        panGesture.PanUpdated -= OnPanUpdated;
    }


    protected override Size ArrangeOverride(Rect bounds)
    {
        if (defaultXMax == 0)
        {
            defaultXMax = bounds.Width;
        }
        if (defaultYMax == 0)
        {
            defaultYMax = bounds.Height;
        }

        return base.ArrangeOverride(bounds);
    }


    private void OnEditCustomerLocation(Microsoft.Maui.Devices.Sensors.Location location)
    {
        if (Customer is null)
        {
            return;
        }
        Customer.Latitude = location.Latitude;
        Customer.Longitude = location.Longitude;
        OnPropertyChanged(nameof(Customer.Latitude));
        OnPropertyChanged(nameof(Customer.Longitude));
    }
    private MapSpan GetVisibleRegionCenter()
    {
        return this.Map.VisibleRegion;
    }
    private void SetPolyline(Polyline polyline)
    {
        Map.MapElements.Add(polyline);
    }
    private void ClearPolyline()
    {
        Map.MapElements.Clear();
    }

    private void Button_Clicked_Close(object sender, EventArgs e)
    {
        MapSmallVIsVisible = !MapSmallVIsVisible;
    }

    public void CalculateRoute()
    {
        MapSmallVM.GetRoutesCommand.Execute(Customer);
    }


    #region Resize

    private double initialWidth;
    private double initialHeight;
    private double startX;
    private double startY;

    private double parentXMax;
    private double parentYMax;

    private double defaultXMax = 0;
    private double defaultYMax = 0;

    private readonly PanGestureRecognizer panGesture;

    void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var parent = (this.Parent as View);
        parentXMax = parent.Width;
        parentYMax = parent.Height;

        if (grid.MaximumWidthRequest == double.PositiveInfinity)
        {
            grid.WidthRequest = border.Width;
        }
        if (grid.MaximumHeightRequest == double.PositiveInfinity)
        {
            grid.WidthRequest = border.Height;
        }


        switch (e.StatusType)
        {
            case GestureStatus.Started:
                {
                    initialWidth = Width;
                    initialHeight = Height;
                    startX = e.TotalX;
                    startY = e.TotalY;
                }
                break;
            case GestureStatus.Running:

                {
                    double newWidth = initialWidth + (-e.TotalX - startX);
                    double newHeight = initialHeight + (e.TotalY - startY);

                    // Optional: Set a minimum size for the view
                    var x = Math.Max(newWidth, 50);
                    if (parentXMax > x)
                    {
                        //WidthRequest = x;
                        border.WidthRequest = x;
                    }
                    var y = Math.Max(newHeight, 50);
                    if (parentYMax > y)
                    {
                        //HeightRequest = y;
                        border.HeightRequest = x;
                    }
                }
                break;
            case GestureStatus.Completed:
                {
                    grid.WidthRequest = border.WidthRequest - 20;
                    grid.HeightRequest = border.HeightRequest - 21;

                    hlLeftOptions.Margin = new Thickness(10);

                    if (border.HeightRequest > (parentYMax / 2)
                        || border.WidthRequest > (parentXMax / 2))
                    {
                        MapSmallVM.IsFullscreen = true;
                    }
                    else
                    {
                        MapSmallVM.IsFullscreen = false;
                    }

                }
                break;
        }
    }


    private void ImageButton_Clicked_Fullscreen(object sender, EventArgs e)
    {
        var parent = (this.Parent as View);
        parentXMax = parent.Width;
        parentYMax = parent.Height;
        var bounds = this.Bounds;


        if (MapSmallVM.IsFullscreen)
        {
            border.WidthRequest = defaultXMax;
            border.HeightRequest = defaultYMax;
            MapSmallVM.IsFullscreen = false;
        }
        else
        {
            border.WidthRequest = parentXMax;
            if (PositionOfMap == Shared.CustomControls.Direction.Up)
            {
                border.HeightRequest = parentYMax - bounds.Top;

            }
            if (PositionOfMap == Shared.CustomControls.Direction.Down)
            {
                border.HeightRequest = parentYMax - (parentYMax - bounds.Bottom);
            }

            MapSmallVM.IsFullscreen = true;
        }
        grid.WidthRequest = border.WidthRequest - 20;
        grid.HeightRequest = border.HeightRequest - 21;
        hlLeftOptions.Margin = new Thickness(10);
    }

    #endregion

}