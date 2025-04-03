using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTDaysTest
    {
        [Fact]
        public async Task GetDays_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDays_ShouldGet));

            var excepted = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.Days("", null!);

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(exceptedJson, resultJson);
        }

    }
}
