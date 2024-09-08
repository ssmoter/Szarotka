using DataBase.Helper;
using DataBase.Model.EntitiesInventory;

using FluentAssertions;

using Inventory.Pages.RangeDay;

using System.Globalization;

namespace InventoryUnitTest.RangeCalculations
{
    public class RangeCalculationsDateTest
    {
        readonly static Guid Guid = new("9bcaf77f-46e4-4479-aca2-df1acd702a2c");
        readonly static Driver Driver = new()
        {
            Id = Guid
        };
        private static List<RangeDayM> GetDays()
        {
            var Monday = 638396640000000000;
            var OneDay = 638397504000000000 - Monday;
            List<RangeDayM> DayMs = [];
            DayMs.Add(new RangeDayM(new Day() { SelectedDate = new DateTime(Monday).AddHours(1), TotalPrice = 2 }, Driver));
            for (int i = 1; i < 366; i++)
            {
                DayMs.Add(new RangeDayM(new Day()
                {
                    SelectedDate = new DateTime(Monday + (OneDay * i)).AddHours(1),
                    TotalPrice = 2,
                }, Driver));
            }

            Inventory.Helper.RangeCalculations.GetUniqueDriver(DayMs);

            return DayMs;
        }
        private static string GetPerWeek(DateTime first, DateTime last)
        {
            return $"{first.ToShortDateString()} {first.DayOfWeek.TranslateSelectedDay()} {last.ToShortDateString()} {last.DayOfWeek.TranslateSelectedDay()}";

        }
        private static string GetMonth(int month, DateTime first, DateTime last)
        {
            return $"{CultureInfo.GetCultureInfo("pl-PL").DateTimeFormat.GetMonthName(month)}.{2024}{Environment.NewLine}({last:dd.MM}-{first:dd.MM})";
        }

        [Fact]
        public void SumTotalAsList()
        {
            var request = GetDays();

            var obj = Inventory.Helper.RangeCalculations.SumTotalOfRangeCalculateAverages(request);

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            int totalPrice = 0;
            for (int i = 0; i < 366; i++)
            {
                totalPrice += 2;
            }

            RangeDayM[] result = [new RangeDayM(driver:Driver,day:
                new Day()
                {
                    TotalPrice=totalPrice,
                    DriverGuid= Driver.Id,
                })];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);


            objJson.Should().Be(resultJson);
        }
        [Fact]
        public void SumTotalAsIEnumerable()
        {
            var request = GetDays();

            var obj = Inventory.Helper.RangeCalculations.SumTotalOfRangeCalculateAverages(request.Take(request.Count));

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            int totalPrice = 0;
            for (int i = 0; i < 366; i++)
            {
                totalPrice += 2;
            }

            RangeDayM[] result = [new RangeDayM(driver:Driver,day:
                new Day()
                {
                    TotalPrice=totalPrice,
                    DriverGuid= Driver.Id,
                })];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);


