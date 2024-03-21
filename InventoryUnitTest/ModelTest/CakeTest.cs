using FluentAssertions;

using DataBase.Model.EntitiesInventory;

namespace InventoryUnitTest.ModelTest
{
    public class CakeTest
    {
        [Theory]
        [InlineData(10.50, 1050)]
        public void TestParsingDecimal(decimal request, int result)
        {
            var obj = new Cake
            {
                PriceDecimal = request
            };


            obj.Price.Should().Be(result);

        }
        [Theory]
        [InlineData(10, 1000)]
        public void TestParsingDecimal2(decimal request, int result)
        {
            var obj = new Cake
            {
                PriceDecimal = request
            };


            obj.Price.Should().Be(result);

        }
        [Theory]
        [InlineData(0.9, 90)]
        public void TestParsingDecimal3(decimal request, int result)
        {
            var obj = new Cake
            {
                PriceDecimal = request
            };


            obj.Price.Should().Be(result);

        }

        [Theory]
        [InlineData(297.75, 29775)]
        public void TestParsingDecimal4(decimal request, int result)
        {
            var obj = new Cake
            {
                PriceDecimal = request
            };


            obj.Price.Should().Be(result);

        }

        [Theory]
        [InlineData(29775, 297.75)]
        public void TestParsingDecimalReverse(int request, decimal result)
        {
            var obj = new Cake
            {
                Price = request
            };


            obj.PriceDecimal.Should().Be(result);

        }

        [Theory]
        [InlineData(90, 0.9)]
        public void TestParsingDecimalReverse1(int request, decimal result)
        {
            var obj = new Cake
            {
                Price = request
            };


            obj.PriceDecimal.Should().Be(result);

        }
        [Theory]
        [InlineData(1000, 10)]
        public void TestParsingDecimalReverse2(int request, decimal result)
        {
            var obj = new Cake
            {
                Price = request
            };


            obj.PriceDecimal.Should().Be(result);

        }

        [Theory]
        [InlineData(1000, 10)]
        public void TestCreated(int request, decimal result)
        {
            var obj = new Cake
            {
                Price = request
            };


            obj.PriceDecimal.Should().Be(result);

        }

    }
}
