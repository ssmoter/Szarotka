using DataBase.Model.EntitiesInventory;

using Inventory.Pages.RangeDay;


namespace Inventory.Helper.Calculations;


public static class DayRangeCalculationSum
{
    public static IEnumerable<DayExpanded> Week(IEnumerable<DayExpanded> source)
    {
        IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetWeek(source);

        var result = Sum(summary);

        return result;
    }
    public static IEnumerable<DayExpanded> Month(IEnumerable<DayExpanded> source)
    {
        IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetMonth(source);

        var result = Sum(summary);

        return result;
    }
    public static IEnumerable<DayExpanded> Year(IEnumerable<DayExpanded> source)
    {
        IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetYear(source);


        var result = Sum(summary);

        return result;
    }
    public static IEnumerable<DayExpanded> DayOfWeek(IEnumerable<DayExpanded> source)
    {
        IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetDayOfWeek(source);

        var result = Sum(summary);

        return result;
    }
    public static IEnumerable<DayExpanded> All(IEnumerable<DayExpanded> source)
    {
        IEnumerable<DayRangeCalculation.GroupByDay> summary = DayRangeCalculation.GetUsers(source);

        var result = Sum(summary);

        return result;
    }
    private static IEnumerable<DayExpanded> Sum(IEnumerable<DayRangeCalculation.GroupByDay> source)
    {
        var result = source.Select((x, index) => new DayExpanded()
        {
            Index = index + 1,
            SelectedHeaders = [.. x.Days.FirstOrDefault().SelectedHeaders, "Zakres"],
            SelectedValue = $"{x.Days.FirstOrDefault().Day.SelectedDateString}-{x.Days.Last().Day.SelectedDateString}",
            Day = new Day(x.Days.FirstOrDefault().Day)
            {
                TotalPriceAfterCorrectDecimal = x.Days.Sum(x => x.Day.TotalPriceAfterCorrectDecimal),
                TotalPriceCorrectDecimal = x.Days.Sum(x => x.Day.TotalPriceCorrectDecimal),
                TotalPriceCakeDecimal = x.Days.Sum(x => x.Day.TotalPriceCakeDecimal),
                TotalPriceDecimal = x.Days.Sum(x => x.Day.TotalPriceDecimal),
                TotalPriceDifferenceDecimal = x.Days.Sum(x => x.Day.TotalPriceDifferenceDecimal),
                TotalPriceMoneyDecimal = x.Days.Sum(x => x.Day.TotalPriceMoneyDecimal),
                TotalPriceProductsDecimal = x.Days.Sum(x => x.Day.TotalPriceProductsDecimal),
                Products = [..x.Days.
                            SelectMany(z => z.Day.Products)
                            .GroupBy(y => y.ProductNameId)
                            .Select(k => new Product(k.FirstOrDefault())
                            {
                                Number = k.Sum(s=>s.Number),
                                NumberEdit = k.Sum(s=>s.NumberEdit),
                                NumberReturn = k.Sum(s=>s.NumberReturn),
                                PriceTotalAfterCorrectDecimal=k.Sum(s=>s.PriceTotalAfterCorrectDecimal),
                                PriceTotalCorrectDecimal=k.Sum(s=>s.PriceTotalCorrectDecimal),
                                PriceTotalDecimal=k.Sum(s=>s.PriceTotalDecimal),
                            })]
            }
        });

        return result;
    }
}