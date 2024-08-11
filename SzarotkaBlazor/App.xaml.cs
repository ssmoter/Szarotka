using DataBase.Helper;

namespace SzarotkaBlazor
{
    public partial class App : Application
    {
        public App()
        {
            var theme = (AppTheme)Preferences.Get("Theme", 0);
            UserAppTheme = theme;

            MudBlazorTheme.SetCurrentTheme(theme);

            RequestedThemeChanged += MudBlazorTheme.Current_RequestedThemeChanged;

            InitializeComponent();

            MainPage = new AppShell();
        }


    }
}
