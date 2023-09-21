using CommunityToolkit.Mvvm.ComponentModel;

using Inventory.Helper;
using Inventory.Model;
using Inventory.Model.MVVM;

using System.Collections.ObjectModel;

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
                        await SelectedDate(IsSelectedDate);
                        int a;
                    });
                }
            }
        }

        DataBase.Data.AccessDataBase _db;

        public RangeDayVM(DataBase.Data.AccessDataBase db)
        {
            RangeDays = new ObservableCollection<RangeDayM>();
            TotalPriceOfRange = new DayM();
            _db = db;
            ragne = new ObservableCollection<string>();
            ragne.Add("Dzień");
            ragne.Add("Tydzień");
            ragne.Add("Miesiąc");
            ragne.Add("Rok");
            ragne.Add("Wybierz dzień");
        }

        #region Method

        async Task SelectedDate(string range)
        {
            try
            {

                switch (range)
                {
                    case "Dzień":
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
                TotalPriceOfRange = SumTotalPriceOfRange(RangeDays);
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
            DateTime endOfWeek = startOfWeek.AddDays(6).AddHours(23);

            var from = startOfWeek.ToUniversalTime().Ticks;
            var to = endOfWeek.ToUniversalTime().Ticks;

            var result = await SelectDays(from, to, selectedDriverName);
            return result;
        }
        async Task<ObservableCollection<RangeDayM>> SelectMonth(string selectedDriverName = "")
        {
            var from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToUniversalTime().Ticks;
            var to = new DateTime(DateTime.Today.AddDays(1).Ticks).ToUniversalTime().Ticks;

            var result = await SelectDays(from, to, selectedDriverName);
            return result;
        }
        async Task<ObservableCollection<RangeDayM>> SelectYear(string selectedDriverName = "")
        {
            var from = new DateTime(DateTime.Today.Year, 1, 1).ToUniversalTime().Ticks;
            var to = new DateTime(DateTime.Today.AddDays(1).Ticks).ToUniversalTime().Ticks;

            var result = await SelectDays(from, to, selectedDriverName);
            return result;
        }
        async Task<ObservableCollection<RangeDayM>> SelectMore(string selectedDriverName = "")
        {
            var from = new DateTime().ToUniversalTime().Ticks;
            var to = new DateTime().ToUniversalTime().Ticks;

            var result = await SelectDays(from, to, selectedDriverName);
            return result;
        }


        async Task<ObservableCollection<RangeDayM>> SelectDays(long from, long to, string selectedDriverName = "")
        {
            Day[] days = new Day[0];
            if (string.IsNullOrWhiteSpace(selectedDriverName))
            {
                days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to).ToArrayAsync();
            }
            else
            {
                var selectedDriver = await _db.DataBaseAsync.Table<Driver>().FirstOrDefaultAsync(x => x.Name == selectedDriverName);
                days = await _db.DataBaseAsync.Table<Inventory.Model.Day>().Where(x => x.CreatedTicks >= from && x.CreatedTicks <= to && x.DriverGuid == selectedDriver.Guid).ToArrayAsync();
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

        DayM SumTotalPriceOfRange(ObservableCollection<RangeDayM> range)
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

        #endregion
    }
}
