using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.UserDisplay.UserEdit
{
    public partial class UserEditM : ObservableObject
    {

        private string emailError = "";
        public string EmailError
        {
            get => emailError;
            set
            {
                if (SetProperty(ref emailError, value, nameof(EmailError))) { }
            }
        }
        private string nameError = "";
        public string NameError
        {
            get => nameError;
            set
            {
                if (SetProperty(ref nameError, value, nameof(NameError))) { }
            }
        }
        private string phoneError = "";
        public string PhoneError
        {
            get => phoneError;
            set
            {
                if (SetProperty(ref phoneError, value, nameof(PhoneError))) { }
            }
        }
        private string error = "";
        public string Error
        {
            get => error;
            set
            {
                if (SetProperty(ref error, value, nameof(Error))) { }
            }
        }
    }
}
