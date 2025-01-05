namespace Shared.Pages.FlyoutFooter;

public partial class FlyoutFooterV : ContentView
{
    public FlyoutFooterV()
    {
        InitializeComponent();
        BindingContext = new FlyoutFooterVM();
    }
}