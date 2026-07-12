using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetDriverRoutesAoTCustomersExtensionTest
    {
        [Fact]
        public async Task GetCustomerRoutes_ShouldGet_AllFromRouteId()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoutes_ShouldGet_AllFromRouteId));

            var expected = Helper.SetExampleCustomerRoutes(16, db);

            var id = expected.First();

            var expectedSelected = expected.Where(x => x.RoutesId == id.RoutesId).OrderBy(x => x.CreatedTicks);

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoutes(id.RoutesId, []);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expectedSelected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result!.OrderBy(x => x.CreatedTicks));

            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task GetCustomerRoutes_ShouldGet_AllWhereDayOfWeekIsSelected_Single()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoutes_ShouldGet_AllWhereDayOfWeekIsSelected_Single));

            var expected = Helper.SetExampleCustomerRoutes(20, db);

            var id = expected.First();

            var expectedSelected = expected
                .Where(x => x.RoutesId == id.RoutesId && x.DayOfWeek.Monday == true)
                .OrderBy(x => x.CreatedTicks);

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoutes(id.RoutesId, [DayOfWeek.Monday]);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expectedSelected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result!.OrderBy(x => x.CreatedTicks));

            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task GetCustomerRoutes_ShouldGet_AllWhereDayOfWeekIsSelected_Multi()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoutes_ShouldGet_AllWhereDayOfWeekIsSelected_Multi));

            var expected = Helper.SetExampleCustomerRoutes(100, db, 60);

            var id = expected.First();

            var expectedSelected = expected
                .Where(x => x.RoutesId == id.RoutesId && (x.DayOfWeek.Monday == true || x.DayOfWeek.Friday == true))
                .OrderBy(x => x.CreatedTicks);

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoutes(id.RoutesId, [DayOfWeek.Monday, DayOfWeek.Friday]);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expectedSelected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result!.OrderBy(x => x.CreatedTicks));

            Assert.Equal(expectedJson, resultJson);
        }
    }
}
