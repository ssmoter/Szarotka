using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Helper;
using Inventory.Helper.Parse;
using Inventory.Model.MVVM;
using Inventory.Service;

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
        readonly Service.ISaveDayService _saveDay;

        public SingleDayVM(DataBase.Data.AccessDataBase db, ISaveDayService saveDay)
        {
            Initialize();
            _db = db;
            _saveDay = saveDay;
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
            if (DayM.DriverGuid == Guid.Empty)
            {
                await Shell.Current.DisplayAlert("Kierowca", $"Kierowca nie został wybrany{Environment.NewLine}Wybierz kierowcę w celu zapisania", "Ok");
                return true;
            }
            return false;
        }

        void FastChangeProduct(ProductM product, int value)
        {
            if (product is not null)
            {
                product.NumberEdit += value;
            }
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
        void FastAddProduct(ProductM productM)
        {
            FastChangeProduct(productM, 1);
        }
        [RelayCommand]
        void FastMinusProduct(ProductM productM)
        {
            FastChangeProduct(productM, -1);
        }

        [RelayCommand]
        async Task SaveDay()
        {
            await _saveDay.SaveDayMAsync(DayM);
        }

        [RelayCommand]
        async Task AddCake()
        {
            try
            {
                if (DayM is null)
                {
                    return;
                }
                if (DayM.Cakes is null)
                {
                    return;
                }

#if __ANDROID_24__
                var response = await Shell.Current.DisplayPromptAsync("Ciasto", "Podaj cene ciasta", "Tak", "Anuluj", keyboard: Keyboard.Telephone);
#else
                var response = await Shell.Current.DisplayPromptAsync("Ciasto", "Podaj cene ciasta", "Tak", "Anuluj", keyboard: Keyboard.Numeric);
#endif

                if (string.IsNullOrWhiteSpace(response))
                {
                    return;
                }
                response = response.Replace('.', ',');
                if (decimal.TryParse(response, DataBase.Helper.Constants.CultureInfo, out decimal value))
                {
                    var cake = new CakeM()
                    {
                        IsSell = false,
                        Price = value,
                        DayId = DayM.Id,
                    };

                    DayM.Cakes.Add(cake);

                    await Helper.SnackbarAsToats.OnShow("Dodano ciasto");
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task DeleteCake(CakeM cake)
        {
            try
            {
                if (DayM is null)
                {
                    return;
                }
                if (DayM.Cakes is null)
                {
                    return;
                }

                await _db.DataBaseAsync.DeleteAsync(cake.PareseAsCake());
                DayM.Cakes.Remove(cake);
                Inventory.Service.ProductUpdatePriceService.OnUpdate();
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        [RelayCommand]
        async Task Back()
        {
            try
            {
                await SaveDay();
                await Shell.Current.GoToAsync("..?",
                    new Dictionary<string, object>()
                    {
                        [nameof(Model.MVVM.DayM)] = DayM
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        [RelayCommand]
        async Task BackWithoutSave()
        {
            try
            {
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }
        #endregion
    }
}
