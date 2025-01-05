using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesServer;

using Shared.Helper;

namespace SzarotkaBlazor
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellVM();
        }
    }



    public partial class AppShellVM : ObservableObject
    {
        private bool islogin;
        public bool IsLogin
        {
            get => islogin;
            set
            {
                if (SetProperty(ref islogin, value))
                {
                    OnPropertyChanged(nameof(IsLogin));
                }
            }
        }
        private User user;

        public AppShellVM()
        {
            UserAfterLogin.OnLogin += UserAfterLogin_OnLogin;
        }

        private void UserAfterLogin_OnLogin(User user, bool islogin)
        {
            this.user = user;
            IsLogin = islogin;
        }
    }
}
