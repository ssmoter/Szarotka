using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTExtensionDaysExtensionTest
    {
        [Fact]
        public async Task GetDaysExtension_ShouldGet_AllGuids()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtension_ShouldGet_AllGuids));

            var expected = await Helper.SetExampleDays(15, db);

            expected = [.. expected.OrderBy(x => x.SelectedDateTicks)];

            var ids = expected.Select(x => x.UserCreatedId).ToArray();

            var aot = new GetInventoryAoT(db);

            var result = await aot.Days(expected.First().SelectedDateTicks,
                                        expected.Last().SelectedDateTicks,
                                        ids);

            result = [.. result.OrderBy(x => x.SelectedDateTicks)];

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task GetDaysExtension_ShouldGet_SingleGuid()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtension_ShouldGet_SingleGuid));

            var expected = await Helper.SetExampleDays(15, db);

            expected = [.. expected.OrderBy(x => x.SelectedDateTicks)];

            var ids = expected.Select(x => x.UserCreatedId).First();

            expected = [.. expected.Where(x => x.UserCreatedId == ids)];

            var aot = new GetInventoryAoT(db);

            var result = await aot.Days(expected.First().SelectedDateTicks,
                                        expected.Last().SelectedDateTicks,
                                        [ids]);

            result = [.. result.OrderBy(x => x.SelectedDateTicks)];

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }

        [Fact]
        public async Task GetDaysExtension_ShouldGet_NoGuids_GetAll()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtension_ShouldGet_NoGuids_GetAll));

            var expected = await Helper.SetExampleDays(15, db);

            expected = [.. expected.OrderBy(x => x.SelectedDateTicks)];

            var aot = new GetInventoryAoT(db);

            var result = await aot.Days(expected.First().SelectedDateTicks,
                                        expected.Last().SelectedDateTicks,
                                        []);

            result = [.. result.OrderBy(x => x.SelectedDateTicks)];

            var expectedJson = System.Text.Json.JsonSerializer.Serialize(expected);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(expectedJson, resultJson);
        }

    }
}
