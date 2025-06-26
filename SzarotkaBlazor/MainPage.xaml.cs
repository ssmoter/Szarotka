using DataBase.Data;
using DataBase.Model.EntitiesServer;

using DriversRoutes.Data.RouteApi;

using Shared.Helper;
using Shared.Pages.UpdateDifference;
using Shared.Service;


namespace SzarotkaBlazor
{
    public partial class MainPage : ContentPage
    {
        private readonly ICreatedDataBase _createdDataBase;
        private readonly IAccessDataBase _db;
        private readonly IGetCustomersHttp _customer;
        public MainPage(IAccessDataBase db, ICreatedDataBase createdDataBase, IGetCustomersHttp customer)
        {
            InitializeComponent();
            _db = db;
            _createdDataBase = createdDataBase;
            _customer = customer;
        }
        private async void Options_Clicked(object sender, EventArgs e)
        {
            //try
            //{
            //    var sourceToken = new CancellationTokenSource();
            //    var progress = UpdateProgressBar.CreatedUpdateProgressBar("Anuluj",
            //                    "Pobieranie listy punktów",
            //                    "logo2.png", async () =>
            //                    {
            //                        sourceToken.Cancel();
            //                        await Toast.Make("Anulowano pobieranie listy punktów").Show();
            //                        Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);

            //                    });
            //    var result = await _customer.GetCustomerRoutes(new Guid("baf3bb5a-59f6-5524-10d6-2d4c3c84b98b"),
            //          new() { Monday = true, Thursday = true }
            //          , progress, sourceToken.Token);
            //}
            //catch (Exception ex)
            //{
            //}
            var time = DateTime.Now;
            UpdateDifferences UpdateDifferences = new()
            {
                UpdateDifferencesDriverRoutes = []
            };
            UpdateDifferences.UpdateDifferencesDriverRoutes.Add(
                new UpdateDifferencesDriverRoutes()
                {
                    Server = new DataBase.Model.EntitiesRoutes.CustomerRoutes()
                    {
                        Created = time,
                        Updated = time.AddDays(5),
                        UserCreatedId = new Guid("0194d0ef-c56c-712e-b507-5b540f639749"),
                        Name = "Server",
                        DayOfWeek = new DataBase.Model.EntitiesRoutes.SelectedDayOfWeekRoutes()
                        {
                            Monday = true,
                            Thursday = true,
                            Wednesday = true,
                            Tuesday = true,
                            Friday = true,
                            Saturday = true,
                            Sunday = true,
                        },
                        ResidentialAddress = new DataBase.Model.EntitiesRoutes.ResidentialAddress()
                        {
                            Name = "Name",
                            Surname = "Surname",
                            Street = "Street",
                            HouseNumber = "1",
                            ApartmentNumber = "12",
                            PostalCode = "PostalCode",
                            City = "City",
                            Country = "Country",
                        }
                    },
                    Update = new DataBase.Model.EntitiesRoutes.CustomerRoutes()
                    {
                        Created = time,
                        UserCreatedId = new Guid("0194d0ef-c56c-712e-b507-5b540f639749"),
                        Updated = time,
                        Name = "Update",
                        DayOfWeek = new DataBase.Model.EntitiesRoutes.SelectedDayOfWeekRoutes()
                        {
                            Monday = true,
                            Thursday = true,
                        }
                    }
                });
            UpdateDifferences.UpdateDifferencesDriverRoutes.Add(
                new UpdateDifferencesDriverRoutes()
                {
                    Server = new DataBase.Model.EntitiesRoutes.CustomerRoutes()
                    {
                        Name = "test2",
                        DayOfWeek = new DataBase.Model.EntitiesRoutes.SelectedDayOfWeekRoutes()
                        {
                            Monday = true,
                            Thursday = true,
                        }
                    }
                });


            // await Shell.Current.GoToAsync(nameof(MainOptionsV));
            var navigationParameter = new Dictionary<string, object>
            {
                { nameof(UpdateDifferences), UpdateDifferences }
            };

            await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
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
