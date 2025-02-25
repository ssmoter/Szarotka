using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesServer;

using Shared.Helper;

namespace Shared.Pages.FlyoutHeader
{
    public partial class FlyoutHeaderVM : ObservableObject
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



        public FlyoutHeaderVM()
        {
            UserAfterLogin.OnLogin += UserAfterLogin_OnLogin;
        }

        private void UserAfterLogin_OnLogin(User user, bool arg2)
        {
            User = user;
            IsLogin = arg2;
        }

        [RelayCommand]
        async Task GoToLogin()
        {
            await Shell.Current.GoToAsync(nameof(LogIn.LogInV));
        }

        [RelayCommand]
        async Task GoToUser()
        {
            var user = User;
            await Shell.Current.GoToAsync($"{nameof(UserDisplay.UserDisplayV)}?",
                new Dictionary<string, object>()
                {
                    [nameof(UserDisplay.UserDisplayVM.User)] = user
                });
        }
    }
}
