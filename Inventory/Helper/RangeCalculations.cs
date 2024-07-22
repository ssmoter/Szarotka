﻿using DataBase.Helper;
using DataBase.Model.EntitiesInventory;

using Inventory.Pages.RangeDay;

using System.Globalization;

namespace Inventory.Helper;

public class RangeCalculations
{
    public static List<DataBase.Model.EntitiesInventory.Driver> UniqueDriver { get; set; } = [];

    public static IList<RangeDayM> SumTotalOfRangeCalculateAverages(IList<RangeDayM> value, bool CalculateAverages = false)
    {
        RangeDayM[] SumRange = new RangeDayM[UniqueDriver.Count];
        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            var day = new Day();
            if (CalculateAverages)
            {
                var count = singleDriver.Count();
                if (count == 0)
                {
                    count = 1;
                }

                day = new Day
                {
                    TotalPriceProducts = singleDriver.Sum(x => x.Day.TotalPriceProducts) / count,
                    TotalPriceCake = singleDriver.Sum(x => x.Day.TotalPriceCake) / count,
                    TotalPrice = singleDriver.Sum(x => x.Day.TotalPrice) / count,
                    TotalPriceCorrect = singleDriver.Sum(x => x.Day.TotalPriceCorrect) / count,
                    TotalPriceMoney = singleDriver.Sum(x => x.Day.TotalPriceMoney) / count,
                    TotalPriceDifference = singleDriver.Sum(x => x.Day.TotalPriceDifference) / count,
                    TotalPriceAfterCorrect = singleDriver.Sum(x => x.Day.TotalPriceAfterCorrect) / count,
                    Products = new(
                           singleDriver
                             .SelectMany(m => m.Day.Products)
                             .GroupBy(p => p.Name.Id)
                             .Select(g => new Product()
                             {
                                 Name = g.FirstOrDefault().Name,
                                 ProductNameId = g.FirstOrDefault().Name.Id,
                                 PriceTotalDecimal = g.Sum(p => p.PriceTotalDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 PriceTotalCorrectDecimal = g.Sum(p => p.PriceTotalCorrectDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 PriceTotalAfterCorrectDecimal = g.Sum(p => p.PriceTotalAfterCorrectDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),

                                 Number = g.Sum(p => p.Number) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 NumberEdit = g.Sum(p => p.NumberEdit) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 NumberReturn = g.Sum(p => p.NumberReturn) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                             }))
                };
            }
            else
            {
                day = new Day
                {
                    TotalPriceProducts = singleDriver.Sum(x => x.Day.TotalPriceProducts),
                    TotalPriceCake = singleDriver.Sum(x => x.Day.TotalPriceCake),
                    TotalPrice = singleDriver.Sum(x => x.Day.TotalPrice),
                    TotalPriceCorrect = singleDriver.Sum(x => x.Day.TotalPriceCorrect),
                    TotalPriceMoney = singleDriver.Sum(x => x.Day.TotalPriceMoney),
                    TotalPriceDifference = singleDriver.Sum(x => x.Day.TotalPriceDifference),
                    TotalPriceAfterCorrect = singleDriver.Sum(x => x.Day.TotalPriceAfterCorrect),
                    Products = new(
                            singleDriver
                                .SelectMany(m => m.Day.Products)
                                .GroupBy(p => p.Name.Id)
                                .Select(g => new Product()
                                {
                                    Name = g.FirstOrDefault().Name,
                                    ProductNameId = g.FirstOrDefault().Name.Id,
                                    PriceTotalDecimal = g.Sum(p => p.PriceTotalDecimal),
                                    PriceTotalCorrectDecimal = g.Sum(p => p.PriceTotalCorrectDecimal),
                                    PriceTotalAfterCorrectDecimal = g.Sum(p => p.PriceTotalAfterCorrectDecimal),

                                    Number = g.Sum(p => p.Number),
                                    NumberEdit = g.Sum(p => p.NumberEdit),
                                    NumberReturn = g.Sum(p => p.NumberReturn)
                                }))
                };
            }

            day.DriverGuid = UniqueDriver[i].Id;

            SumRange[i] = new()
            {
                Driver = UniqueDriver[i],
                Day = day,
            };
        }
        return SumRange;

        static int GetCoutWhereXIsMoreThan0(int count)
        {
            return count > 0 ? count : 1;
        }
    }
    public static IList<RangeDayM> SumTotalOfRangeCalculateAverages(IEnumerable<RangeDayM> value, bool CalculateAverages = false)
    {
        RangeDayM[] SumRange = new RangeDayM[UniqueDriver.Count];
        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            var day = new Day();
            if (CalculateAverages)
            {
                var count = singleDriver.Count();
                if (count == 0)
                {
                    count = 1;
                }

                day = new Day
                {
                    TotalPriceProducts = singleDriver.Sum(x => x.Day.TotalPriceProducts) / count,
                    TotalPriceCake = singleDriver.Sum(x => x.Day.TotalPriceCake) / count,
                    TotalPrice = singleDriver.Sum(x => x.Day.TotalPrice) / count,
                    TotalPriceCorrect = singleDriver.Sum(x => x.Day.TotalPriceCorrect) / count,
                    TotalPriceMoney = singleDriver.Sum(x => x.Day.TotalPriceMoney) / count,
                    TotalPriceDifference = singleDriver.Sum(x => x.Day.TotalPriceDifference) / count,
                    TotalPriceAfterCorrect = singleDriver.Sum(x => x.Day.TotalPriceAfterCorrect) / count,
                    Products = new(
                           singleDriver
                             .SelectMany(m => m.Day.Products)
                             .GroupBy(p => p.Name.Id)
                             .Select(g => new Product()
                             {
                                 Name = g.FirstOrDefault().Name,
                                 ProductNameId = g.FirstOrDefault().Name.Id,
                                 PriceTotalDecimal = g.Sum(p => p.PriceTotalDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 PriceTotalCorrectDecimal = g.Sum(p => p.PriceTotalCorrectDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 PriceTotalAfterCorrectDecimal = g.Sum(p => p.PriceTotalAfterCorrectDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),

                                 Number = g.Sum(p => p.Number) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 NumberEdit = g.Sum(p => p.NumberEdit) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 NumberReturn = g.Sum(p => p.NumberReturn) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                             }))
                };
            }
            else
            {
                day = new Day
                {
                    TotalPriceProducts = singleDriver.Sum(x => x.Day.TotalPriceProducts),
                    TotalPriceCake = singleDriver.Sum(x => x.Day.TotalPriceCake),
                    TotalPrice = singleDriver.Sum(x => x.Day.TotalPrice),
                    TotalPriceCorrect = singleDriver.Sum(x => x.Day.TotalPriceCorrect),
                    TotalPriceMoney = singleDriver.Sum(x => x.Day.TotalPriceMoney),
                    TotalPriceDifference = singleDriver.Sum(x => x.Day.TotalPriceDifference),
                    TotalPriceAfterCorrect = singleDriver.Sum(x => x.Day.TotalPriceAfterCorrect),
                    Products = new(
                            singleDriver
                                .SelectMany(m => m.Day.Products)
                                .GroupBy(p => p.Name.Id)
                                .Select(g => new Product()
                                {
                                    Name = g.FirstOrDefault().Name,
                                    ProductNameId = g.FirstOrDefault().Name.Id,
                                    PriceTotalDecimal = g.Sum(p => p.PriceTotalDecimal),
                                    PriceTotalCorrectDecimal = g.Sum(p => p.PriceTotalCorrectDecimal),
                                    PriceTotalAfterCorrectDecimal = g.Sum(p => p.PriceTotalAfterCorrectDecimal),

                                    Number = g.Sum(p => p.Number),
                                    NumberEdit = g.Sum(p => p.NumberEdit),
                                    NumberReturn = g.Sum(p => p.NumberReturn)
                                }))
                };
            }

            day.DriverGuid = UniqueDriver[i].Id;

            SumRange[i] = new()
            {
                Driver = UniqueDriver[i],
                Day = day,
            };
        }
        return SumRange;

        static int GetCoutWhereXIsMoreThan0(int count)
        {
            return count > 0 ? count : 1;
        }
    }
    public static IEnumerable<RangeDayM> SumTotalOfRangeCalculateAveragesIEnumerable(IEnumerable<RangeDayM> value, bool CalculateAverages = false)
    {
        RangeDayM[] SumRange = new RangeDayM[UniqueDriver.Count];
        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            var day = new Day();
            if (CalculateAverages)
            {
                var count = singleDriver.Count();
                if (count == 0)
                {
                    count = 1;
                }

                day = new Day
                {
                    TotalPriceProducts = singleDriver.Sum(x => x.Day.TotalPriceProducts) / count,
                    TotalPriceCake = singleDriver.Sum(x => x.Day.TotalPriceCake) / count,
                    TotalPrice = singleDriver.Sum(x => x.Day.TotalPrice) / count,
                    TotalPriceCorrect = singleDriver.Sum(x => x.Day.TotalPriceCorrect) / count,
                    TotalPriceMoney = singleDriver.Sum(x => x.Day.TotalPriceMoney) / count,
                    TotalPriceDifference = singleDriver.Sum(x => x.Day.TotalPriceDifference) / count,
                    TotalPriceAfterCorrect = singleDriver.Sum(x => x.Day.TotalPriceAfterCorrect) / count,
                    Products = new(
                           singleDriver
                             .SelectMany(m => m.Day.Products)
                             .GroupBy(p => p.Name.Id)
                             .Select(g => new Product()
                             {
                                 Name = g.FirstOrDefault().Name,
                                 ProductNameId = g.FirstOrDefault().Name.Id,
                                 PriceTotalDecimal = g.Sum(p => p.PriceTotalDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 PriceTotalCorrectDecimal = g.Sum(p => p.PriceTotalCorrectDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 PriceTotalAfterCorrectDecimal = g.Sum(p => p.PriceTotalAfterCorrectDecimal) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),

                                 Number = g.Sum(p => p.Number) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 NumberEdit = g.Sum(p => p.NumberEdit) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                                 NumberReturn = g.Sum(p => p.NumberReturn) / GetCoutWhereXIsMoreThan0(g.Count(p => (p.Number + p.NumberEdit) > 0)),
                             }))
                };
            }
            else
            {
                day = new Day
                {
                    TotalPriceProducts = singleDriver.Sum(x => x.Day.TotalPriceProducts),
                    TotalPriceCake = singleDriver.Sum(x => x.Day.TotalPriceCake),
                    TotalPrice = singleDriver.Sum(x => x.Day.TotalPrice),
                    TotalPriceCorrect = singleDriver.Sum(x => x.Day.TotalPriceCorrect),
                    TotalPriceMoney = singleDriver.Sum(x => x.Day.TotalPriceMoney),
                    TotalPriceDifference = singleDriver.Sum(x => x.Day.TotalPriceDifference),
                    TotalPriceAfterCorrect = singleDriver.Sum(x => x.Day.TotalPriceAfterCorrect),
                    Products = new(
                            singleDriver
                                .SelectMany(m => m.Day.Products)
                                .GroupBy(p => p.Name.Id)
                                .Select(g => new Product()
                                {
                                    Name = g.FirstOrDefault().Name,
                                    ProductNameId = g.FirstOrDefault().Name.Id,
                                    PriceTotalDecimal = g.Sum(p => p.PriceTotalDecimal),
                                    PriceTotalCorrectDecimal = g.Sum(p => p.PriceTotalCorrectDecimal),
                                    PriceTotalAfterCorrectDecimal = g.Sum(p => p.PriceTotalAfterCorrectDecimal),

                                    Number = g.Sum(p => p.Number),
                                    NumberEdit = g.Sum(p => p.NumberEdit),
                                    NumberReturn = g.Sum(p => p.NumberReturn)
                                }))
                };
            }

            day.DriverGuid = UniqueDriver[i].Id;

            SumRange[i] = new()
            {
                Driver = UniqueDriver[i],
                Day = day,
            };
        }
        return SumRange;

        static int GetCoutWhereXIsMoreThan0(int count)
        {
            return count > 0 ? count : 1;
        }
    }




    public static void GetUniqueDriver(IList<RangeDayM> value)
    {
        UniqueDriver = value.DistinctBy(x => x.Driver.Id).Select(z => z.Driver).ToList();
    }

    public static IList<RangeDayM> SumDayOfWeek(IList<RangeDayM> value)
    {
        List<RangeDayM> SumRange = [];

        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            for (int j = 1; j < 7; j++)
            {
                var dayOfWeek = singleDriver.Where(x => x.Day.SelectedDate.DayOfWeek == (DayOfWeek)j);

                IList<RangeDayM> sum = [];
                if (j == 1)
                {
                    dayOfWeek = dayOfWeek.Concat(singleDriver.Where(x => x.Day.SelectedDate.DayOfWeek == (DayOfWeek)0));
                    sum = SumTotalOfRangeCalculateAverages(dayOfWeek);
                }
                else
                {
                    sum = SumTotalOfRangeCalculateAverages(dayOfWeek);
                }

                for (int k = 0; k < sum.Count; k++)
                {
                    sum[k].Day.SelectedDateString = ((DayOfWeek)j).TranslateSelectedDay();
                }
                if (sum.Count > 0)
                {
                    SumRange.AddRange(sum);
                }
            }
        }

        return SumRange;
    }

    /// <summary>
    /// średnia
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IList<RangeDayM> AveragesDayOfWeek(IList<RangeDayM> value)
    {
        List<RangeDayM> SumRange = [];

        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            for (int j = 1; j < 7; j++)
            {
                var dayOfWeek = singleDriver.Where(x => x.Day.SelectedDate.DayOfWeek == (DayOfWeek)j);

                IList<RangeDayM> sum = [];
                if (j == 1)
                {
                    dayOfWeek = [.. dayOfWeek, .. singleDriver.Where(x => x.Day.SelectedDate.DayOfWeek == (DayOfWeek)0)];
                    sum = SumTotalOfRangeCalculateAverages(dayOfWeek, true);
                }
                else
                {
                    sum = SumTotalOfRangeCalculateAverages(dayOfWeek, true);
                }

                for (int k = 0; k < sum.Count; k++)
                {
                    sum[k].Day.SelectedDateString = ((DayOfWeek)j).TranslateSelectedDay();
                }

                SumRange.AddRange(sum);
            }
        }

        return SumRange;
    }




