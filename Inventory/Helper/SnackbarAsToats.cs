using CommunityToolkit.Maui.Alerts;

namespace Inventory.Helper
{
    public static class SnackbarAsToats
    {
        public static async Task OnShow(string message)
        {
            CancellationTokenSource cancellationTokenSource = new();
            string text = message;
            TimeSpan _duration = TimeSpan.FromSeconds(2);
            var snackbar = Snackbar.Make(text, cancellationTokenSource.Cancel, duration: _duration);

            await snackbar.Show(cancellationTokenSource.Token);
        }
    }
}
