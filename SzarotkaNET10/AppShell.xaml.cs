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

        private void SetToolbarItem(ToolbarItem toolbarItem)
        {
            if (!ToolbarItems.Contains(toolbarItem))
                ToolbarItems.Add(toolbarItem);

            if (ToolbarItems.Count > 2)
            {
                foreach (var item in ToolbarItems)
                {
                    item.Order = ToolbarItemOrder.Secondary;
                }
            }
            else
            {
                foreach (var item in ToolbarItems)
                {
                    item.Order = ToolbarItemOrder.Primary;
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
