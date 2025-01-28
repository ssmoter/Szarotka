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
        private bool isLogin;
        public bool IsLogin
        {
            get => isLogin;
            set
            {
                if (SetProperty(ref isLogin, value))
                {
                    OnPropertyChanged(nameof(IsLogin));
                }
            }
        }
        private User user = new();

        public AppShellVM()
        {
            UserAfterLogin.OnLogin += UserAfterLogin_OnLogin;
        }

        private void UserAfterLogin_OnLogin(User user, bool isLogin)
        {
            this.user = user;
            IsLogin = isLogin;
        }
    }
}
