using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using DataBase.Helper;
using DataBase.Model.EntitiesInventory;

using Inventory.Service;


namespace Inventory.Pages.Main
{
    [QueryProperty(nameof(Day), nameof(DataBase.Model.EntitiesInventory.Day))]
    public partial class MainVM : ObservableObject
    {
        [ObservableProperty]
        string name;

        [ObservableProperty]
        MainM mainM;

        public Day Day { get; set; }

        readonly DataBase.Data.AccessDataBase _db;
        readonly Service.ISelectDayService _selectDayService;

        public MainVM(DataBase.Data.AccessDataBase db, ISelectDayService selectDay)
        {
            _db = db;
            _selectDayService = selectDay;
            MainM = new MainM();
            Name = "wybierz kierowcę";
            Service.DriverNameUpdateService.Update += SetName;
        }

        #region Method

        public async Task LookingForSelectedDriver()
        {
            try
            {
                var tableInfo = _db.DataBase.GetTableInfo(nameof(SelectedDriver));
                bool exist = tableInfo.Count > 0;
                if (exist)
                {
                    var selectedDriver = _db.DataBase.Table<SelectedDriver>().FirstOrDefault();
                    if (selectedDriver is not null)
                    {
                        var driver = _db.DataBase.Table<Driver>().FirstOrDefault(x => x.Id == selectedDriver.SelectedGuid);
                        Helper.SelectedDriver.Id = driver.Id.ToString();
                        Helper.SelectedDriver.Name = driver.Name;
                        Helper.SelectedDriver.Description = driver.Description;
                        SetName();
                    }
                }
                if (await MainVM.CheckDriver())
                {
                    await Shell.Current.GoToAsync("../MainOptionsV?",
                        new Dictionary<string, object>()
                        {
                            [nameof(ListOfEnums.TypOfOptions)] = ListOfEnums.TypOfOptions.Inventory,
                        });
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        static async Task<bool> CheckDriver()
        {
            if (string.IsNullOrWhiteSpace(Helper.SelectedDriver.Id))
            {
                await Shell.Current.DisplayAlert("Kierowca", $"Kierowca nie został wybrany{Environment.NewLine}Wybierz kierowcę w celu wczytania", "Ok");
                return true;
            }
            return false;
        }

        void SetName()
        {
            Name = Helper.SelectedDriver.Name;
        }



        #endregion

        #region Command

        [RelayCommand]
        async Task NavigationToSingleDay()
        {
            try
            {
                if (await MainVM.CheckDriver())
                    return;

                if (Day is not null)
                {
                    Day.Products = new(Day.Products.OrderBy(x => x.Name.Arrangement));
                }

                if (Day is null)
                {
                    Day = await _selectDayService.GetDayProcedure(DateTime.Now);
                }
                else if (Day.Created.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    Day = await _selectDayService.GetDayProcedure(DateTime.Now);
                }
                else if (Day.DriverGuid != new Guid(Helper.SelectedDriver.Id))
                {
                    Day = await _selectDayService.GetDayProcedure(DateTime.Now);
                }

                Day.CanUpadte = true;
                for (int i = 0; i < Day.Products.Count; i++)
                {
                    Day.Products[i].CanUpadte = true;
                }
                await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(DataBase.Model.EntitiesInventory.Day)] = Day,

                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
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
                if (await MainVM.CheckDriver())
                    return;
                if (string.IsNullOrWhiteSpace(MainM.DisplyDate))
                    return;


                var days = await _selectDayService.GetDayProcedure(MainM.Date);
                days.CanUpadte = true;
                for (int i = 0; i < days.Products.Count; i++)
                {
                    days.Products[i].CanUpadte = true;
                }
                await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(DataBase.Model.EntitiesInventory.Day)] = days
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }


        #endregion

    }
}
