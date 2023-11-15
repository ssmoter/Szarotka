namespace Szarotka
{
    public partial class App : Application
    {
        public App()
        {
            var theme = Preferences.Get("Theme", 0);
            UserAppTheme = (AppTheme)theme;

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
