using CommunityToolkit.Mvvm.ComponentModel;

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
    }
}
