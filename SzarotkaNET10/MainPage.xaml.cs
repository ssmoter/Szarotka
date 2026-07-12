using DataBase.Model.EntitiesServer;

using DriversRoutes.Data.GoogleApi;

using Shared.Helper;

using SzarotkaNET10.Pages.Options.Main;


namespace SzarotkaNET10
{
    public partial class MainPage : ContentPage
    {
        private readonly IRoutes _routes;
        public MainPage(IRoutes routes)
        {
            InitializeComponent();
            _routes = routes;
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var user = UserAfterLogin.User;
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(User), user }
            };

            await Shell.Current.GoToAsync(nameof(Shared.Pages.UserDisplay.UserDisplayV), navigationParameter);
        }

    }
}
