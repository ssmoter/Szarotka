namespace DriversRoutes.Pages.Main;

public partial class MainVDriversRoutesV : ContentPage
{
    public MainVDriversRoutesV(MainVDriversRoutesVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is MainVDriversRoutesVM vm)
        {
            vm.Routes = await vm.GetRoutes();
        }

    }
}
