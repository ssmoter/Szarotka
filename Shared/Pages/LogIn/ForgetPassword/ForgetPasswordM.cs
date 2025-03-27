using CommunityToolkit.Mvvm.ComponentModel;

namespace Shared.Pages.LogIn.ForgetPassword
{
    public partial class ForgetPasswordM : ObservableObject
    {
        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value, nameof(Email));
        }
        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value, nameof(Password));
        }
        private string passwordReaped;
        public string PasswordReaped
        {
            get => passwordReaped;
            set => SetProperty(ref passwordReaped, value, nameof(PasswordReaped));
        }

        private bool isPassword = true;
        public bool IsPassword
        {
            get => isPassword;
            set => SetProperty(ref isPassword, value, nameof(IsPassword));
        }

        private bool isSendCodeVisible;
        public bool IsSendCodeVisible
        {
            get => isSendCodeVisible;
            set => SetProperty(ref isSendCodeVisible, value, nameof(IsSendCodeVisible));
        }
        private bool isSendCodeEnable = true;
        public bool IsSendCodeEnable
        {
            get => isSendCodeEnable;
            set => SetProperty(ref isSendCodeEnable, value, nameof(IsSendCodeEnable));
        }
        private bool isEmailEnable = true;
        public bool IsEmailEnable
        {
            get => isEmailEnable;
            set => SetProperty(ref isEmailEnable, value, nameof(IsEmailEnable));
        }


        private bool isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => isPasswordVisible;
            set => SetProperty(ref isPasswordVisible, value, nameof(IsPasswordVisible));
        }

        private int code;
        public int Code
        {
            get => code;
            set => SetProperty(ref code, value, nameof(Code));
        }

        private bool isPasswordEquals;
        public bool IsPasswordEquals
        {
            get => isPasswordEquals;
            set => SetProperty(ref isPasswordEquals, value, nameof(IsPasswordEquals));
        }


        private string emailValid = "";
        public string EmailValid
        {
            get => emailValid;
            set => SetProperty(ref emailValid, value, nameof(EmailValid));
        }
        private string codeValid = "";
        public string CodeValid
        {
            get => codeValid;
            set => SetProperty(ref codeValid, value, nameof(CodeValid));
        }
        private string passwordValid = "";
        public string PasswordValid
        {
            get => passwordValid;
            set => SetProperty(ref passwordValid, value, nameof(PasswordValid));
        }
    }
}
