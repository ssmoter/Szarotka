using DataBase.Model.EntitiesRoutes;

using Microsoft.Maui.Controls.Maps;

namespace DriversRoutes.Pages.Maps.MapAndPoints;

public partial class MapsV : ContentPage, IDisposable
{
    public MapsV(MapsVM vm)
    {
        InitializeComponent();
        vm.GoToLocationAction += Map.MoveToRegion;
        vm.AddRoutesPolilineAction += SetPolyline;
        vm.ClearRoutesPolilineAction += ClearPolyline;
        vm.GetMap = Map;
        BindingContext = vm;
    }
    public void Dispose()
    {
        if (BindingContext is MapsVM vm)
        {
            vm.GoToLocationAction -= Map.MoveToRegion;
            vm.AddRoutesPolilineAction -= SetPolyline;
            vm.ClearRoutesPolilineAction -= ClearPolyline;
        }
        GC.SuppressFinalize(this);
    }

    private void SetPolyline(Polyline polyline)
    {
        Map.MapElements.Add(polyline);
    }
    private void ClearPolyline()
    {
        Map.MapElements.Clear();
    }




    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        if (BindingContext is MapsVM vm)
        {
            vm.StopListeningLocation();
            if (vm.LastSelectedDayOfWeek is not null)
            {
                vm.LastSelectedDayOfWeekWhenNavigation = vm.LastSelectedDayOfWeek;
            }
        }
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is MapsVM vm)
        {
            vm.StartListeningLocation();

            if (vm.Routes is null)
                return;

            vm.DriversRoutesName = vm.Routes.Name;

            if (vm.LastSelectedDayOfWeekWhenNavigation is not null)
            {
                vm.GetSelectedDaysAndForget(vm.LastSelectedDayOfWeekWhenNavigation);
            }
            else if (vm.LastSelectedDayOfWeek is not null)
            {
                vm.GetSelectedDaysAndForget(vm.LastSelectedDayOfWeek);
            }
        }
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

            var customer = new CustomerRoutes()
            {
                Created = DateTime.Now,
                Longitude = e.Location.Longitude,
                Latitude = e.Location.Latitude,
                RoutesId = vm.Routes.Id,
            };

            await Shell.Current.GoToAsync($"{nameof(Pages.Customer.AddCustomer.AddCustomerV)}"
                , new Dictionary<string, object>
                {
                    [nameof(CustomerRoutes)] = customer,
                    [nameof(Routes)] = vm.Routes,
                });
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

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is MapsVM vm)
        {
            if (sender is RadioButton radio)
            {
                var value = radio.Value;
                vm.ChangeTypeOfMapCommand.Execute(value);
            }
        }
    }

    private void CustomPin_InfoWindowClicked(object sender, PinClickedEventArgs e)
    {
        if (BindingContext is MapsVM vm)
        {
            if (sender is Pin pin)
            {
                vm.OpenMoreDetail(pin);
                vm.StepSelected = DataBase.CustomControls.StepSelected.Full;
            }
        }
    }

}


