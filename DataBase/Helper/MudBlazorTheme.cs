using MudBlazor;

namespace DataBase.Helper
{
    public class MudBlazorTheme
    {
        public static bool IsDarkMode = false;
        public static MudTheme Theme = new();

        public static void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            var theme = e.RequestedTheme;

            SetCurrentTheme(theme);

        }

        public static void SetCurrentTheme(AppTheme theme)
        {
            if (theme == AppTheme.Light)
            {
                IsDarkMode = false;
            }
            else
            {
                IsDarkMode = true;
            }
        }
    }
}
