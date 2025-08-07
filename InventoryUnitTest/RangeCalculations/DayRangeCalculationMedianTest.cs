using DataBase.Model.EntitiesInventory;

using Inventory.Pages.RangeDay;

namespace InventoryUnitTest.RangeCalculations
{
    public class DayRangeCalculationMedianTest
    {
        private readonly IList<DayExpanded> dayExpanded;

        public DayRangeCalculationMedianTest()
        {
            dayExpanded = [];
            var nameId1 = Guid.CreateVersion7();
            var nameId2 = Guid.CreateVersion7();
            bool user = false;
            var time = new DateTime(2025, 1, 1);
            for (int i = 0; i < 365; i++)
            {
                dayExpanded.Add(new DayExpanded()
                {
                    Index = i,
                    SelectedHeaders = [],
                    Day = new Day()
                    {
                        SelectedDate = time.AddDays(i),
                        Products = [CreatedProduct(nameId1, i), CreatedProduct(nameId2, i)]
                    }
                });
                dayExpanded[i].Day.CalculatePrice();
                if (user)
                {
                    dayExpanded[i].Day.UserCreatedId = nameId1;
                }
                else
                {
                    dayExpanded[i].Day.UserCreatedId = nameId2;
                }
                user = !user;
            }
            static Product CreatedProduct(Guid id, int i)
            {
                var product = new Product()
                {
                    ProductNameId = id,
                    Number = i,
                    NumberReturn = 0,
                    Price = new ProductPrice()
                    {
                        PriceDecimal = 1
                    }
                };
                product.CalculatePrice();
                return product;
            }
        }

        [Fact]
        public void Week_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationMedian.Week(dayExpanded);

            Assert.Equal(106, result.Count());
            var first = result.FirstOrDefault();
            Assert.Equal(2, first!.Day.TotalPriceAfterCorrectDecimal);
            var second = result.Skip(1).FirstOrDefault();
            Assert.Equal(4, second!.Day.TotalPriceAfterCorrectDecimal);
        }
        [Fact]
        public void Month_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationMedian.Month(dayExpanded);

            Assert.Equal(24, result.Count());
            var first = result.FirstOrDefault();
            Assert.Equal(30, first!.Day.TotalPriceAfterCorrectDecimal);
            var second = result.Skip(1).FirstOrDefault();
            Assert.Equal(30, second!.Day.TotalPriceAfterCorrectDecimal);
        }

        [Fact]
        public void Year_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationMedian.Year(dayExpanded);

            Assert.Equal(2, result.Count());

            var first = result.FirstOrDefault();
            Assert.Equal(364, first!.Day.TotalPriceAfterCorrectDecimal);
            var second = result.Skip(1).FirstOrDefault();
            Assert.Equal(364, second!.Day.TotalPriceAfterCorrectDecimal);
        }
        [Fact]
        public void DayOfWeek_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationMedian.DayOfWeek(dayExpanded);

            var first = result.FirstOrDefault();
            Assert.Equal(364, first!.Day.TotalPriceAfterCorrectDecimal);
            var second = result.Skip(1).FirstOrDefault();
            Assert.Equal(352, second!.Day.TotalPriceAfterCorrectDecimal);
        }
        [Fact]
        public void All_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationMedian.All(dayExpanded);

            Assert.Equal(2, result.Count());
            var first = result.FirstOrDefault();
            var second = result.LastOrDefault();
            Assert.Equal(364, first!.Day.TotalPriceDecimal);
            Assert.Equal(364, second!.Day.TotalPriceDecimal);
        }
    }

}
