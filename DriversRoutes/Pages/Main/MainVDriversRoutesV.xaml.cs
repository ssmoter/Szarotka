namespace DriversRoutes.Pages.Main;

public partial class MainVDriversRoutesV : ContentPage
{
    public MainVDriversRoutesV(MainVDriversRoutesVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}