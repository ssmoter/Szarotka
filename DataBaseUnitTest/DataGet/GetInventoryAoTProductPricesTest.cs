using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTProductPricesTest
    {
        [Fact]
        public async Task GetProductPrices_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetProductPrices_ShouldGet));
            var excepted = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.GetProductPrices(excepted[0].Products[0].ProductNameId);
            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted[0].Products[0].Price);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result[0]);

            Assert.NotNull(result);
            Assert.Equal(exceptedJson,resultJson);
        }

        [Fact]
        public async Task GetProductPrice_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetProductPrice_ShouldGet));
            var excepted = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.GetProductPrice(excepted[0].Products[0].ProductPriceId);
            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted[0].Products[0].Price);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.NotNull(result);
            Assert.Equal(exceptedJson, resultJson);
        }
    }
}
