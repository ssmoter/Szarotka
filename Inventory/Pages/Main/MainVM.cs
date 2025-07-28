using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.Get;

using Shared.Data;
using Shared.Helper;


namespace Inventory.Pages.Main
{
    public partial class MainVM : ObservableObject
    {
        private MainM mainM;
        public MainM MainM
        {
            get => mainM;
            set
            {
                if (SetProperty(ref mainM, value, nameof(MainM))) { }
            }
        }

        private readonly IAccessDataBase _db;
        private readonly DataBase.Data.Get.IGetInventoryAoT _get;
        private readonly DataBase.Service.ITimeService _time;

        public MainVM(IAccessDataBase db,
                      DataBase.Data.Get.IGetInventoryAoT get,
                      DataBase.Service.ITimeService time)
        {
            _db = db;
            _get = get;
            MainM = new MainM();
            _time = time;
        }

        [RelayCommand]
        async Task NavigationToSingleDay()
        {
            try
            {
                DataBase.Model.EntitiesInventory.Day day = await GetDay(_time.UtcNow().ToShortDateString());

                if (day.Id == Guid.Empty)
                {

                    var result = await Shell.Current.DisplayAlert("Czy chcesz utworzyć nowy wpis",
                        "Wraz z utworzeniem nowego dnia dane są automatycznie zapisywane",
                        "Utwórz", "Anuluj");

                    if (result)
                    {
                        await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = day,
                        });
                    }
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = day,
                            [nameof(DataBase.Model.EntitiesInventory.Driver)] = UserAfterLogin.User.Name,

                        });
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        private async Task<DataBase.Model.EntitiesInventory.Day> GetDay(string shortDate)
        {
            var userId = UserAfterLogin.User.Id;
            DataBase.Model.EntitiesInventory.Day day = await _get.DaySelectedDateString(shortDate, userId);

            if (day is null)
            {
                var products = await _get.EmptyProducts();
                day.Products = new System.Collections.ObjectModel.ObservableCollection<DataBase.Model.EntitiesInventory.Product>(products);
            }

            return day;
        }

        [RelayCommand]
        async Task NavigationToRange()
        {
            await Shell.Current.GoToAsync(nameof(Inventory.Pages.RangeDay.RangeDayV));
        }

        [RelayCommand]
        async Task NavigationToEdit()
        {
            await Shell.Current.GoToAsync(nameof(Inventory.Pages.Products.ListProduct.ListProductV));
        }

        [RelayCommand]
        async Task NavigationToSingleDayWitchSelectedDay()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(MainM.DisplayDate))
                    return;

                var days = await GetDay(MainM.Date.ToShortDateString());

                if (days.Id == Guid.Empty)
                {
                    var result = await Shell.Current.DisplayAlert("Czy chcesz utworzyć nowy wpis",
                                "Wraz z utworzeniem nowego dnia dane są automatycznie zapisywane",
                                "Utwórz", "Anuluj");

                    if (result)
                    {
                        await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = days,
                        });
                    }
                }
                else
                {
                    await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageV)}?",
                        new Dictionary<string, object>()
                        {
                            [nameof(DataBase.Model.EntitiesInventory.Day)] = days,
                            [nameof(DataBase.Model.EntitiesInventory.Driver)] = Helper.SelectedDriver.Name,
                        });
                }
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

    }
}
