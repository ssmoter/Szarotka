using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTDaysExtensionIdTest
    {
        [Fact]
        public async Task GetDaysExtensionId_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtensionId_ShouldGet));

            var excepted = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.Day(excepted[0].Id);

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted[0]);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(exceptedJson, resultJson);
        }

        [Fact]
        public async Task GetDaysExtensionId_IdIsNull()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtensionId_IdIsNull));

            var aot = new GetInventoryAoT(db);
            await Assert.ThrowsAsync<ArgumentNullException>(
                    () => aot.Day(Guid.Empty));
        }
    }
}
