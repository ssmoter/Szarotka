using CommunityToolkit.Maui.Views;

using Microsoft.Maui.Controls.Maps;

namespace DriversRoutes.Pages.Maps;

public partial class MapsV : ContentPage
{
    public MapsV(MapsVM vm)
    {
        InitializeComponent();
        vm.GoToLocation += Map.MoveToRegion;
        BindingContext = vm;
        Task.Run(async () =>
        {
            var mapSpan = await vm.GetCurrentLocation();
            this.Map.MoveToRegion(mapSpan);
        });
    }




    private async void Map_MapClicked(object sender, MapClickedEventArgs e)
    {
        if (BindingContext is MapsVM vm)
        {
            if (!vm.AddLocationIs)
            {
                return;
            }

            var popup = new Popups.AddCustomer.AddCustomerV(new Model.Customer()
            {
                Latitude = e.Location.Latitude,
                Longitude = e.Location.Longitude
            });

            var a = await this.ShowPopupAsync(popup);
        }
    }

    private void Pin_InfoWindowClicked(object sender, PinClickedEventArgs e)
    {
        if (BindingContext is MapsVM vm)
        {
            if (sender is Pin pin)
            {
                vm.OpenMoreDetail(pin);
            }
        }

    }
}