using DataBase.Data.Get;

using DataBaseUnitTest;

using DriversRoutes.Data.RouteApi;

using Xunit;

namespace DriversRoutesUnitTest.API.Send
{
    [Collection("Serwer")]
    public class SendCustomersHttpTest
    : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly LocalHttpClientFactory _clientFactory;

        private readonly SendCustomersHttp _sendCustomersHttp;
        private readonly DataBase.Data.Get.IGetDriverRoutesAoT _iGetDriverRoutesAoT;

        public SendCustomersHttpTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _clientFactory = new LocalHttpClientFactory(factory);

            var a = _factory.CreateDefaultClient();

            _sendCustomersHttp = new SendCustomersHttp(factory.TestDatabase!, _clientFactory);
            _iGetDriverRoutesAoT = new DataBase.Data.Get.GetDriverRoutesAoT(factory.TestDatabase!);
        }

        [Fact]
        public async Task SendCustomerRoute()
        {
            var all = await _iGetDriverRoutesAoT.CustomerRoutes();

            var edit = all[0];
            edit.Description = Guid.NewGuid().ToString();

            var result = await _sendCustomersHttp.SendCustomerRoute(edit, forceUpdate: true);

            var actual = await _iGetDriverRoutesAoT.CustomerRoute(edit.Id);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(edit);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(actual);

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(expectedJson, resultJson);
        }
        [Fact]
        public async Task SendCustomerRoutes()
        {
            var all = await _iGetDriverRoutesAoT.CustomerRoutes();

            foreach (var item in all)
            {
                item.Description = Guid.NewGuid().ToString();
            }

            var result = await _sendCustomersHttp.SendCustomerRoutes(all, forceUpdate: true);

            var actual = await _iGetDriverRoutesAoT.CustomerRoutes();

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(all);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(actual);

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal(expectedJson, resultJson);
        }

    }
}
