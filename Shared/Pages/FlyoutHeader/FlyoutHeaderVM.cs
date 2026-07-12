using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Model.EntitiesServer;

using Shared.Helper;

namespace Shared.Pages.FlyoutHeader
{
    public partial class FlyoutHeaderVM : ObservableObject, IDisposable
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

        private IView customContent;
        public IView CustomContent
        {
            get => customContent;
            set
            {
                if (SetProperty(ref customContent, value, nameof(CustomContent))) { }
            }
        }
        public static List<ToolbarItem> ToolbarItems { get; set; } = [];
        public static Action<ToolbarItem> ActionToolbarItemSet;
        public static Action<ToolbarItem> ActionToolbarItemRemove;
        private void _OnSetToolbarItem(ToolbarItem item)
        {
            ToolbarItems ??= [];
            ToolbarItems.Add(item);
        }
        public static void SetToolbarItem(ToolbarItem item)
        {
            ActionToolbarItemSet?.Invoke(item);
        }
        private void _RemoveSetToolbarItem(ToolbarItem item)
        {
            ToolbarItems.Remove(item);
        }
        public static void RemoveToolbarItem(ToolbarItem item)
        {
            ActionToolbarItemRemove?.Invoke(item);
        }
        private void _onCustomContent(IView view)
        {
            if (view is null)
            {
                FadeOutElementThrowAndForget(CustomContent);
            }
            if (view is not null)
            {
                Shell.Current.FlyoutIsPresented = true;
                CustomContent = view;
            }
        }
        private static Func<bool> funcCustomContentExist;
        private static Action<IView> actionCustomContent;
        public static void OnCustomContent(IView view = null)
        {
            actionCustomContent?.Invoke(view);
        }
        /// <summary>
        /// Sprawdza czy została dodana jakaś zawartość.
        /// Zwraca false jeżeli content jest null.
        /// </summary>
        /// <returns>Zwraca false jeżeli content jest null</returns>
        public static bool IsCustomContentExist()
        {
            return (bool)funcCustomContentExist?.Invoke();
        }
        private async void FadeOutElementThrowAndForget(IView view)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                await FadeOutElement(view);
                CustomContent = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task FadeOutElement(IView view)
        {
            if (view is VisualElement element)
            {
                await Task.WhenAll(
                    element.FadeToAsync(0, 500),
                    element.TranslateToAsync(0, -20, 500)
                );
            }
        }
        private bool isCustomContentExist()
        {
            if (CustomContent is null)
            {
                return false;
            }
            return true;
        }
        public FlyoutHeaderVM()
        {
            UserAfterLogin.OnLogin += UserAfterLogin_OnLogin;

            actionCustomContent += _onCustomContent;
            funcCustomContentExist += isCustomContentExist;

            ActionToolbarItemSet += _OnSetToolbarItem;
            ActionToolbarItemRemove += _RemoveSetToolbarItem;
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

        public void Dispose()
        {
            UserAfterLogin.OnLogin -= UserAfterLogin_OnLogin;
            actionCustomContent -= _onCustomContent;
            ActionToolbarItemSet -= _OnSetToolbarItem;
            ActionToolbarItemRemove -= _RemoveSetToolbarItem;
            funcCustomContentExist -= isCustomContentExist;

        }
    }
}

