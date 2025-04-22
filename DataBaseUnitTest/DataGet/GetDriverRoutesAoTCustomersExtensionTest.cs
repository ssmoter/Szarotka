using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetDriverRoutesAoTCustomersExtensionTest
    {
        [Fact]
        public async Task GetCustomerRoutes_ShouldGet_AllFromRouteId()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoutes_ShouldGet_AllFromRouteId));

            var excepted = Helper.SetExampleCustomerRoutes(16, db);

            var id = excepted.First();

            var exceptedSelected = excepted.Where(x => x.RoutesId == id.RoutesId).OrderBy(x => x.CreatedTicks);

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoutes(id.RoutesId, []);

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(exceptedSelected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result!.OrderBy(x => x.CreatedTicks));

            Assert.Equal(exceptedJson, resultJson);
        }

        [Fact]
        public async Task GetCustomerRoutes_ShouldGet_AllWhereDayOfWeekIsSelected_Single()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoutes_ShouldGet_AllWhereDayOfWeekIsSelected_Single));

            var excepted = Helper.SetExampleCustomerRoutes(20, db);

            var id = excepted.First();

            var exceptedSelected = excepted
                .Where(x => x.RoutesId == id.RoutesId && x.DayOfWeek.Monday == true)
                .OrderBy(x => x.CreatedTicks);

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoutes(id.RoutesId, [DayOfWeek.Monday]);

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(exceptedSelected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result!.OrderBy(x => x.CreatedTicks));

            Assert.Equal(exceptedJson, resultJson);
        }

        [Fact]
        public async Task GetCustomerRoutes_ShouldGet_AllWhereDayOfWeekIsSelected_Multi()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoutes_ShouldGet_AllWhereDayOfWeekIsSelected_Multi));

            var excepted = Helper.SetExampleCustomerRoutes(100, db, 60);

            var id = excepted.First();

            var exceptedSelected = excepted
                .Where(x => x.RoutesId == id.RoutesId && (x.DayOfWeek.Monday == true || x.DayOfWeek.Friday == true))
                .OrderBy(x => x.CreatedTicks);

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoutes(id.RoutesId, [DayOfWeek.Monday, DayOfWeek.Friday]);

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(exceptedSelected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result!.OrderBy(x => x.CreatedTicks));

            Assert.Equal(exceptedJson, resultJson);
        }
    }
}
