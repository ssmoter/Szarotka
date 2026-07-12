using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Inventory.Model;

using System.Collections.ObjectModel;

namespace Inventory.Pages.RangeDay.PopupSelectRangeDate
{
    public partial class PopupSelectRangeDateVM : ObservableObject, IQueryAttributable
    {
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue(nameof(PopupDateModel), out object popupDateModel))
            {
                if (popupDateModel is PopupDateModel _popupDateModel  && _popupDateModel is not null )
                {
                    FromDate = new DateTime(_popupDateModel.From);
                    ToDate = new DateTime(_popupDateModel.To);
                }
            }
        }

        DateTime fromDate;
        public DateTime FromDate
        {
            get => fromDate;
            set
            {
                if (SetProperty(ref fromDate, value, nameof(FromDate)))
                {
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
                if (SetProperty(ref toDate, value, nameof(ToDate)))
                {
                    to = ToDate.AddHours(23).Ticks;
                }
            }
        }
        private ObservableCollection<string> rangeFast;
        public ObservableCollection<string> RangeFast
        {
            get => rangeFast;
            set
            {
                if (SetProperty(ref rangeFast, value, nameof(RangeFast))) { }
            }
        }

        private ObservableCollection<string> rangeMonth;
        public ObservableCollection<string> RangeMonth
        {
            get => rangeMonth;
            set
            {
                if (SetProperty(ref rangeMonth, value, nameof(RangeMonth))) { }
            }
        }

        string isSelectedDateFast;
        public string IsSelectedDateFast
        {
            get => isSelectedDateFast;
            set
            {
                if (SetProperty(ref isSelectedDateFast, value, nameof(IsSelectedDateFast)))
                {
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
                if (SetProperty(ref isSelectedDateMonth, value, nameof(IsSelectedDateMonth)))
                {
                    if (!string.IsNullOrWhiteSpace(IsSelectedDateMonth))
                    {
                        SelectedDate(IsSelectedDateMonth);
                        IsSelectedDateFast = string.Empty;
                    }
                }
            }
        }

        private ObservableCollection<PopupSelectRangeDate.PopupSelectRangeDateM> selectRangeDateMs;
        public ObservableCollection<PopupSelectRangeDate.PopupSelectRangeDateM> SelectRangeDateMs
        {
            get => selectRangeDateMs;
            set
            {
                if (SetProperty(ref selectRangeDateMs, value, nameof(SelectRangeDateMs))) { }
            }
        }


        long from = 0;
        long to = 0;

        private readonly IPopupService _popupService;

        public PopupSelectRangeDateVM(IPopupService popupService)
        {
            Init();
            _popupService = popupService;
        }

        private void Init()
        {
            RangeFast ??=
            [
                "Poprzedni tydzień",
                "Dzisiaj",
                "Tydzień",
                "Miesiąc",
                "Rok",
                "Cały zakres",
            ];
            RangeMonth ??=
            [
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
            ];
            FromDate = DateTime.Today.AddDays(-1);
            ToDate = DateTime.Today.AddDays(1);

            SelectRangeDateMs ??= [];

            var users = Shared.Pages.UserDisplay.PopupUser.UserDisplayVPopup.Users.Select(x => x.Value);

            foreach (var item in users)
            {
                SelectRangeDateMs.Add(new PopupSelectRangeDateM(item));
            }
        }

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
                case "Cały zakres":
                    result = FullRange();
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

            var from = startOfWeek.AddDays(-7).AddHours(-2).ToUniversalTime();
            var to = endOfWeek.AddDays(-7).AddHours(-2).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }
        static (long, long) SelectToday()
        {
            var now = DateTime.Today;
            var from = new DateTime(now.Ticks).AddHours(2).ToUniversalTime();
            var to = new DateTime(now.AddHours(25).Ticks).ToUniversalTime();

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

            var from = startOfWeek.AddHours(2).ToUniversalTime();
            var to = endOfWeek.AddHours(2).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }
        static (long, long) SelectYear()
        {
            var from = new DateTime(DateTime.Today.Year, 1, 1, 1, 0, 0).ToUniversalTime();
            var to = new DateTime(DateTime.Today.AddYears(1).Year, 1, 1, 0, 0, 0).ToUniversalTime();

            return (from.Ticks, to.Ticks);
        }
        static (long, long) FullRange()
        {
            return (0, DateTime.Today.Ticks);
        }
        static (long, long) SelectMonth(int month = 0)
        {
            DateTime to;
            var from = new DateTime(DateTime.Today.Year, month, 1, 0, 0, 0);
            if (month == 1)
                to = new DateTime(DateTime.Today.Year, 2, 1, 0, 0, 0);
            else if (month == 12)
                to = new DateTime(DateTime.Today.Year + 1, 1, 1, 0, 0, 0);
            else
                to = new DateTime(DateTime.Today.Year, month + 1, 1, 0, 0, 0);

            return (from.Ticks, to.Ticks);
        }


        [RelayCommand]
        async Task SaveAndReturn()
        {
            var guids = new Guid[SelectRangeDateMs.Count(x => x.IsChecked)];
            int n = 0;
            for (int i = 0; i < SelectRangeDateMs.Count; i++)
            {
                if (SelectRangeDateMs[i].IsChecked)
                {
                    guids[n] = new Guid(SelectRangeDateMs[i].Driver.Id.ToByteArray());
                    n++;
                }
            }
            await _popupService.ClosePopupAsync(page: Shell.Current, result: new PopupDateModel(from, to, guids));
        }
        [RelayCommand]
        async Task CancelAndReturn()
        {
            await _popupService.ClosePopupAsync(page: Shell.Current);
        }


    }
}
