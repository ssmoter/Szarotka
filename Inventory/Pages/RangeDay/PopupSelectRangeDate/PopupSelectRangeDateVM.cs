using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Model;

using System.Collections.ObjectModel;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate
{
    public partial class PopupSelectRangeDateVM : ObservableObject
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
                    to = ToDate.Ticks;
                }
            }
        }
        [ObservableProperty]
        ObservableCollection<string> ragne;

        string isSelectedDate;
        public string IsSelectedDate
        {
            get => isSelectedDate;
            set
            {
                if (SetProperty(ref isSelectedDate, value))
                {
                    OnPropertyChanged(nameof(IsSelectedDate));
                    SelectedDate(IsSelectedDate);
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
            ragne = new ObservableCollection<string>
            {
                "Dzisiaj",
                "Tydzień",
                "Miesiąc",
                "Rok",
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
                case "Dzisiaj":
                    {
                        result = SelectToday();
                    }
                    break;
                case "Tydzień":
                    {
                        result = SelectWeek();
                    }
                    break;
                case "Miesiąc":
                    {
                        result = SelectMonth();
                    }
                    break;
                case "Rok":
                    {
                        result = SelectYear();
                    }
                    break;
                default:
                    break;
            }
            from = result.Item1;
            to = result.Item2;
            FromDate = new DateTime(from);
            ToDate = new DateTime(to);
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
        static (long, long) SelectMonth()
        {
            var from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 2, 0, 0).ToUniversalTime();
            var to = new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1, 2, 0, 0).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }
        static (long, long) SelectYear()
        {
            var from = new DateTime(DateTime.Today.Year, 1, 1, 1, 0, 0).ToUniversalTime();
            var to = new DateTime(DateTime.Today.AddYears(1).Year, 1, 1, 1, 0, 0).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }


        #endregion


        #region Command
        [RelayCommand]
        async Task SaveAndReturn()
        {
            await OnClose(new PopupDateModel(from, to));
        }
        [RelayCommand]
        async Task CancelAndRetur()
        {
            await OnClose(null);
        }

        #endregion
    }
}
