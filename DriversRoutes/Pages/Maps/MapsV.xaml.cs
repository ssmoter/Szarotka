using CommunityToolkit.Maui.Views;

using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace DriversRoutes.Pages.Maps;

public partial class MapsV : ContentPage
{
    public MapsV(MapsVM vm)
    {
        InitializeComponent();
        vm.GoToLocation += Map.MoveToRegion;
        BindingContext = vm;

        MapSpan mapSpan = vm.szarotka;
        Task.Run(async () =>
        {
            mapSpan = await vm.GetCurrentLocation();
        });
        if (mapSpan is not null)
            this.Map.MoveToRegion(mapSpan);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private async void Map_MapClicked(object sender, MapClickedEventArgs e)
    {
        if (BindingContext is MapsVM vm)
        {
            if (!vm.AddLocationIs)
            {
                return;
            }
            if (vm.Routes is null)
            {
                await Shell.Current.DisplayAlert("Brak trasy", "Zapisywanie jest dostępne tylko po wybraniu trasy konkretnego kierowcy", "Ok");
                return;
            }
            var popup = new Popups.AddCustomer.AddCustomerV(new Model.CustomerRoutes()
            {
                Latitude = e.Location.Latitude,
                Longitude = e.Location.Longitude,
                DayOfWeek = new Model.SelectedDayOfWeekRoutes()
            });

            var update = await this.ShowPopupAsync(popup);
            if (update is null)
            {
                return;
            }
            if (update is Model.CustomerRoutes customerUpdate)
            {
                await vm.AddNewPoint(customerUpdate);
            }
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