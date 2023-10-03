namespace DriversRoutes.Pages.Maps;

public partial class MapsV : ContentPage
{
    public MapsV(MapsVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}