            objJson.Should().Be(resultJson);
        }
        [Fact]
        public void AveragesTotalAsList()
        {
            var request = GetDays();

            var obj = Inventory.Helper.RangeCalculations.SumTotalOfRangeCalculateAverages(request, true);

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            int totalPrice = 0;
            for (int i = 0; i < 366; i++)
            {
                totalPrice += 2;
            }

            RangeDayM[] result = [new RangeDayM(driver:Driver,day:
                new Day()
                {
                    TotalPrice=totalPrice/366,
                    DriverGuid= Driver.Id,
                })];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);


            objJson.Should().Be(resultJson);
        }
        [Fact]
        public void AveragesTotalAsIEnumerable()
        {
            var request = GetDays();

            var obj = Inventory.Helper.RangeCalculations.SumTotalOfRangeCalculateAverages(request.Take(request.Count), true);

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            int totalPrice = 0;
            for (int i = 0; i < 366; i++)
            {
                totalPrice += 2;
            }

            RangeDayM[] result = [new RangeDayM(driver:Driver,day:
                new Day()
                {
                    TotalPrice=totalPrice/366,
                    DriverGuid= Driver.Id,
                })];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);


            objJson.Should().Be(resultJson);
        }
        [Fact]
        public void SumDayOfWeek()
        {
            var request = GetDays();

            var obj = Inventory.Helper.RangeCalculations.SumDayOfWeek(request);

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            RangeDayM[] result =
            [
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 52*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)0).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 53*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)1).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 53*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)2).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 52*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)3).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 52*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)4).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 52*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)5).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 52*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)6).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
            ];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);
        }
        [Fact]
        public void AveragesDayOfWeek()
        {
            var request = GetDays();

            var obj = Inventory.Helper.RangeCalculations.AveragesDayOfWeek(request);

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            RangeDayM[] result =
            [
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)0).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)1).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)2).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)3).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)4).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)5).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = ((DayOfWeek)6).TranslateSelectedDay()
                    },
                    Driver=Driver,
                },
            ];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);
        }

        [Fact]
        public void SumPerOfMonth()
        {
            var request = GetDays();

            var obj = Inventory.Helper.RangeCalculations.SumPerOfMonth(request).Reverse();

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            RangeDayM[] result =
               [
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 31*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(1,new DateTime(2024,1,1),new DateTime(2024,1,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 29*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(2,new DateTime(2024,2,1),new DateTime(2024,2,29))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 31*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(3,new DateTime(2024,3,1),new DateTime(2024,3,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 30*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(4,new DateTime(2024,4,1),new DateTime(2024,4,30))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 31*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(5,new DateTime(2024,5,1),new DateTime(2024,5,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 30*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(6,new DateTime(2024,6,1),new DateTime(2024,6,30))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 31*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(7,new DateTime(2024,7,1),new DateTime(2024,7,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 31*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(8,new DateTime(2024,8,1),new DateTime(2024,8,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 30*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(9,new DateTime(2024,9,1),new DateTime(2024,9,30))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 31*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(10,new DateTime(2024,10,1),new DateTime(2024,10,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 30*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(11,new DateTime(2024,11,1),new DateTime(2024,11,30))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 31*2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(12,new DateTime(2024,12,1),new DateTime(2024,12,31))
                    },
                    Driver=Driver,
                },
            ];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);
        }

        [Fact]
        public void AveragesPerOfMonth()
        {
            var request = GetDays();

            var obj = Inventory.Helper.RangeCalculations.AveragesPerOfMonth(request).Reverse();

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            RangeDayM[] result =
               [
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(1,new DateTime(2024,1,1),new DateTime(2024,1,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(2,new DateTime(2024,2,1),new DateTime(2024,2,29))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(3,new DateTime(2024,3,1),new DateTime(2024,3,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(4,new DateTime(2024,4,1),new DateTime(2024,4,30))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(5,new DateTime(2024,5,1),new DateTime(2024,5,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(6,new DateTime(2024,6,1),new DateTime(2024,6,30))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(7,new DateTime(2024,7,1),new DateTime(2024,7,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(8,new DateTime(2024,8,1),new DateTime(2024,8,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(9,new DateTime(2024,9,1),new DateTime(2024,9,30))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(10,new DateTime(2024,10,1),new DateTime(2024,10,31))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(11,new DateTime(2024,11,1),new DateTime(2024,11,30))
                    },
                    Driver=Driver,
                },
                new()
                {
                    Day = new Day()
                    {
                        TotalPrice = 2,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetMonth(12,new DateTime(2024,12,1),new DateTime(2024,12,31))
                    },
                    Driver=Driver,
                },
            ];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);
        }


        [Fact]
        public void SumPerOfWeek()
        {
            var request = GetDays();
            request.Reverse();
            request = request.Take(31).ToList();
            var obj = Inventory.Helper.RangeCalculations.SumPerOfWeek(request);

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            RangeDayM[] result =
                [
                 new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 * 3,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,31), new DateTime(2024, 12, 29))
                    },
                    Driver = Driver,
                  },
                  new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 * 7,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,28), new DateTime(2024, 12, 22))
                    },
                    Driver = Driver,
                  },
                  new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 * 7,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,21), new DateTime(2024, 12, 15))
                    },
                    Driver = Driver,
                  },
                  new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 * 7,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,14), new DateTime(2024, 12, 8))
                    },
                    Driver = Driver,
                  },
                  new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 * 7,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,7), new DateTime(2024, 12, 1))
                    },
                    Driver = Driver,
                  },
                ];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);
        }

        [Fact]
        public void AveragesPerOfWeek()
        {
            var request = GetDays();
            request.Reverse();
            request = request.Take(31).ToList();
            var obj = Inventory.Helper.RangeCalculations.AveragesPerOfWeek(request);

            var objJson = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            RangeDayM[] result =
                [
                 new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 ,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,31), new DateTime(2024, 12, 29))
                    },
                    Driver = Driver,
                  },
                  new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 ,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,28), new DateTime(2024, 12, 22))
                    },
                    Driver = Driver,
                  },
                  new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 ,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,21), new DateTime(2024, 12, 15))
                    },
                    Driver = Driver,
                  },
                  new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 ,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,14), new DateTime(2024, 12, 8))
                    },
                    Driver = Driver,
                  },
                  new()
                  {
                    Day = new Day()
                    {
                        TotalPrice = 2 ,
                        DriverGuid = Driver.Id,
                        SelectedDateString = GetPerWeek(new DateTime(2024,12,7), new DateTime(2024, 12, 1))
                    },
                    Driver = Driver,
                  },
                ];

            var resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(result);

            objJson.Should().Be(resultJson);
        }
    }
}
