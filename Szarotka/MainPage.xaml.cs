using DataBase.Data;
using DataBase.Model;
using DataBase.Service;

using Szarotka.Pages.Options.Main;

namespace Szarotka
{
    public partial class MainPage : ContentPage
    {
        private readonly CreatedDataBase _createdDataBase;
        private readonly AccessDataBase _db;
        public MainPage()
        {
            InitializeComponent();
            _db = new();
            _createdDataBase = new CreatedDataBase(_db);
        }


        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            try
            {
                var old = _createdDataBase.GetCurrentVersion();
                if (!old.Equals(new DataBaseVersion()))
                {
                    await Shell.Current.GoToAsync(nameof(DataBase.Pages.UpdateDataBase.UpdateDataBaseV));
                }
                await _createdDataBase.CreateBackUp();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
