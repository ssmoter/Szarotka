using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.Log
{
    public partial class LogM : ObservableObject
    {

        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (SetProperty(ref id, value))
                {
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        private string stackTrace;
        public string StackTrace
        {
            get => stackTrace;
            set
            {
                if (SetProperty(ref stackTrace, value))
                {
                    OnPropertyChanged(nameof(StackTrace));
                }
            }
        }
        private string message;
        public string Message
        {
            get => message;
            set
            {
                if (SetProperty(ref message, value))
                {
                    OnPropertyChanged(nameof(Message));
                }
            }
        }
        private DateTime created;
        public DateTime Created
        {
            get => created;
            set
            {
                if (SetProperty(ref created, value))
                {
                    OnPropertyChanged(nameof(Created));
                }
            }
        }
    }
}
