using DataBase.Model;
using DataBase.Service;

using Szarotka.Pages.Options.Main;

namespace Szarotka
{
    public partial class MainPage : ContentPage
    {
        readonly ICreatedDataBase _createdDataBase;
        public MainPage()
        {
            InitializeComponent();
            _createdDataBase = new DataBase.Data.CreatedDataBase(new DataBase.Data.AccessDataBase());
        }


        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            var old = _createdDataBase.GetCurrentVersion();
            if (!old.Equals(new DataBaseVersion()))
            {
                await Shell.Current.GoToAsync(nameof(DataBase.Pages.UpdateDataBase.UpdateDataBaseV));
            }
        }


        private async void DataBase_Clicked(object sender, EventArgs e)
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
