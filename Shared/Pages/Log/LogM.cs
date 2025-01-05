using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.Log
{
    public partial class LogM : ObservableObject
    {

        [ObservableProperty]
        int id;
        [ObservableProperty]
        string stackTrace;
        [ObservableProperty]
        string message;
        [ObservableProperty]
        DateTime created;
    }
}
