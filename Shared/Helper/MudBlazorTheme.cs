using MudBlazor;

namespace Shared.Helper
{
    public static class MudBlazorTheme
    {
        public static bool IsDarkMode = false;
        public static MudTheme Theme = new();
        public static Action ActionThemeChanged;

        public static void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            var theme = e.RequestedTheme;

            SetCurrentTheme(theme);
            ActionThemeChanged?.Invoke();
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
