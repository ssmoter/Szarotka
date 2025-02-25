using Shared.Helper;

namespace Shared.Pages.FlyoutHeader;

public partial class FlyoutHeaderV : ContentView
{
    public FlyoutHeaderV()
    {
        InitializeComponent();
        BindingContext = new FlyoutHeaderVM();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is not Label item) { return; }
        await item.BounceOnPressAsync();
    }

}