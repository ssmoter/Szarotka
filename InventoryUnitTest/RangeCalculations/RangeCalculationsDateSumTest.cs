using DataBase.Model.EntitiesInventory;

using Inventory.Pages.RangeDay;

namespace InventoryUnitTest.RangeCalculations
{
    public class RangeCalculationsDateSumTest
    {
        private readonly IList<DayExpanded> dayExpanded;

        public RangeCalculationsDateSumTest()
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
                        Products = [CreatedProduct(nameId1), CreatedProduct(nameId2)]
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
            static Product CreatedProduct(Guid id)
            {
                var product = new Product()
                {
                    ProductNameId = id,
                    Number = 1,
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
            var result = Inventory.Helper.Calculations.DayRangeCalculationSum.Week(dayExpanded);

            Assert.Equal(106, result.Count());
            var first = result.FirstOrDefault();
            Assert.Equal(4, first!.Day.TotalPriceAfterCorrectDecimal);
            var second = result.Skip(1).FirstOrDefault();
            Assert.Equal(4, second!.Day.TotalPriceAfterCorrectDecimal);
        }
        [Fact]
        public void Month_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationSum.Month(dayExpanded);

            Assert.Equal(24, result.Count());
            var first = result.FirstOrDefault();
            Assert.Equal(32, first!.Day.TotalPriceAfterCorrectDecimal);
            var second = result.Skip(1).FirstOrDefault();
            Assert.Equal(30, second!.Day.TotalPriceAfterCorrectDecimal);
        }

        [Fact]
        public void Year_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationSum.Year(dayExpanded);

            Assert.Equal(2, result.Count());

            var first = result.FirstOrDefault();
            Assert.Equal(366, first!.Day.TotalPriceAfterCorrectDecimal);
            var second = result.Skip(1).FirstOrDefault();
            Assert.Equal(364, second!.Day.TotalPriceAfterCorrectDecimal);
        }
        [Fact]
        public void DayOfWeek_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationSum.DayOfWeek(dayExpanded);

            var first = result.FirstOrDefault();
            Assert.Equal(54, first!.Day.TotalPriceAfterCorrectDecimal);
            var second = result.Skip(1).FirstOrDefault();
            Assert.Equal(52, second!.Day.TotalPriceAfterCorrectDecimal);
        }
        [Fact]
        public void All_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculationSum.All(dayExpanded);

            Assert.Equal(2, result.Count());
            var first = result.FirstOrDefault();
            var second = result.LastOrDefault();
            Assert.Equal(366, first!.Day.TotalPriceDecimal);
            Assert.Equal(364, second!.Day.TotalPriceDecimal);
        }
    }

}