    public static IList<RangeDayM> SumPerOfWeek(IList<RangeDayM> value)
    {
        List<RangeDayM> SumRange = [];

        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            IList<RangeDayM> sum = [];
            int firstDayOfWeek = (int)singleDriver.FirstOrDefault().Day.SelectedDate.DayOfWeek;


            foreach (var item in singleDriver)
            {
                var dayOfWeek = (int)item.Day.SelectedDate.DayOfWeek;

                if (dayOfWeek > firstDayOfWeek)
                {
                    var weekSum = SumTotalOfRangeCalculateAverages(sum);

                    for (int k = 0; k < weekSum.Count; k++)
                    {
                        weekSum[k].Day.SelectedDateString = $"{sum.FirstOrDefault().Day.SelectedDate.ToShortDateString()} {sum.FirstOrDefault().Day.SelectedDate.DayOfWeek.TranslateSelectedDay()} {sum.LastOrDefault().Day.SelectedDate.ToShortDateString()} {sum.LastOrDefault().Day.SelectedDate.DayOfWeek.TranslateSelectedDay()}";
                    }
                    if (weekSum.Count > 0)
                    {
                        SumRange.AddRange(weekSum);
                    }
                    firstDayOfWeek = dayOfWeek;
                    sum.Clear();
                }

                if (dayOfWeek <= firstDayOfWeek)
                {
                    sum.Add(item);
                    firstDayOfWeek = dayOfWeek;
                }
            }
        }

