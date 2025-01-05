using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.ConfirmEmail
{
    public partial class ConfirmEmailM : ObservableObject
    {
        private int? code;
        public int? Code
        {
            get => code;
            set
            {
                if (SetProperty(ref code, value))
                {
                    OnPropertyChanged(nameof(Code));
                }
            }
        }
        private string error;
        public string Error
        {
            get => error;
            set
            {
                if (SetProperty(ref error, value))
                {
                    OnPropertyChanged(nameof(Error));
                }
            }
        }

    }
}
