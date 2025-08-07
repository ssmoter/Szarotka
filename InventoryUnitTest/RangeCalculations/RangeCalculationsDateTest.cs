using DataBase.Model.EntitiesInventory;

using Inventory.Pages.RangeDay;

namespace InventoryUnitTest.RangeCalculations
{
    public class RangeCalculationsDateTest
    {
        private readonly IList<DayExpanded> dayExpanded;

        public RangeCalculationsDateTest()
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
                    Number = 2,
                    NumberReturn = 1,
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
            var result = Inventory.Helper.Calculations.DayRangeCalculation.GetWeek(dayExpanded);

            Assert.Equal(106, result.Count());
        }
        [Fact]
        public void Month_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculation.GetMonth(dayExpanded);

            Assert.Equal(24, result.Count());
        }

        [Fact]
        public void Year_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculation.GetYear(dayExpanded);

            Assert.Equal(2, result.Count());
        }
        [Fact]
        public void DayOfWeek_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculation.GetDayOfWeek(dayExpanded);

            Assert.Equal(14, result.Count());
        }
        [Fact]
        public void All_Should_Calculate()
        {
            var result = Inventory.Helper.Calculations.DayRangeCalculation.GetUsers(dayExpanded);

            Assert.Equal(2, result.Count());
        }
    }
}
