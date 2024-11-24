using CommunityToolkit.Maui.Alerts;

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

#if ANDROID

        var android = DeviceInfo.Current.VersionString;

        if (double.TryParse(android, out double result))
        {
            if (result < 9)
            {
                var snack = new Snackbar()
                {
                    Text = "Z daną wersję Androida (7,8) mapy google nie są kompatybilne",
                    ActionButtonText = "OK",
                    Duration = TimeSpan.FromSeconds(60),
                };
                await snack.Show();
            }
        }
#endif

    }
}
