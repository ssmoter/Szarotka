namespace Shared.Pages.FlyoutHeader;

public partial class FlyoutHeaderV : ContentView
{
    public FlyoutHeaderV()
    {
        InitializeComponent();
        BindingContext = new FlyoutHeaderVM();
    }
}