using DataBase.Data.Get;

using DataBaseUnitTest;

using Inventory.Data.InventoryApi;

namespace InventoryUnitTest.API.Send
{
    [Collection("Serwer")]
    public class SendDayHttpTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly LocalHttpClientFactory _clientFactory;
        private readonly ISendDayHttp _sendDay;
        private readonly DataBase.Data.Get.IGetInventoryAoT _getInventoryAoT;

        public SendDayHttpTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _clientFactory = new LocalHttpClientFactory(factory);

            var a = _factory.CreateDefaultClient();
            _sendDay = new SendDayHttp(factory.TestDatabase!, _clientFactory);

            _getInventoryAoT = new DataBase.Data.Get.GetInventoryAoT(factory.TestDatabase!);
        }



        [Fact]
        public async Task SendDayHttp()
        {
            var testList = _factory.AllDays;
            var test = testList[0];
            test.Description = Guid.NewGuid().ToString();

            var result = await _sendDay.SendDay(test);

            var expected = await _getInventoryAoT.Day(test.Id);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(test);

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(expectedJson, resultJson);
        }


        [Fact]
        public async Task SendDaysHttp()
        {
            var testList = _factory.AllDays;

            for (int i = 0; i < testList.Count; i++)
            {
                testList[i].Description = Guid.NewGuid().ToString();
            }

            var result = await _sendDay.SendDays(testList);

            var expected = await _getInventoryAoT.Days(DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks, []);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected.OrderBy(x=>x.Id));
            var resultJson = System.Text.Json.JsonSerializer.Serialize(testList.OrderBy(x => x.Id));

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(expectedJson, resultJson);
        }

    }
}
