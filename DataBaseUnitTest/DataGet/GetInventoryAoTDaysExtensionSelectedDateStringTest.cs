using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTDaysExtensionSelectedDateStringTest
    {
        [Fact]
        public async Task GetDaysExtensionSelectedDateString_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtensionSelectedDateString_ShouldGet));

            var excepted = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.DaySelectedDateString(excepted[0].SelectedDateString, excepted[0].UserCreatedId);

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted[0]);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(exceptedJson, resultJson);
        }

        [Fact]
        public async Task GetDaysExtensionSelectedDateString_IsNull()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtensionSelectedDateString_IsNull));

            var aot = new GetInventoryAoT(db);
            await Assert.ThrowsAsync<ArgumentNullException>(
                    () => aot.DaySelectedDateString("", Guid.CreateVersion7()));
        }
    }
}
