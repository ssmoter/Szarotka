using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetDriverRoutesAoTCustomerRoutesTest
    {
        [Fact]
        public async Task GetCustomerRoutes_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoutes_ShouldGet));

            var excepted = Helper.SetExampleCustomerRoutes(15, db).OrderBy(x => x.CreatedTicks);

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoutes("", false, null!);

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result.OrderBy(x => x.CreatedTicks));

            Assert.Equal(exceptedJson, resultJson);
        }
    }
}
