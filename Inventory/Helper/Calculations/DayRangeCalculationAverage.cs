using DataBase.Model.EntitiesInventory;

using Inventory.Pages.RangeDay;

namespace Inventory.Helper.Calculations
{
    public static class DayRangeCalculationAverage
    {

        public static IEnumerable<DayExpanded> Week(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetWeek(source);

            var result = Average(summary);

            return result;
        }
        public static IEnumerable<DayExpanded> Month(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetMonth(source);

            var result = Average(summary);

            return result;
        }
        public static IEnumerable<DayExpanded> Year(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetYear(source);


            var result = Average(summary);

            return result;
        }
        public static IEnumerable<DayExpanded> DayOfWeek(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetDayOfWeek(source);

            var result = Average(summary);

            return result;
        }
        public static IEnumerable<DayExpanded> All(IEnumerable<DayExpanded> source)
        {
            IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetUsers(source);

            var result = Average(summary);

            return result;
        }
        private static IEnumerable<DayExpanded> Average(IEnumerable<DayRangeCalculation.GroupByDay> source)
        {
            var result = source.Select((x, index) => new DayExpanded()
            {
                Index = index + 1,
                SelectedHeaders = [.. x.Days.FirstOrDefault().SelectedHeaders, "Zakres"],
                SelectedValue = $"{x.Days.FirstOrDefault().Day.SelectedDateString}-{x.Days.Last().Day.SelectedDateString}",
                Day = new Day(x.Days.FirstOrDefault().Day)
                {
                    TotalPriceAfterCorrectDecimal = x.Days.Average(x => x.Day.TotalPriceAfterCorrectDecimal),
                    TotalPriceCorrectDecimal = x.Days.Average(x => x.Day.TotalPriceCorrectDecimal),
                    TotalPriceCakeDecimal = x.Days.Average(x => x.Day.TotalPriceCakeDecimal),
                    TotalPriceDecimal = x.Days.Average(x => x.Day.TotalPriceDecimal),
                    TotalPriceDifferenceDecimal = x.Days.Average(x => x.Day.TotalPriceDifferenceDecimal),
                    TotalPriceMoneyDecimal = x.Days.Average(x => x.Day.TotalPriceMoneyDecimal),
                    TotalPriceProductsDecimal = x.Days.Average(x => x.Day.TotalPriceProductsDecimal),
                    Products = [..x.Days.
                            SelectMany(z => z.Day.Products)
                            .GroupBy(y => y.ProductNameId)
                            .Select(k => new Product(k.FirstOrDefault())
                            {
                                Number = (int)k.Average(s=>s.Number),
                                NumberEdit = (int)k.Average(s=>s.NumberEdit),
                                NumberReturn = (int)k.Average(s=>s.NumberReturn),
                                PriceTotalAfterCorrectDecimal=k.Average(s=>s.PriceTotalAfterCorrectDecimal),
                                PriceTotalCorrectDecimal=k.Average(s=>s.PriceTotalCorrectDecimal),
                                PriceTotalDecimal=k.Average(s=>s.PriceTotalDecimal),
                            })]
                }
            });

            return result;
        }

    }
}

