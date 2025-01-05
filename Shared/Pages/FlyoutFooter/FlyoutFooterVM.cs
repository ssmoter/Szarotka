using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesServer;

using Shared.Helper;

namespace Shared.Pages.FlyoutFooter
{
    public partial class FlyoutFooterVM : ObservableObject
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
        public User User
        {
            get => user;
            set
            {
                if (SetProperty(ref user, value))
                {
                    OnPropertyChanged(nameof(User));
                }
            }
        }



        public FlyoutFooterVM()
        {
            UserAfterLogin.OnLogin += UserAfterLogin_OnLogin;
        }

        private void UserAfterLogin_OnLogin(User user, bool arg2)
        {
            User = user;
            IsLogin = arg2;
        }

        [RelayCommand]
        async Task LogOut()
        {
            var toast = Toast.Make("Wylogowywanie...");
            var log = Shared.Service.AppServiceProvider.GetService<LogIn.LogInV>();
            var navigation = Shell.Current.Navigation.PushAsync(log);

            var pages = Shell.Current.Navigation.NavigationStack.Where(x => x is not null).ToArray();

            foreach (Page item in pages)
            {
                Shell.Current.Navigation.RemovePage(item);
            }
            UserAfterLogin.RemoveLoginUser();

            await Task.WhenAll(toast.Show(), navigation);
        }

    }
}
