using DataBase.Data.Get;
using DataBase.Model.EntitiesRoutes;

using DataBaseUnitTest;

using DriversRoutes.Data.RouteApi;

using Xunit;

namespace DriversRoutesUnitTest.API.Get
{
    [Collection("Serwer")]
    public class GetCustomersHttpTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly LocalHttpClientFactory _clientFactory;

        private readonly IGetCustomersHttp _getCustomersHttp;
        private readonly DataBase.Data.Get.IGetDriverRoutesAoT _iGetDriverRoutesAoT;

        public GetCustomersHttpTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _clientFactory = new LocalHttpClientFactory(factory);

            var a = _factory.CreateDefaultClient();

            _getCustomersHttp = new GetCustomersHttp(factory.TestDatabase!, _clientFactory);
            _iGetDriverRoutesAoT = new DataBase.Data.Get.GetDriverRoutesAoT(factory.TestDatabase!);
        }


        [Fact]
        public async Task GetCustomerRoutesRouteId()
        {

            var expected = _factory.AllCustomers;
            var guid = expected[0].RoutesId;

            var result = await _getCustomersHttp.GetCustomerRoutes(guid, new DataBase.Model.EntitiesRoutes.SelectedDayOfWeekRoutes()
            {
                Monday = true,
                Thursday = true,
                Wednesday = true,
                Friday = true,
                Saturday = true,
                Sunday = true,
                Tuesday = true,
            });

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected.Where(x => x.RoutesId == guid).ToArray());
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result.ToArray());

            Assert.Equal(expectedJson, resultJson);
        }
        [Fact]
        public async Task GetCustomerRoutesIds()
        {

            IList<CustomerRoutes> all = _factory.AllCustomers;
            var expected = all.Take(5);

            var result = await _getCustomersHttp.GetCustomerRoutes([.. expected.Select(x => x.Id)]);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected.ToArray());
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result.ToArray());

            Assert.Equal(expectedJson, resultJson);
        }

    }
}
