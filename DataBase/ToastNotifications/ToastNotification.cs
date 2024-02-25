using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace DataBase.ToastNotifications
{
    public class ToastNotification
    {
        public static async Task MakeToast(string message, ToastDuration toastDuration = ToastDuration.Short, CancellationToken token = default)
        {
            double fontSize = 14;

            var toast = Toast.Make(message, toastDuration, fontSize);
            await toast.Show(token);
        }
    }
}
