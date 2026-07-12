using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTDaysExtensionSelectedDateStringTest
    {
        [Fact]
        public async Task GetDaysExtensionSelectedDateString_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtensionSelectedDateString_ShouldGet));

            var expected = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.DaySelectedDateString(expected[0].SelectedDateString, expected[0].UserCreatedId);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected[0]);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
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
