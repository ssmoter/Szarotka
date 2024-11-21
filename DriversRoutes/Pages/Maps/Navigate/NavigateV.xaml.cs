using DriversRoutes.Pages.Maps.Controls;

namespace DriversRoutes.Pages.Maps.Navigate;

public partial class NavigateV : ContentPage
{
    public NavigateV(NavigateVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

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
    }
}