using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Shared.Helper;
using Shared.Model;

namespace Shared.Pages.FlyoutFooter
{
    public partial class FlyoutFooterVM : ObservableObject, IDisposable
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
                try
                {
                    Shell.Current.Navigation.RemovePage(item);
                }
                catch (Exception)
                {
                }
            }
            UserAfterLogin.RemoveLoginUser();

            await Task.WhenAll(toast.Show(), navigation);
        }

        public void Dispose()
        {
            UserAfterLogin.OnLogin -= UserAfterLogin_OnLogin;
        }
    }
}
