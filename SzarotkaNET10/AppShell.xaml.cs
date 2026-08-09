using CommunityToolkit.Mvvm.ComponentModel;

using DataBase.Data;
using DataBase.Model;
using DataBase.Model.EntitiesServer;

using Shared.Data;
using Shared.Helper;
using Shared.Model;
using Shared.Service;

namespace SzarotkaNET10
{
    public partial class AppShell : Shell, IDisposable
    {
        public AppShell()
        {
            InitializeComponent();
            var vm = new AppShellVM();
            BindingContext = vm;
            _createdDataBase = Shared.Service.AppServiceProvider.GetService<ICreatedDataBase>();
            _db = Shared.Service.AppServiceProvider.GetService<IAccessDataBaseAoT>();


            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.ActionToolbarItemSet += SetToolbarItem;
            Shared.Pages.FlyoutHeader.FlyoutHeaderVM.ActionToolbarItemRemove += RemoveToolbarItem;
        }

        private readonly ICreatedDataBase _createdDataBase;
        private readonly IAccessDataBaseAoT _db;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var update = UpdateDataBase();
            await update;
            if (update.Result)
            {
                await GotToLogin();
            }
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            var update = UpdateDataBase();
            await update;
            if (update.Result)
            {
                await GotToLogin();
            }
        }



        private async Task GotToLogin()
        {
            try
            {
                var user = await nameof(UserAfterLogin.User.Token).GetHelperTableAsync(_db);
                if (user is not null)
                {
                    try
                    {
                        UserAfterLogin.SetLoginUser(user.Value);
                    }
                    catch (Exception)
                    { }
                }

                if (!UserAfterLogin.IsLogin)
                {
                    await Shell.Current.GoToAsync($"{nameof(Shared.Pages.LogIn.LogInV)}");
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }
        private async Task<bool> UpdateDataBase()
        {
            var result = true;
            try
            {
                var old = _createdDataBase.GetCurrentVersion();
                if (!old.Equals(new DataBaseVersion()))
                {
                    result = false;
                    await Shell.Current.GoToAsync(nameof(Shared.Pages.UpdateDataBase.UpdateDataBaseV));
                }
                await _createdDataBase.CreateBackUp();
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
            return result;
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
        }

        private void UserAfterLogin_OnLogin(User user, bool isLogin)
        {
            User = user;
            IsLogin = isLogin;
            UserName = $"Konto {user.Name}";
        }

        public void Dispose()
        {
            UserAfterLogin.OnLogin -= UserAfterLogin_OnLogin;
        }
    }
}
