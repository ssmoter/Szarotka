using DriversRoutes.Pages.Maps.Controls;

using Microsoft.AspNetCore.Components.WebView.Maui;

namespace DriversRoutes.Pages.Maps.Navigate;

public partial class NavigateV : ContentPage
{
    public NavigateV(NavigateVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    BlazorWebView _blazorMap;
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _blazorMap = new Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView()
        {
            HostPage = "wwwroot/index.html",
        };
        _blazorMap.RootComponents.Add(new RootComponent() { ComponentType = typeof(BlazorMap), Selector = "#app" });

        this.mGrid.Add(_blazorMap, row: 0);

        if (BindingContext is NavigateVM vm)
        {
            Task.Run(async () =>
            {
                do
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                while (!BlazorMap.AfterOnInitializedAsync);
                BlazorMap.OnRemoveAdvancedMarker();
                BlazorMap.OnSetCustomer(vm.SelectedPoint);
                BlazorMap.OnSetAdvancedMarker();
                BlazorMap.OnRemoveDrirections();
                BlazorMap.OnFitMapToAdvancedMarkers();
            });
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        this.mGrid.Remove(_blazorMap);
    }


    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is NavigateVM vm)
        {
            if (sender is RadioButton radio)
            {
                var value = radio.Value;
                vm.ChangeTypeOfMapCommand.Execute(value);
            }
        }
    }
}