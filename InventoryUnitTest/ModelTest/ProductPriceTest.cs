using FluentAssertions;

using Inventory.Model;

namespace InventoryUnitTest.ModelTest
{
    public class ProductPriceTest
    {
        public static IEnumerable<object[]> TestDateTime =>
            new List<object[]>
            {
              new object []{DateTime.Now,DateTime.Now.Ticks},
              new object []{DateTime.Now.AddDays(5),DateTime.Now.AddDays(5).Ticks},
              new object []{DateTime.Now.AddDays(-3),DateTime.Now.AddDays(-3).Ticks},

            };

        [Theory]
        [MemberData(nameof(TestDateTime))]
        public void TestParsingDateTime(DateTime request, long result)
        {
            var sut = new ProductPrice
            {
                CreatedDateTime = request
            };

            sut.Created.Should().Be(result);
        }

        static readonly DateTime date = DateTime.Now;
        public static IEnumerable<object[]> TestDateTimeReverse =>
    new List<object[]>
    {
              new object []{ date.AddDays(10).Ticks, date.AddDays(10)},
              new object []{ date.AddDays(-9).Ticks, date.AddDays(-9)},
              new object []{ date.Ticks,date},

    };



        [Theory]
        [MemberData(nameof(TestDateTimeReverse))]
        public void TestParsingDateTimeReverse(long request, DateTime result)
        {
            var sut = new ProductPrice
            {
                Created = request
            };

            sut.CreatedDateTime.Should().Be(result);
        }
    }
}
