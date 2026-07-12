using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTProductNameAndPriceTest
    {
        [Fact]
        public async Task GetEmptyProductsNameAndPrices_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetEmptyProductsNameAndPrices_ShouldGet));
            var expected = await Helper.SetExampleDays(1, db);


            var aot = new GetInventoryAoT(db);

            var result = await aot.EmptyProductsNameAndPrices();

            Assert.NotEmpty(result);
            Assert.Equal(expected[0].Products.Count, result.Count);
        }

        [Fact]
        public async Task GetDaysEmptyProducts_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysEmptyProducts_ShouldGet));
            var expected = await Helper.SetExampleDays(1, db);


            var aot = new GetInventoryAoT(db);

            var result = await aot.EmptyProducts();

            Assert.NotEmpty(result);
            Assert.Equal(expected[0].Products.Count, result.Count);
        }

    }
}
