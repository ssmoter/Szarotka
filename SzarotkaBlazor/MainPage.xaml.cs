using DataBase.Data;
using DataBase.Model;

using Shared.Data;
using Shared.Helper;
using Shared.Model;

using SzarotkaBlazor.Pages.Options.Main;


namespace SzarotkaBlazor
{
    public partial class MainPage : ContentPage
    {
        private readonly CreatedDataBase _createdDataBase;
        private readonly AccessDataBase _db;
        public MainPage(AccessDataBase db)
        {
            InitializeComponent();
            _db = db;
            _createdDataBase = new(db);
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            var update = UpdateDataBase();
            await update;
            if (update.IsCompleted)
            {
                await GotToLogin();
            }
        }

        private async Task GotToLogin()
        {
            var user = await HelperTable.Get(nameof(UserAfterLogin.User.Token), _db);
            if (user is not null)
            {
                UserAfterLogin.SetLoginUser(user.Value);
            }

            if (!UserAfterLogin.IsLogin)
            {
                await Shell.Current.GoToAsync($"{nameof(Shared.Pages.LogIn.LogInV)}");
            }
        }
        private async Task UpdateDataBase()
        {
            try
            {
                var old = _createdDataBase.GetCurrentVersion();
                if (!old.Equals(new DataBaseVersion()))
                {
                    await Shell.Current.GoToAsync(nameof(Shared.Pages.UpdateDataBase.UpdateDataBaseV));
                }
                await _createdDataBase.CreateBackUp();
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        private async void Options_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MainOptionsV));
        }

        private async void Inventory_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Inventory.Pages.Main.MainV));
        }

        private async void Maps_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DriversRoutes.Pages.Main.MainVDriversRoutesV));
        }
    }
}
