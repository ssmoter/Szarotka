using Inventory.Pages.RangeDay;

namespace Inventory.Helper.Calculations
{
    public static class DayRangeCalculation
    {
        public class GroupByDay()
        {
            public IEnumerable<DayExpanded> Days { get; set; }
        }


        public static IEnumerable<DayRangeCalculation.GroupByDay> GetWeek(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = source
                .GroupBy(x => new
                {
                    Id = x.Day.UserCreatedId,
                    DateStart = x.Day.SelectedDate.Date.AddDays(-(int)x.Day.SelectedDate.DayOfWeek),
                })
                .Select(z => new DayRangeCalculation.GroupByDay()
                {
                    Days = z.GroupBy(x => x.Day.SelectedDate)
                    .Select(g => g.FirstOrDefault())
                });

            return summary;
        }
        public static IEnumerable<DayRangeCalculation.GroupByDay> GetMonth(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = source
                    .GroupBy(x => new
                    {
                        Id = x.Day.UserCreatedId,
                        DateStart = x.Day.SelectedDate.Month,
                    })
                    .Select(z => new DayRangeCalculation.GroupByDay()
                    {
                        Days = z.GroupBy(x => x.Day.SelectedDate)
                        .Select(g => g.FirstOrDefault())
                    });

            return summary;
        }
        public static IEnumerable<DayRangeCalculation.GroupByDay> GetYear(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = source
                .GroupBy(x => new
                {
                    Id = x.Day.UserCreatedId,
                    DateStart = x.Day.SelectedDate.Year,
                })
                .Select(z => new DayRangeCalculation.GroupByDay()
                {
                    Days = z.GroupBy(x => x.Day.SelectedDate)
                    .Select(g => g.FirstOrDefault())
                });

            return summary;
        }
        public static IEnumerable<DayRangeCalculation.GroupByDay> GetDayOfWeek(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = source
                .GroupBy(x => new
                {
                    Id = x.Day.UserCreatedId,
                    DateStart = x.Day.SelectedDate.DayOfWeek,
                })
                .Select(z => new DayRangeCalculation.GroupByDay()
                {
                    Days = z.GroupBy(x => x.Day.SelectedDate)
                    .Select(g => g.FirstOrDefault())
                });

            return summary;
        }
        public static IEnumerable<DayRangeCalculation.GroupByDay> GetUsers(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = source
                .GroupBy(x => new
                {
                    Id = x.Day.UserCreatedId,
                })
                .Select(z => new DayRangeCalculation.GroupByDay()
                {
                    Days = z.GroupBy(x => x.Day.SelectedDate)
                    .Select(g => g.FirstOrDefault())
                });

            return summary;
        }


    }
}
