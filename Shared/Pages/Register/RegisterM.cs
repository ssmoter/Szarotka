using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.Register
{
    public partial class RegisterM : ObservableObject
    {
        private string confirmPassword = "";
        public string ConfirmPassword
        {
            get => confirmPassword;
            set
            {
                if (SetProperty(ref confirmPassword, value))
                {
                    OnPropertyChanged(nameof(ConfirmPassword));
                }
            }
        }


        private bool passwordEquels;
        public bool PasswordEquels
        {
            get => passwordEquels;
            set
            {
                if (SetProperty(ref passwordEquels, value))
                {
                    OnPropertyChanged(nameof(PasswordEquels));
                }
            }
        }

        private bool isPassword;
        public bool IsPassword
        {
            get => isPassword;
            set
            {
                if (SetProperty(ref isPassword, value))
                {
                    OnPropertyChanged(nameof(IsPassword));
                }
            }
        }


        private string emailError = "";
        public string EmailError
        {
            get => emailError;
            set
            {
                if (SetProperty(ref emailError, value))
                {
                    OnPropertyChanged(nameof(EmailError));
                }
            }
        }

        private string passwordError = "";
        public string PasswordError
        {
            get => passwordError;
            set
            {
                if (SetProperty(ref passwordError, value))
                {
                    OnPropertyChanged(nameof(PasswordError));
                }
            }
        }


    }
}
