using FluentAssertions;

using Inventory.Model;

namespace InventoryUnitTest.ModelTest
{
    public class ProductPriceTest
    {
        public static IEnumerable<object[]> TestDateTime =>
            new List<object[]>
            {
              new object []{DateTime.Now,DateTime.Now.ToString()},
              new object []{DateTime.Now.AddDays(5),DateTime.Now.AddDays(5).ToString()},
              new object []{DateTime.Now.AddDays(-3),DateTime.Now.AddDays(-3).ToString()},

            };

        [Theory]
        [MemberData(nameof(TestDateTime))]
        public void TestParsingDateTime(DateTime request, string result)
        {
            var sut = new ProductPrice();

            sut.CreatedDateTime = request;

            sut.Created.Should().Be(result);
        }

        static string Format = "yyyy-MM-dd HH:mm:ss.fffffff";
        static DateTime date = DateTime.Now;
        public static IEnumerable<object[]> TestDateTimeReverse =>
    new List<object[]>
    {
              new object []{ date.AddDays(10).ToString(Format), date.AddDays(10)},
              new object []{ date.AddDays(-9).ToString(Format), date.AddDays(-9)},
              new object []{ date.ToString(Format),date},

    };



        [Theory]
        [MemberData(nameof(TestDateTimeReverse))]
        public void TestParsingDateTimeReverse(string request, DateTime result)
        {
            var sut = new ProductPrice();

            sut.Created = request;

            sut.CreatedDateTime.Should().Be(result);
        }
    }
}
