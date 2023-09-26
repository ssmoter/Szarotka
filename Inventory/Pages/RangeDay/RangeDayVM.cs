using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Data.File;
using Inventory.Helper.Parse;
using Inventory.Model;
using Inventory.Model.MVVM;
using Inventory.Service;

using System.Collections.ObjectModel;

using Szarotka.Service;

namespace Inventory.Pages.RangeDay
{
    public partial class RangeDayVM : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<RangeDayM> rangeDays;

        [ObservableProperty]
        DayM totalPriceOfRange;

        [ObservableProperty]
        ObservableCollection<string> ragne;

        [ObservableProperty]
        string selectedDriverName;

        [ObservableProperty]
        bool enableSave;

        string isSelectedDate;
        public string IsSelectedDate
        {
            get => isSelectedDate;
            set
            {
                if (SetProperty(ref isSelectedDate, value))
                {
                    OnPropertyChanged(nameof(IsSelectedDate));
                    Task.Run(async () =>
                    {
                        try
                        {
                            await SelectedDate(IsSelectedDate);
                        }
                        catch (Exception ex)
                        {
                            _db.SaveLog(ex);
                        }
                    });
                }
            }
        }

        readonly DataBase.Data.AccessDataBase _db;
        readonly ISelectDayService _selectDayService;
        readonly ISaveDayService _dayService;
        public RangeDayVM(DataBase.Data.AccessDataBase db, ISelectDayService selectDay, ISaveDayService dayService)
        {
            RangeDays = new ObservableCollection<RangeDayM>();
            TotalPriceOfRange = new DayM();
            _db = db;
            _selectDayService = selectDay;
            ragne = new ObservableCollection<string>
            {
                "Dzisiaj",
                "Tydzień",
                "Miesiąc",
                "Rok",
                //"Wybierz daty"
            };
            EnableSave = false;
            _dayService = dayService;
        }

        #region Method

        async Task SelectedDate(string range)
        {
            try
            {
                switch (range)
                {
                    case "Dzisiaj":
                        {
                            RangeDays = await SelectToday(SelectedDriverName);
                        }
                        break;
                    case "Tydzień":
                        {
                            RangeDays = await SelectWeek(SelectedDriverName);
                        }
                        break;
                    case "Miesiąc":
                        {
                            RangeDays = await SelectMonth(SelectedDriverName);
                        }
                        break;
                    case "Rok":
                        {
                            RangeDays = await SelectYear(SelectedDriverName);
                        }
                        break;
                    case "Wybierz daty":
                        {
                            RangeDays = await SelectMore(SelectedDriverName);
                        }
                        break;
                    default:
                        break;
                }
                TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                EnableSave = false;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        async Task<ObservableCollection<RangeDayM>> SelectToday(string selectedDriverName = "")
        {
            var now = DateTime.Now;
            var from = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, 0, 0).ToUniversalTime().Ticks;
            var to = new DateTime(now.Year, now.Month, now.Day + 1, 0, 0, 0, 0, 0).ToUniversalTime().Ticks;

            var result = await SelectDays(from, to, selectedDriverName);
            return result;
        }
        async Task<ObservableCollection<RangeDayM>> SelectWeek(string selectedDriverName = "")
        {
            DateTime now = DateTime.Today;
            DayOfWeek startDayOfWeek = DayOfWeek.Monday;
            DateTime startOfWeek = now.AddDays(-(now.DayOfWeek - startDayOfWeek)).Date;
            DateTime endOfWeek = startOfWeek.AddDays(+7).AddHours(-1);

            if (now.DayOfWeek == DayOfWeek.Sunday)
            {
                startOfWeek = now.AddDays(-(now.DayOfWeek - startDayOfWeek)).AddDays(-7).Date;
                endOfWeek = startOfWeek.AddDays(+7).AddHours(-1);
            }

            var from = startOfWeek.ToUniversalTime().Ticks;
            var to = endOfWeek.ToUniversalTime().Ticks;

            var result = await SelectDays(from, to, selectedDriverName);
            return result;
        }
        async Task<ObservableCollection<RangeDayM>> SelectMonth(string selectedDriverName = "")
        {
            var from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 2, 0, 0).ToUniversalTime();
            var to = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1, 2, 0, 0).ToUniversalTime();

