using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Shared.Pages.Log.LogData
{

    public partial class LogDataVM : ObservableObject, IQueryAttributable
    {
        private LogM singleLog;
        public LogM SingleLog
        {
            get => singleLog;
            set
            {
                if (SetProperty(ref singleLog, value))
                {
                    OnPropertyChanged(nameof(SingleLog));
                }
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(LogM), out object singleLog))
            {
                if (singleLog is LogM _singleLog)
                {
                    SingleLog = _singleLog;
                }
            }

        }
        public LogDataVM()
        {
            SingleLog = new LogM();
        }



        [RelayCommand]
        async Task Copy()
        {
            var txt = $"Data-{SingleLog.Created} {Environment.NewLine}Wiadomość-{SingleLog.Message} {Environment.NewLine}Miejsce wystąpienia-{SingleLog.StackTrace}";
            await Clipboard.SetTextAsync(txt);

            var toast = Toast.Make("Skopiowano informacje o błędzie", CommunityToolkit.Maui.Core.ToastDuration.Short);
            await toast.Show();
        }
    }
}