        return SumRange;
    }
    public static IList<RangeDayM> AveragesPerOfWeek(IList<RangeDayM> value)
    {
        List<RangeDayM> SumRange = [];

        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            IList<RangeDayM> sum = [];
            int firstDayOfWeek = (int)singleDriver.FirstOrDefault().Day.SelectedDate.DayOfWeek;


            foreach (var item in singleDriver)
            {
                var dayOfWeek = (int)item.Day.SelectedDate.DayOfWeek;

                if (dayOfWeek > firstDayOfWeek)
                {
                    var weekSum = SumTotalOfRangeCalculateAverages(sum, true);

                    for (int k = 0; k < weekSum.Count; k++)
                    {
                        weekSum[k].Day.SelectedDateString = $"{sum.FirstOrDefault().Day.SelectedDate.ToShortDateString()} {sum.FirstOrDefault().Day.SelectedDate.DayOfWeek.TranslateSelectedDay()} {sum.LastOrDefault().Day.SelectedDate.ToShortDateString()} {sum.LastOrDefault().Day.SelectedDate.DayOfWeek.TranslateSelectedDay()}";
                    }
                    if (weekSum.Count > 0)
                    {
                        SumRange.AddRange(weekSum);
                    }
                    firstDayOfWeek = dayOfWeek;
                    sum.Clear();
                }

                if (dayOfWeek <= firstDayOfWeek)
                {
                    sum.Add(item);
                    firstDayOfWeek = dayOfWeek;
                }

            }
        }

        return SumRange;
    }



    public static IList<RangeDayM> SumPerOfMonth(IList<RangeDayM> value)
    {
        List<RangeDayM> SumRange = [];

        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            var sorted = singleDriver.OrderBy(z => z.Day.SelectedDateTicks);
            var lastYear = sorted.LastOrDefault().Day.SelectedDate.Year;
            var firstYear = sorted.FirstOrDefault().Day.SelectedDate.Year;
            var differenceYear = lastYear - firstYear + 1;
            var curentYear = lastYear;

            for (int y = differenceYear; y > 0; y--)
            {
                for (int j = 12; j > 0; j--)
                {
                    var month = singleDriver.Where(x => x.Day.SelectedDate.Month == j && x.Day.SelectedDate.Year == curentYear);

                    if (month.FirstOrDefault() is null)
                    {
                        continue;
                    }

                    IList<RangeDayM> sum = [];

                    sum = SumTotalOfRangeCalculateAverages(month);
                    var first = month.FirstOrDefault();
                    var last = month.LastOrDefault();

                    for (int k = 0; k < sum.Count; k++)
                    {
                        sum[k].Day.SelectedDateString = $"{CultureInfo.GetCultureInfo("pl-PL").DateTimeFormat.GetMonthName(j)}.{curentYear}{Environment.NewLine}({last.Day.SelectedDate:dd.MM}-{first.Day.SelectedDate:dd.MM})";
                    }
                    if (sum.Count > 0)
                    {
                        SumRange.AddRange(sum);
                    }
                }
                curentYear--;
            }

        }

        return SumRange;
    }
    public static IList<RangeDayM> AveragesPerOfMonth(IList<RangeDayM> value)
    {
        List<RangeDayM> SumRange = [];

        for (int i = 0; i < UniqueDriver.Count; i++)
        {
            var driver = UniqueDriver[i];
            var singleDriver = value.Where(z => z.Driver.Id == driver.Id);

            var sorted = singleDriver.OrderBy(z => z.Day.SelectedDateTicks);
            var lastYear = sorted.LastOrDefault().Day.SelectedDate.Year;
            var firstYear = sorted.FirstOrDefault().Day.SelectedDate.Year;
            var differenceYear = lastYear - firstYear + 1;
            var curentYear = lastYear;

            for (int y = differenceYear; y > 0; y--)
            {
                for (int j = 12; j > 0; j--)
                {
                    var month = singleDriver.Where(x => x.Day.SelectedDate.Month == j && x.Day.SelectedDate.Year == curentYear);
                    if (month.FirstOrDefault() is null)
                    {
                        continue;
                    }


                    IList<RangeDayM> sum = [];

                    sum = SumTotalOfRangeCalculateAverages(month, true);
                    var first = month.FirstOrDefault();
                    var last = month.LastOrDefault();

                    for (int k = 0; k < sum.Count; k++)
                    {
                        sum[k].Day.SelectedDateString = $"{CultureInfo.GetCultureInfo("pl-PL").DateTimeFormat.GetMonthName(j)}.{curentYear}{Environment.NewLine}({last.Day.SelectedDate:dd.MM}-{first.Day.SelectedDate:dd.MM})";
                    }
                    if (sum.Count > 0)
                    {
                        SumRange.AddRange(sum);
                    }
                }
                curentYear--;
            }

        }

        return SumRange;
    }

}