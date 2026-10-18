using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Helper;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;

using Shared.Data;
using Shared.Helper;
using Shared.Pages.UpdateDifference;

using System.Collections;


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

        private readonly IAccessDataBaseAoT _db;
        private readonly DataBase.Data.Get.IGetInventoryAoT _get;
        private readonly DataBase.Service.ITimeService _time;
        private readonly IGetDriverRoutesAoT _getRoutes;

        public MainVM(IAccessDataBaseAoT db,
                      DataBase.Data.Get.IGetInventoryAoT get,
                      DataBase.Service.ITimeService time,
                      IGetDriverRoutesAoT getRoutes)
        {
            _db = db;
            _get = get;
            MainM = new MainM();
            _time = time;
            _getRoutes = getRoutes;
        }

        [RelayCommand]
        async Task NavigationToSingleDay()
        {
            try
            {
                var day = await GetDay(_time.UtcNow().ToString("dd.MM.yyyy"));
                if (day.Id == Guid.Empty)
                {
                    day.SelectedDate = _time.UtcNow();
                }


                await GoToDay(day);
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
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
                var day = await GetDay(MainM.Date.ToString("dd.MM.yyyy"));
                if (day.Id == Guid.Empty)
                {
                    day.SelectedDate = new DateTime(MainM.Date.Year, MainM.Date.Month, MainM.Date.Day, 12, 0, 0);
                }
                await GoToDay(day);
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
        }

        private async Task GoToDay(Day days)
        {
            if (days.Id == Guid.Empty)
            {
                var result = await Shell.Current.DisplayAlertAsync("Czy chcesz utworzyć nowy wpis",
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
                    });
            }
        }
        private async Task<DataBase.Model.EntitiesInventory.Day> GetDay(string shortDate)
        {
            var userId = UserAfterLogin.User.Id;
            DataBase.Model.EntitiesInventory.Day day = await _get.DaySelectedDateString(shortDate, userId);

            if (day is null)
            {
                day = new();
                var products = await _get.EmptyProducts();
                var productsSorted = products.Where(x => x.Name.IsVisible && !x.Name.IsDelete).OrderBy(x => x.Name.Arrangement);

                day.SelectedRoute = await GetSelectedRoute();

                day.Products = new System.Collections.ObjectModel.ObservableCollection<DataBase.Model.EntitiesInventory.Product>(productsSorted);
            }

            return day;
        }

        private async Task<Guid> GetSelectedRoute()
        {
            var name = $"{nameof(Day)}.{nameof(Day.SelectedRoute)}";
            var route = await name.GetHelperTableAsync(_db);
            if (route is not null && !string.IsNullOrWhiteSpace(route.Value))
            {
                return new Guid(route.Value);
            }

            var all = await _getRoutes.Routes();
            Guid selected = Guid.Empty;

            var result = await Shell.Current.DisplayActionSheetAsync("Wybierz trasę", "Anuluj", null, [.. all.Select(x => x.Name)]);

            selected = all.FirstOrDefault(x => x.Name == result)?.Id ?? Guid.Empty;

            if (selected != Guid.Empty)
            {
                var helperTable = new HelperTable()
                {
                    Name = name,
                    Value = selected.ToString(),
                    Created = _db.TimeService.UtcNow(),
                    Updated = _db.TimeService.UtcNow(),
                    Id = Guid.CreateVersion7(),
                    UserCreatedId = UserAfterLogin.User.Id,
                    UserUpdatedId = UserAfterLogin.User.Id,
                };
                await helperTable.SetAsync(_db);
            }
            return selected;
        }




        [RelayCommand]
        async Task test()
        {
            Day day = null;
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                day = await _get.DaySelectedDateString("12.10.2026", UserAfterLogin.User.Id);
            else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
                day = await _get.Day(new Guid("01a111a4-c0cb-7989-b587-cd35e163e5de"));

            var product = await _get.EmptyProducts();

            var customer = new CustomerRoutes()
            {
                DayOfWeek = new(),
                ResidentialAddress = new(),
            };

            IList<UpdateDifference> exists = [];
            exists.Add(new UpdateDifference() { Index = 1, Server = day, Update = day });
            exists.Add(new UpdateDifference() { Index = 2, Server = day, Update = day });
            exists.Add(new UpdateDifference() { Index = 3, Server = product[0].Name, Update = product[0].Name });
            exists.Add(new UpdateDifference() { Index = 4, Server = product[1].Name, Update = product[1].Name });
            exists.Add(new UpdateDifference() { Index = 5, Server = customer, Update = customer });
            var navigationParameter = new Dictionary<string, object>
                     {
                        { nameof(UpdateDifference), exists },
                        {nameof(Action),(Action<IEnumerable>)(async (days)
                        =>
                            {
                            })
                        }
                    };

            await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
        }
    }
}

