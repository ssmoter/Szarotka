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
    RootComponent rootComponent;
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _blazorMap = new Microsoft.AspNetCore.Components.WebView.Maui.BlazorWebView()
        {
            HostPage = "wwwroot/index.html",
        };
        rootComponent ??= new RootComponent() { ComponentType = typeof(BlazorMap), Selector = "#app" };


        _blazorMap.RootComponents.Add(rootComponent);

        //_blazorMap.RootComponents.Add(new RootComponent() { ComponentType = typeof(BlazorMap), Selector = "#app" });


        this.mGrid.Add(_blazorMap, row: 0);

        if (BindingContext is NavigateVM vm)
        {
            BlazorMap.AfterOnInitializedAsync = () =>
            {
                BlazorMap.OnRemoveAdvancedMarker();
                BlazorMap.OnSetCustomer(vm.SelectedPoint);
                BlazorMap.OnSetAdvancedMarker();
                BlazorMap.OnRemoveDrirections();
                BlazorMap.OnFitMapToAdvancedMarkers();
            };
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        this.mGrid.Remove(_blazorMap);
    }
}