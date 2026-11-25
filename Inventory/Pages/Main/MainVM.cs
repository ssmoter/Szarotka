using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Data;
using DataBase.Data.Get;
using DataBase.Data.Save;
using DataBase.Helper;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Shared.CustomControls.FromCode;
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
        private readonly Data.InventoryApi.IGetProductHttp _getHttp;
        private readonly ISaveInventoryAoT _save;
        private readonly IUpdateLogService _updateLogService;

        public MainVM(IAccessDataBaseAoT db,
                      DataBase.Data.Get.IGetInventoryAoT get,
                      DataBase.Service.ITimeService time,
                      IGetDriverRoutesAoT getRoutes,
                      Data.InventoryApi.IGetProductHttp getHttp,
                      ISaveInventoryAoT save,
                      IUpdateLogService updateLogService)
        {
            _db = db;
            _get = get;
            MainM = new MainM();
            _time = time;
            _getRoutes = getRoutes;
            _getHttp = getHttp;
            _save = save;
            _updateLogService = updateLogService;
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

                if (products.Count == 0)
                {
                    await GetProductIfEmpty();
                    products = await _get.EmptyProducts();
                }

                var productsSorted = products.Where(x => x.Name.IsVisible && !x.Name.IsDelete).OrderBy(x => x.Name.Arrangement);

                var route = await GetSelectedRoute();
                if (route == Guid.Empty)
                {
                    throw new Exception("Nie wybrano trasy");
                }
                day.SelectedRoute = route;

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

        private async Task GetProductIfEmpty()
        {
            await Toast.Make("Rozpoczęto Pobieranie").Show();
            using var sourceToken = new CancellationTokenSource();
            try
            {
                using var progress = UpdateProgressBar.CreatedUpdateProgressBar(
                                title: "Anuluj"
                                , description: "Pobieranie listy produktów"
                                , icon: UpdateProgressBar.GetSyncImage()
                                , rotateIcon: true
                                , action: async () =>
                                {
                                    sourceToken.Cancel();
                                    await Toast.Make("Anulowano pobieranie listy produktów").Show();
                                    Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                                });
                await Toast.Make("Pobieranie zakończone").Show();
                var result = await _getHttp.GetProducts(progress, sourceToken.Token);
                await Toast.Make("Rozpoczęto zapisywanie").Show();


                foreach (EmptyProduct item in result.Products)
                {

                    await _save.SaveProductName(item.Name, item.Name.UserUpdatedId.ToByteArray(), true);
                    var log = await _updateLogService.Insert(new DataBase.Model.UpdateLog()
                    {
                        IsServer = false,
                    }, item.Name);


                    foreach (var price in item.Prices)
                    {
                        await _save.SaveProductPrice(price, price.UserUpdatedId.ToByteArray(), true);
                    }
                }
                await Toast.Make("Zapisywanie zakończone").Show();
            }
            catch (Exception ex)
            {
                _db.SaveLogExtension(ex);
            }
            finally
            {
                Shared.Pages.FlyoutHeader.FlyoutHeaderVM.OnCustomContent(null);
                sourceToken.Dispose();
                Shell.Current.FlyoutIsPresented = false;
            }
        }


        [RelayCommand]
        async Task test()
        {


            //Day day = null;
            //if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            //    day = await _get.DaySelectedDateString("12.10.2026", UserAfterLogin.User.Id);
            //else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
            //    day = await _get.Day(new Guid("01a111a4-c0cb-7989-b587-cd35e163e5de"));

            //var product = await _get.EmptyProducts();

            //var customer = new CustomerRoutes()
            //{
            //    DayOfWeek = new(),
            //    ResidentialAddress = new(),
            //};

            //IList<UpdateDifference> exists = [];
            //exists.Add(new UpdateDifference() { Index = 1, Server = day, Update = day });
            //exists.Add(new UpdateDifference() { Index = 2, Server = day, Update = day });
            //exists.Add(new UpdateDifference() { Index = 3, Server = product[0].Name, Update = product[0].Name });
            //exists.Add(new UpdateDifference() { Index = 4, Server = product[1].Name, Update = product[1].Name });
            //exists.Add(new UpdateDifference() { Index = 5, Server = customer, Update = customer });
            //var navigationParameter = new Dictionary<string, object>
            //         {
            //            { nameof(UpdateDifference), exists },
            //            {nameof(Action),(Action<IEnumerable>)(async (days)
            //            =>
            //                {
            //                })
            //            }
            //        };

            //await Shell.Current.GoToAsync(nameof(UpdateDifferenceV), navigationParameter);
            var a = await _get.DayCalculationType(0, DateTime.MaxValue.Ticks, [], DayCalculationType.MedianWeek);
        }
    }
}

