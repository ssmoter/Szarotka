using DataBase.Model.EntitiesInventory;

using DataBaseUnitTest;

using Inventory.Data.InventoryApi;

namespace InventoryUnitTest.API.Send
{
    [Collection("Serwer")]
    public class SendProductHttpTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly LocalHttpClientFactory _clientFactory;
        private readonly ISendProductHttp _sendProdukt;
        private readonly DataBase.Data.Get.IGetInventoryAoT _getInventoryAoT;

        public SendProductHttpTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _clientFactory = new LocalHttpClientFactory(factory);

            var a = _factory.CreateDefaultClient();
            _sendProdukt = new SendProductHttp(factory.TestDatabase!, _clientFactory);

            _getInventoryAoT = new DataBase.Data.Get.GetInventoryAoT(factory.TestDatabase!);
        }

        [Fact]
        public async Task SendProductHttp()
        {
            var a = _factory.CreateClient();

            var testList = await _getInventoryAoT.EmptyProductsNameAndPrices();
            var emptyProdukt = new EmptyProducts(testList);

            var test = emptyProdukt.Products[0];
            test.Name.Description = Guid.NewGuid().ToString();
            test.Prices[0].Price = Random.Shared.Next(0, 1000);

            var result = await _sendProdukt.SendProduct(test, forceUpdate: true);

            var expected = new EmptyProducts(await _getInventoryAoT.EmptyProductsNameAndPrices());

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected.Products[0]);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(test);

            Assert.True(result.httpMessage.IsSuccessStatusCode);
            Assert.Equal(expectedJson, resultJson);
        }
        [Fact]
        public async Task SendProductsHttp()
        {
            var a = _factory.CreateClient();

            var testList = await _getInventoryAoT.EmptyProductsNameAndPrices();
            var emptyProdukt = new EmptyProducts(testList);

            foreach (var item in emptyProdukt.Products)
            {
                item.Name.Description = Guid.NewGuid().ToString();
                item.Prices[0].Price = Random.Shared.Next(0, 1000);
            }

            var result = await _sendProdukt.SendProducts(emptyProdukt, forceUpdate: true);

            var expected = new EmptyProducts(await _getInventoryAoT.EmptyProductsNameAndPrices());

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(emptyProdukt);

            Assert.True(result.httpMessage.IsSuccessStatusCode);
            Assert.Equal(expectedJson, resultJson);
        }

    }
}
