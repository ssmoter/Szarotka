using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetDriverRoutesAoTCustomerRoutesTest
    {
        [Fact]
        public async Task GetCustomerRoutes_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoutes_ShouldGet));

            var expected = Helper.SetExampleCustomerRoutes(15, db).OrderBy(x => x.CreatedTicks);

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoutes("", false, null!);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result.OrderBy(x => x.CreatedTicks));

            Assert.Equal(expectedJson, resultJson);
        }
    }
}
