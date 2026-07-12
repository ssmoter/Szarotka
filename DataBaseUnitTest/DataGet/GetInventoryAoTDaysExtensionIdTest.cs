using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTDaysExtensionIdTest
    {
        [Fact]
        public async Task GetDaysExtensionId_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtensionId_ShouldGet));

            var expected = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.Day(expected[0].Id);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected[0]);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
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
