using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DataBase.Pages.Log.LogData
{

    [QueryProperty(nameof(SingleLog), nameof(LogM))]
    public partial class LogDataVM : ObservableObject
    {
        [ObservableProperty]
        LogM singleLog;

        public LogDataVM()
        {
            SingleLog = new LogM();
        }



        [RelayCommand]
        async Task Copy()
        {
            var txt = $"Data-{SingleLog.Created} {Environment.NewLine}Wiadomość-{SingleLog.Message} {Environment.NewLine}Miejsce wystąpienia-{SingleLog.StackTrace}";
            await Clipboard.SetTextAsync(txt);

            var toast = Toast.Make("Skopiowano współrzędne geograficzne", CommunityToolkit.Maui.Core.ToastDuration.Short);
            await toast.Show();
        }
    }
}
