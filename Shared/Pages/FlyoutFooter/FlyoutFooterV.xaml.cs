using Shared.Helper;

namespace Shared.Pages.FlyoutFooter;

public partial class FlyoutFooterV : ContentView
{
    public FlyoutFooterV()
    {
        InitializeComponent();
        BindingContext = new FlyoutFooterVM();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label item) { return; }
        await item.BounceOnPressAsync();
    }
}