﻿using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace DriversRoutes.Pages.Maps;

public partial class MapsV : ContentPage
{
    public MapsV(MapsVM vm)
    {
        InitializeComponent();
        vm.GoToLocation += Map.MoveToRegion;
        BindingContext = vm;

        Task.Run(async() =>
        {
            await vm.StartListeningLocation(this.Map);
        });

        //MapSpan mapSpan = vm.szarotka;
        //Task.Run(async () =>
        //{
        //    mapSpan = await vm.GetCurrentLocation();
        //});
        //if (mapSpan is not null)
        //    this.Map.MoveToRegion(mapSpan);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is MapsVM vm)
        {
            if (vm.Routes is null)
                return;
            if (vm.DriversRoutesName.Length <= 16)
                vm.DriversRoutesName += vm.Routes.Name;
            if (vm.LastSeectedDayOfWeek is not null)
            {
                vm.AllPoints = await vm.GetSelectedDays(vm.LastSeectedDayOfWeek);
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

            var customer = new Model.CustomerRoutes()
            {
                CreatedDate = DateTime.Now,
                Longitude = e.Location.Longitude,
                Latitude = e.Location.Latitude,
                RoutesId = vm.Routes.Id,
            };

            await Shell.Current.GoToAsync($"{nameof(Pages.AddCustomer.AddCustomerV)}"
                , new Dictionary<string, object>
                {
                    [nameof(Model.CustomerRoutes)] = customer,
                    [nameof(Model.Routes)] = vm.Routes,
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
}