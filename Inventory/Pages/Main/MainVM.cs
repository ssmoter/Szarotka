using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
            MainM=new MainM();
            Name = "wybierz kierowcę";
            Service.DriverNameUpdateService.Update += SetName;
            Task.Run(async () =>
            {
                var tableInfo = await db.DataBaseAsync.GetTableInfoAsync(nameof(Model.SelectedDriver));
                bool exist = tableInfo.Count > 0;
                if (exist)
                {
                    var selected = await db.DataBaseAsync.GetAsync<Model.SelectedDriver>(1);
                    var selectedDriver = await db.DataBaseAsync.Table<Model.Driver>().FirstOrDefaultAsync(x => x.Id == selected.SelectedGuid);
                    Helper.SelectedDriver.Id = selectedDriver.Id;
                    Helper.SelectedDriver.Name = selectedDriver.Name;
                    Helper.SelectedDriver.Description = selectedDriver.Description;

                    SetName();
                }
            });
        }

        #region Method

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
                if (dayM is null)
                {
                    dayM = await _selectDayService.GetDay();
                }
                else if (dayM.Created.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    dayM = await _selectDayService.GetDay();
                }
                else if (dayM.DriverGuid != Helper.SelectedDriver.Id)
                {
                    dayM = await _selectDayService.GetDay();
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
                if (string.IsNullOrWhiteSpace(MainM.DisplyDate))
                {
                    return;
                }

                dayM = await _selectDayService.GetDay(MainM.Date);

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


        #endregion
    }
}
