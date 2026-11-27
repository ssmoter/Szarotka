using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Model.EntitiesServer;

using Shared.Helper;

namespace SzarotkaNET10
{
    public partial class AppShell : Shell, IDisposable
    {
        public AppShell()
        {
            InitializeComponent();
            var vm = new AppShellVM();
            BindingContext = vm;

            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.ActionToolbarItemSet += SetToolbarItem;
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.ActionToolbarItemRemove += RemoveToolbarItem;
        }


        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            if (sender is not Label item) return;
            await item.BounceOnPressAsync();
            var user = UserAfterLogin.User;
            await Shell.Current.GoToAsync($"{nameof(Shared.Pages.UserDisplay.UserDisplayV)}?",
                new Dictionary<string, object>()
                {
                    [nameof(Shared.Pages.UserDisplay.UserDisplayVM.User)] = user
                });
        }

        private void SetToolbarItem(ToolbarItem? toolbarItem)
        {
            // 1. Zabezpieczenie przed wartością null
            if (toolbarItem == null)
            {
                return; // lub throw new ArgumentNullException(nameof(toolbarItem));
            }

            // 2. Zabezpieczenie przed wywołaniem z wątku w tle (wymagane dla elementów UI)
            if (!MainThread.IsMainThread)
            {
                MainThread.BeginInvokeOnMainThread(() => SetToolbarItem(toolbarItem));
                return;
            }

            // 3. Zabezpieczenie przed duplikatami (sprawdzenie po referencji oraz po nazwie/tekście)
            bool alreadyExists = ToolbarItems.Any(x =>
                ReferenceEquals(x, toolbarItem) ||
                (!string.IsNullOrEmpty(x.Text) && x.Text == toolbarItem.Text));

            if (!alreadyExists)
            {
                ToolbarItems.Add(toolbarItem);
            }

            // 4. Ustawienie kolejności elementów (zabezpieczenie przed potencjalnym nullem w kolekcji)
            var targetOrder = ToolbarItems.Count > 2
                ? ToolbarItemOrder.Secondary
                : ToolbarItemOrder.Primary;

            foreach (var item in ToolbarItems)
            {
                if (item != null && item.Order != targetOrder)
                {
                    item.Order = targetOrder;
                }
            }
        }

        private void RemoveToolbarItem(ToolbarItem toolbarItem)
        {
            ToolbarItems.Remove(toolbarItem);
        }
        public void Dispose()
        {
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.ActionToolbarItemSet -= SetToolbarItem;
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.ActionToolbarItemRemove -= RemoveToolbarItem;
        }
    }



    public partial class AppShellVM : ObservableObject, IDisposable
    {
        private bool isLogin;
        public bool IsLogin
        {
            get => isLogin;
            set
            {
                if (SetProperty(ref isLogin, value, nameof(IsLogin)))
                {
                    OnPropertyChanged(nameof(IsLogin));
                }
            }
        }
        private bool isInitialized;
        public bool IsInitialized
        {
            get => isInitialized;
            set
            {
                if (SetProperty(ref isInitialized, value, nameof(IsInitialized)))
                {
                }
            }
        }
        private User user = new();
        public User User
        {
            get => user;
            set
            {
                if (SetProperty(ref user, value, nameof(User)))
                {
                }
            }
        }
        private string userName = "";
        public string UserName
        {
            get => userName;
            set
            {
                if (SetProperty(ref userName, value, nameof(UserName)))
                {
                }
            }
        }


        public AppShellVM()
        {
            UserAfterLogin.OnLogin += UserAfterLogin_OnLogin;
            IsInitialized = true;
        }

        private void UserAfterLogin_OnLogin(User user, bool isLogin)
        {
            User = user;
            IsLogin = isLogin;
            UserName = $"Konto {user.Name}";
            IsInitialized = false;
        }

        public void Dispose()
        {
            UserAfterLogin.OnLogin -= UserAfterLogin_OnLogin;
        }
    }
}
