using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetDriverRoutesAoTCustomerExtensionTest
    {
        [Fact]
        public async Task GetCustomerRoute_ShouldGet_SingleFromId()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoute_ShouldGet_SingleFromId));

            var expected = Helper.SetExampleCustomerRoutes(15, db).OrderBy(x => x.CreatedTicks);

            var id = expected.First();

            var aot = new GetDriverRoutesAoT(db);

            var result = await aot.CustomerRoute(id.Id);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(id);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }
        [Fact]
        public async Task GetCustomerRoute_ShouldThrow_GuidEmpty()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetCustomerRoute_ShouldThrow_GuidEmpty));

            var aot = new GetDriverRoutesAoT(db);


            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                aot.CustomerRoute(Guid.Empty)
            );
        }
    }
}
