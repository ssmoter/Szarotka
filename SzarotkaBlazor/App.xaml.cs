using Shared.Helper;

using SzarotkaBlazor.Service;
namespace SzarotkaBlazor
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
            MainPage = new AppShell();
        }


    }
}
