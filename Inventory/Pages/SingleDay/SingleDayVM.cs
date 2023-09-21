using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Helper;
using Inventory.Model.MVVM;

namespace Inventory.Pages.SingleDay
{

    [QueryProperty(nameof(DayM), nameof(Model.MVVM.DayM))]
    public partial class SingleDayVM : ObservableObject
    {
        [ObservableProperty]
        DayM dayM;

        [ObservableProperty]
        SingleDayM singleDayM;


        readonly DataBase.Data.AccessDataBase _db;


        public SingleDayVM(DataBase.Data.AccessDataBase db)
        {
            Initialize();
            _db = db;
        }



        #region Method
        public async Task ShowCurrentDay()
        {
            await SnackbarAsToats.OnShow($"Wczytano dzień {DayM.Created.ToShortDateString()}");
        }

        void Initialize()
        {
            DayM = new DayM();
            SingleDayM = new SingleDayM();
        }

        /// <summary>
        /// return true when is no guid
        /// </summary>
        /// <returns></returns>
        async Task<bool> CheckDriver()
        {
            var guid = DayM.DriverGuid;
            if (DayM.DriverGuid == Guid.Empty)
            {
                guid = Helper.SelectedDriver.Guid;
            }
            if (guid == Guid.Empty)
            {
                await Shell.Current.DisplayAlert("Kierowca", $"Kierowca nie został wybrany{Environment.NewLine}Wybierz kierowcę w celu zapisania", "Ok");
                return true;
            }
            return false;
        }

        #endregion
        #region Command



        [RelayCommand]
        void ChangeProductVisibility()
        {
            SingleDayM.ProductIsVisible = true;
            SingleDayM.CakeIsVisible = false;
        }
        [RelayCommand]
        void ChangeCakeVisibility()
        {
            SingleDayM.CakeIsVisible = true;
            SingleDayM.ProductIsVisible = false;
        }

        [RelayCommand]
        async Task SaveDay()
        {
            if (DayM is not null)
            {
                if (await CheckDriver())
                {
                    return;
                }
                DayM.DriverGuid = Helper.SelectedDriver.Guid;
                var day = DayM.ParseAsDay();
                if (day.Id > 0)
                {
                    await _db.DataBaseAsync.UpdateAsync(day);
                }
                else
                {
                    await _db.DataBaseAsync.InsertAsync(day);
                }

                for (int i = 0; i < day.Products.Count; i++)
                {
                    day.Products[i].DayId = day.Id;
                    if (day.Products[i].Id > 0)
                    {
                        await _db.DataBaseAsync.UpdateAsync(day.Products[i]);
                    }
                    else
                    {
                        await _db.DataBaseAsync.InsertAsync(day.Products[i]);
                    }
                }

                for (int i = 0; i < day.Cakes.Count; i++)
                {
                    day.Cakes[i].DayId = day.Id;
                    if (day.Cakes[i].Id > 0)
                    {
                        await _db.DataBaseAsync.UpdateAsync(day.Cakes[i]);
                    }
                    else
                    {
                        await _db.DataBaseAsync.InsertAsync(day.Cakes[i]);
                    }

                }
                await SnackbarAsToats.OnShow("Zapisano");
            }
        }

        [RelayCommand]
        async Task AddCake()
        {
            if (DayM is null)
            {
                return;
            }
            if (DayM.Cakes is null)
            {
                return;
            }

            var response = await Shell.Current.DisplayPromptAsync("Ciasto", "Podaj cene ciasta", "Tak", "Anuluj", keyboard: Keyboard.Numeric);
            var cake = new CakeM()
            {
                IsSell = false,
                Price = decimal.Parse(response),
                DayId = DayM.Id,
            };

            DayM.Cakes.Add(cake);

            await Helper.SnackbarAsToats.OnShow("Dodano ciasto");
        }

        [RelayCommand]
        void DeleteCake(CakeM cake)
        {
            if (DayM is null)
            {
                return;
            }
            if (DayM.Cakes is null)
            {
                return;
            }
            DayM.Cakes.Remove(cake);
            Inventory.Service.ProductUpdatePriceService.OnUpdate();
        }
        [RelayCommand]
        async Task Back()
        {
            await SaveDay();
            await Shell.Current.GoToAsync("..?",
                new Dictionary<string, object>()
                {
                    [nameof(Model.MVVM.DayM)] = DayM
                });
        }

        #endregion
    }
}
