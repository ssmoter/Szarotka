using DataBase.Model.EntitiesInventory;

using Inventory.Pages.RangeDay;


namespace Inventory.Helper.Calculations
{
    public static class DayRangeCalculationMedian
    {
        public static IEnumerable<DayExpanded> Week(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetWeek(source);

            var result = Median(summary);

            return result;
        }
        public static IEnumerable<DayExpanded> Month(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetMonth(source);

            var result = Median(summary);

            return result;
        }
        public static IEnumerable<DayExpanded> Year(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetYear(source);


            var result = Median(summary);

            return result;
        }
        public static IEnumerable<DayExpanded> DayOfWeek(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetDayOfWeek(source);

            var result = Median(summary);

            return result;
        }
        public static IEnumerable<DayExpanded> All(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetUsers(source);

            var result = Median(summary);

            return result;
        }
        private static IEnumerable<DayExpanded> Median(IEnumerable<DayRangeCalculation.GroupByDay> source)
        {
            var result = source.Select((x, index) => new DayExpanded()
            {
                Index = index+1,
                SelectedHeaders = [.. x.Days.FirstOrDefault().SelectedHeaders, "Zakres"],
                SelectedValue = $"{x.Days.FirstOrDefault().Day.SelectedDateString}-{x.Days.Last().Day.SelectedDateString}",
                Day = new Day(x.Days.FirstOrDefault().Day)
                {
                    TotalPriceAfterCorrectDecimal = x.Days.MedianSpan(x => x.Day.TotalPriceAfterCorrectDecimal),
                    TotalPriceCorrectDecimal = x.Days.MedianSpan(x => x.Day.TotalPriceCorrectDecimal),
                    TotalPriceCakeDecimal = x.Days.MedianSpan(x => x.Day.TotalPriceCakeDecimal),
                    TotalPriceDecimal = x.Days.MedianSpan(x => x.Day.TotalPriceDecimal),
                    TotalPriceDifferenceDecimal = x.Days.MedianSpan(x => x.Day.TotalPriceDifferenceDecimal),
                    TotalPriceMoneyDecimal = x.Days.MedianSpan(x => x.Day.TotalPriceMoneyDecimal),
                    TotalPriceProductsDecimal = x.Days.MedianSpan(x => x.Day.TotalPriceProductsDecimal),
                    Products = [..x.Days.
                            SelectMany(z => z.Day.Products)
                            .GroupBy(y => y.ProductNameId)
                            .Select(k => new Product(k.FirstOrDefault())
                            {
                                Number = (int)k.MedianSpan(s=>s.Number),
                                NumberEdit = (int)k.MedianSpan(s=>s.NumberEdit),
                                NumberReturn = (int)k.MedianSpan(s=>s.NumberReturn),
                                PriceTotalAfterCorrectDecimal=k.MedianSpan(s=>s.PriceTotalAfterCorrectDecimal),
                                PriceTotalCorrectDecimal=k.MedianSpan(s=>s.PriceTotalCorrectDecimal),
                                PriceTotalDecimal=k.MedianSpan(s=>s.PriceTotalDecimal),
                            })]
                }
            });

            return result;
        }
    }
    public static class LinqExtensions
    {
        public static decimal MedianSpan<T>(this IEnumerable<T> source, Func<T, decimal> selector)
        {
            decimal[] array = [.. source.Select(selector).OrderBy(n => n)];
            var span = new Span<decimal>(array);
            span.Sort();

            int count = span.Length;
            if (count == 0) return 0;

            return count % 2 == 0
                ? (span[count / 2 - 1] + span[count / 2]) / 2
                : span[count / 2];
        }
        public static double? Median<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            var data = source.Select(selector).OrderBy(n => n).ToArray();
            int count = data.Length;

            if (count == 0)
                return null;

            if (count % 2 == 0)
            {
                // parzysta liczba elementów
                return (data[count / 2 - 1] + data[count / 2]) / 2.0;
            }
            else
            {
                // nieparzysta liczba elementów
                return data[count / 2];
            }
        }
    }
}
