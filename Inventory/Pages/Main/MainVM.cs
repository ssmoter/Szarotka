using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Model;
using Inventory.Service;

namespace Inventory.Pages.Main
{
    [QueryProperty(nameof(dayM), nameof(Model.MVVM.DayM))]
    public partial class MainVM : ObservableObject
    {
        [ObservableProperty]
        string name;

        [ObservableProperty]
        MainM mainM;


        Model.MVVM.DayM dayM;

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

        public void LookingForSelectedDriver()
        {
            var tableInfo = _db.DataBase.GetTableInfo(nameof(Model.SelectedDriver));
            bool exist = tableInfo.Count > 0;
            if (exist)
            {
                var selectedDriver = _db.DataBase.Table<Model.SelectedDriver>().FirstOrDefault();
                if (selectedDriver is not null)
                {
                    var driver = _db.DataBase.Table<Model.Driver>().FirstOrDefault(x => x.Id == selectedDriver.SelectedGuid);
                    Helper.SelectedDriver.Id = driver.Id.ToString();
                    Helper.SelectedDriver.Name = driver.Name;
                    Helper.SelectedDriver.Description = driver.Description;
                    SetName();
                }
            }
            if (CheckDriver())
            {
                Shell.Current.GoToAsync("..");
            }
        }

        #region Method

        bool CheckDriver()
        {
            if (string.IsNullOrWhiteSpace(Helper.SelectedDriver.Id))
            {
                Shell.Current.DisplayAlert("Kierowca", $"Kierowca nie został wybrany{Environment.NewLine}Wybierz kierowcę w celu wczytania", "Ok");
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
                if (CheckDriver())
                    return;

                if (dayM is null)
                {
                    dayM = await _selectDayService.GetDayProcedure(DateTime.Now);
                }
                else if (dayM.Created.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    dayM = await _selectDayService.GetDayProcedure(DateTime.Now);
                }
                else if (dayM.DriverGuid != new Guid(Helper.SelectedDriver.Id))
                {
                    dayM = await _selectDayService.GetDayProcedure(DateTime.Now);
                }

                dayM.CanUpadte = true;
                for (int i = 0; i < dayM.Products.Count; i++)
                {
                    dayM.Products[i].CanUpadte = true;
                }

                await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(Model.MVVM.DayM)] = dayM
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
                if (CheckDriver())
                    return;
                if (string.IsNullOrWhiteSpace(MainM.DisplyDate))
                {
                    return;
                }

                var day = await _selectDayService.GetDayProcedure(MainM.Date);

                //dayM = await _selectDayService.GetDay(MainM.Date.AddHours(12));
                day.CanUpadte = true;
                for (int i = 0; i < day.Products.Count; i++)
                {
                    day.Products[i].CanUpadte = true;
                }
                await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(Model.MVVM.DayM)] = day
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
