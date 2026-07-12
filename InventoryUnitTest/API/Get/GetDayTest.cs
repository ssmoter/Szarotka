using DataBase.Data.Get;

using DataBaseUnitTest;

using Inventory.Data.InventoryApi;

namespace InventoryUnitTest.API.Get
{
    [Collection("Serwer")]
    public class GetDayTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly LocalHttpClientFactory _clientFactory;
        private readonly IGetDayHttp _getDay;
        private readonly DataBase.Data.Get.IGetInventoryAoT _getInventoryAoT;

        public GetDayTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _clientFactory = new LocalHttpClientFactory(factory);

            var a = _factory.CreateDefaultClient();
            _getDay = new GetDayHttp(_clientFactory, factory.TestDatabase!);

            _getInventoryAoT = new DataBase.Data.Get.GetInventoryAoT(factory.TestDatabase!);
        }



        [Fact]
        public async Task GetDayHttpId()
        {
            var a = _factory.CreateClient();

            var expected = await _getInventoryAoT.Days(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks, []);

            var result = await _getDay.GetDay(expected[0].Id);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected[0]);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task GetDayHttpSelectedDateStringUserId()
        {
            var a = _factory.CreateClient();

            var expected = await _getInventoryAoT.Days(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks, []);

            var result = await _getDay.GetDay(expected[0].SelectedDateString, expected[0].DriverGuid);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected[0]);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task GetDays()
        {
            var a = _factory.CreateClient();

            var expected = await _getInventoryAoT.Days(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks, []);

            var result = await _getDay.GetDays(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks, []);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }
    }
}