            var result = await SelectDays(from.Ticks, to.Ticks, selectedDriverName);
            return result;
        }
        async Task<ObservableCollection<RangeDayM>> SelectYear(string selectedDriverName = "")
        {
            var from = new DateTime(DateTime.Today.Year, 1, 1, 1, 0, 0).ToUniversalTime();
            var to = new DateTime(DateTime.Today.AddYears(1).Year, 1, 1, 1, 0, 0).ToUniversalTime();

            var result = await SelectDays(from.Ticks, to.Ticks, selectedDriverName);
            return result;
        }
        async Task<ObservableCollection<RangeDayM>> SelectMore(string selectedDriverName = "")
        {

            //var popup = new PopupSelectRangeDate.PopupSelectRangeDateV();
            // Shell.Current?.ShowPopup(popup);

            var from = new DateTime().ToUniversalTime().Ticks;
            var to = new DateTime().ToUniversalTime().Ticks;

            var result = await SelectDays(from, to, selectedDriverName);
            return result;
        }


        async Task<ObservableCollection<RangeDayM>> SelectDays(long from, long to, string selectedDriverName = "")
        {
            Day[] days = Array.Empty<Day>();
            if (string.IsNullOrWhiteSpace(selectedDriverName))
            {
                days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().
                    Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to).
                    OrderByDescending(x => x.CreatedTicks).ToArrayAsync();
            }
            else
            {
                var selectedDriver = await _db.DataBaseAsync.Table<Driver>().FirstOrDefaultAsync(x => x.Name == selectedDriverName);
                days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().
                    Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to && x.DriverGuid == selectedDriver.Guid).
                    OrderByDescending(x => x.CreatedTicks).ToArrayAsync();
            }

            var dayM = new ObservableCollection<RangeDayM>();
            for (int i = 0; i < days.Length; i++)
            {
                dayM.Add(new RangeDayM());
                dayM[i].DayM = days[i].ParseAsDayM();
                var guid = days[i].DriverGuid;
                var driver = await _db.DataBaseAsync.Table<Driver>().FirstOrDefaultAsync(x => x.Guid == guid);
                if (driver is not null)
                {
                    dayM[i].Driver = driver.PareseAsDriverM();
                }
            }

            return dayM;
        }

        static DayM SumTotalPriceOfRange(ObservableCollection<RangeDayM> range)
        {
            var day = new DayM();
            var lastValue = Service.ProductUpdatePriceService.EnableUpdate;
            Service.ProductUpdatePriceService.EnableUpdate = false;

            day.TotalPriceProduct = range.Sum(x => x.DayM.TotalPriceProduct);
            day.TotalPriceCake = range.Sum(x => x.DayM.TotalPriceCake);
            day.TotalPrice = range.Sum(x => x.DayM.TotalPrice);
            day.TotalPriceCorrect = range.Sum(x => x.DayM.TotalPriceCorrect);
            day.TotalPriceMoney = range.Sum(x => x.DayM.TotalPriceMoney);
            day.TotalPriceDifference = range.Sum(x => x.DayM.TotalPriceDifference);
            day.TotalPriceAfterCorrect = range.Sum(x => x.DayM.TotalPriceAfterCorrect);

            Service.ProductUpdatePriceService.EnableUpdate = lastValue;
            return day;
        }

        async static Task<string> SelectImportExport()
        {
            var result = await Shell.Current.CurrentPage.DisplayActionSheet("Wybierz co chcesz wykonać", "Anuluj", null, "Import", "Eksport");
            return result;
        }

        static PickOptions FileTypCSV()
        {
            var pOptions = new PickOptions();
            var dictionaryTyp = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { "csv" } },
                { DevicePlatform.Android, new[] { "csv" } }
            };

            pOptions.FileTypes = new FilePickerFileType(dictionaryTyp);

            return pOptions;
        }
        static PickOptions FileTypJson()
        {
            var pOptions = new PickOptions();
            var dictionaryTyp = new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { "json", "txt" } },
                { DevicePlatform.Android, new[] { "json", "txt" } }
            };

            pOptions.FileTypes = new FilePickerFileType(dictionaryTyp);

            return pOptions;
        }

        static string CreateFileName()
        {
            return string.Join('_', "Szarotka", DateTime.Now.ToLocalTime().ToString("dd.MM.yyyy"));
        }

        #endregion

        #region Command

        [RelayCommand]
        async Task OpenDetailpage(RangeDayM rangeDay)
        {
            try
            {
                rangeDay.DayM = await _selectDayService.GetDay(rangeDay.DayM.Id);

                await Shell.Current.GoToAsync($"{nameof(Inventory.Pages.SingleDay.SingleDayV)}?",
                    new Dictionary<string, object>()
                    {
                        [nameof(Model.MVVM.DayM)] = rangeDay.DayM
                    });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task GenerateJsonFile()
        {
            try
            {
#if ANDROID
                if (!await AndroidPermissionService.CheckAllPermissions())
                {
                  //  return;
                }
#endif
                var result = await SelectImportExport();

                if (string.IsNullOrWhiteSpace(result))
                {
                    return;
                }

                if (result == "Anuluj")
                {
                    return;
                }
                if (result == "Import")
                {
                    var response = await FilePicker.PickAsync(FileTypJson());
                    var file = await FileManagerCSVJson.GetFileJson<ObservableCollection<RangeDayM>>(response.FullPath);
                    RangeDays = file;
                    TotalPriceOfRange = RangeDayVM.SumTotalPriceOfRange(RangeDays);
                    EnableSave = true;
                }
                if (result == "Eksport")  
                {
                    for (int i = 0; i < RangeDays.Count; i++)
                    {
                        RangeDays[i].DayM = await _selectDayService.GetDay(RangeDays[i].DayM.Id);
                    }
                    var response = await FileManagerCSVJson.SaveFileJson(RangeDays, CreateFileName());
                    if (response)
                    {
                        await Shell.Current.CurrentPage.DisplayAlert("Plik", "Plik został zapisany", "Ok");
                    }
                    else
                    {
                        await Shell.Current.CurrentPage.DisplayAlert("Plik", "Nie udało się zapisać", "Ok");
                    }
                }


            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task GenerateCSVFile()
        {
            try
            {
#if ANDROID
                if (!await AndroidPermissionService.CheckAllPermissions())
                {
                    return;
                }
#endif
                var result = await SelectImportExport();

                if (string.IsNullOrWhiteSpace(result))
                {
                    return;
                }
                if (result == "Anuluj")
                {
                    return;
                }
                if (result == "Import")
                {
                    var response = await FilePicker.PickAsync(FileTypCSV());

                }
                if (result == "Eksport")
                {

                }


            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        [RelayCommand]
        async Task SaveAnotherDriverData()
        {
            try
            {
                throw new NotImplementedException();
                for (int i = 0; i < RangeDays.Count; i++)
                {
                    
                }
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
            }
        }

        #endregion


    }
}
