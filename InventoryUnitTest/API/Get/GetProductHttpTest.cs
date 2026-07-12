using DataBase.Model.EntitiesInventory;

using DataBaseUnitTest;

using Inventory.Data.InventoryApi;

namespace InventoryUnitTest.API.Get
{
    [Collection("Serwer")]
    public class GetProductHttpTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly LocalHttpClientFactory _clientFactory;
        private readonly IGetProductHttp _getProduct;
        private readonly DataBase.Data.Get.IGetInventoryAoT _getInventoryAoT;

        public GetProductHttpTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _clientFactory = new LocalHttpClientFactory(factory);

            var a = _factory.CreateDefaultClient();
            _getProduct = new GetProductHttp(_clientFactory, factory.TestDatabase!);

            _getInventoryAoT = new DataBase.Data.Get.GetInventoryAoT(factory.TestDatabase!);
        }

        [Fact]
        public async Task GetDayHttpId()
        {
            var expected = await _getInventoryAoT.EmptyProductsNameAndPrices();

            var result = await _getProduct.GetProducts();

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(new EmptyProducts(expected));
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }
    }
}
