using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Model;

using System.Collections.ObjectModel;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate
{
    public partial class PopupSelectRangeDateVM : ObservableObject, IDisposable
    {
        DateTime fromDate;
        public DateTime FromDate
        {
            get => fromDate;
            set
            {
                if (SetProperty(ref fromDate, value))
                {
                    OnPropertyChanged(nameof(FromDate));
                    from = FromDate.Ticks;
                }
            }
        }
        DateTime toDate;
        public DateTime ToDate
        {
            get => toDate;
            set
            {
                if (SetProperty(ref toDate, value))
                {
                    OnPropertyChanged(nameof(ToDate));
                    to = ToDate.AddHours(23).Ticks;
                }
            }
        }
        [ObservableProperty]
        ObservableCollection<string> rangeFast;

        [ObservableProperty]
        ObservableCollection<string> rangeMonth;

        string isSelectedDateFast;
        public string IsSelectedDateFast
        {
            get => isSelectedDateFast;
            set
            {
                if (SetProperty(ref isSelectedDateFast, value))
                {
                    OnPropertyChanged(nameof(IsSelectedDateFast));
                    if (!string.IsNullOrWhiteSpace(IsSelectedDateFast))
                    {
                        SelectedDate(IsSelectedDateFast);
                        IsSelectedDateMonth = string.Empty;
                    }
                }
            }
        }

        string isSelectedDateMonth;
        public string IsSelectedDateMonth
        {
            get => isSelectedDateMonth;
            set
            {
                if (SetProperty(ref isSelectedDateMonth, value))
                {
                    OnPropertyChanged(nameof(IsSelectedDateMonth));
                    if (!string.IsNullOrWhiteSpace(IsSelectedDateMonth))
                    {
                        SelectedDate(IsSelectedDateMonth);
                        IsSelectedDateFast = string.Empty;
                    }
                }
            }
        }

        long from = 0;
        long to = 0;

        public Func<object, Task> Close;
        public Task OnClose(object result = null)
        {
            return Close?.Invoke(result);
        }

        public PopupSelectRangeDateVM()
        {
            RangeFast ??= new ObservableCollection<string>
            {
                "Poprzedni tydzień",
                "Dzisiaj",
                "Tydzień",
                "Miesiąc",
                "Rok",
            };
            RangeMonth ??= new ObservableCollection<string>
            {
                "Styczeń",
                "Luty",
                "Marzec",
                "Kwiecień",
                "Maj",
                "Czerwiec",
                "Lipiec",
                "Sierpień",
                "Wrzesień",
                "Październik",
                "Listopad",
                "Grudzień",
            };
            FromDate = DateTime.Today.AddDays(-1);
            ToDate = DateTime.Today.AddDays(1);
        }


        #region Method

        void SelectedDate(string range)
        {
            (long, long) result = new();
            switch (range)
            {
                case "Poprzedni tydzień":
                    result = SelectLastWeek();
                    break;
                case "Dzisiaj":
                    result = SelectToday();
                    break;
                case "Tydzień":
                    result = SelectWeek();
                    break;
                case "Miesiąc":
                    result = SelectMonth(DateTime.Today.Month);
                    break;
                case "Rok":
                    result = SelectYear();
                    break;


                case "Styczeń":
                    result = SelectMonth(1);
                    break;
                case "Luty":
                    result = SelectMonth(2);
                    break;
                case "Marzec":
                    result = SelectMonth(3);
                    break;
                case "Kwiecień":
                    result = SelectMonth(4);
                    break;

                case "Maj":
                    result = SelectMonth(5);
                    break;
                case "Czerwiec":
                    result = SelectMonth(6);
                    break;
                case "Lipiec":
                    result = SelectMonth(7);
                    break;
                case "Sierpień":
                    result = SelectMonth(8);
                    break;

                case "Wrzesień":
                    result = SelectMonth(9);
                    break;
                case "Październik":
                    result = SelectMonth(10);
                    break;
                case "Listopad":
                    result = SelectMonth(11);
                    break;
                case "Grudzień":
                    result = SelectMonth(12);
                    break;


                default:
                    break;
            }
            from = result.Item1;
            to = result.Item2;
            FromDate = new DateTime(from);
            ToDate = new DateTime(to);

        }
        static (long, long) SelectLastWeek()
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

            var from = startOfWeek.AddDays(-7).ToUniversalTime();
            var to = endOfWeek.AddDays(-7).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }
        static (long, long) SelectToday()
        {
            var now = DateTime.Today;
            var from = new DateTime(now.Ticks).ToUniversalTime();
            var to = new DateTime(now.AddHours(23).Ticks).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }
        static (long, long) SelectWeek()
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

            var from = startOfWeek.ToUniversalTime();
            var to = endOfWeek.ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }
        static (long, long) SelectYear()
        {
            var from = new DateTime(DateTime.Today.Year, 1, 1, 1, 0, 0).ToUniversalTime();
            var to = new DateTime(DateTime.Today.AddYears(1).Year, 1, 1, 1, 0, 0).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }
        static (long, long) SelectMonth(int month = 0)
        {
            DateTime to;
            var from = new DateTime(DateTime.Today.Year, month, 1, 2, 0, 0).ToUniversalTime();
            if (month == 12)
                to = new DateTime(DateTime.Today.Year + 1, 1, 1, 2, 0, 0).ToUniversalTime();
            else
                to = new DateTime(DateTime.Today.Year, from.AddMonths(1).Month, 1, 2, 0, 0).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }



        #endregion


        #region Command
        [RelayCommand]
        async Task SaveAndReturn()
        {
            try
            {
                await OnClose(new PopupDateModel(from, to));
            }
            catch (Exception)
            {
            }
            finally { Dispose(); }
        }
        [RelayCommand]
        async Task CancelAndRetur()
        {
            try
            {
                await OnClose(null);
            }
            catch (Exception)
            {
            }
            finally
            {
                Dispose();
            }
        }


        #endregion
        public void Dispose()
        {
            this.RangeFast?.Clear();
            this.RangeMonth?.Clear();

        }
    }
}
