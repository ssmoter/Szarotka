using Shared.Helper;

using SzarotkaNET10.Service;
namespace SzarotkaNET10
{
    public partial class App : Application
    {
        // This definition works even for views injected into App constructor.
        public static IServiceProvider Services => Shared.Service.AppServiceProvider.Current;
        public App()
        {
            var theme = (AppTheme)Preferences.Get("Theme", 0);
            UserAppTheme = theme;

            MudBlazorTheme.SetCurrentTheme(theme);

            RequestedThemeChanged += MudBlazorTheme.Current_RequestedThemeChanged;

            InitializeComponent();
            RoutingCollectionExtensions.AddRoutings();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
