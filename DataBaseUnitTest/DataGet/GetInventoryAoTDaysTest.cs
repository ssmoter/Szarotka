using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTDaysTest
    {
        [Fact]
        public async Task GetDays_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDays_ShouldGet));

            var expected = await Helper.SetExampleDays(15, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.Days("", null!);

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }

    }
}
