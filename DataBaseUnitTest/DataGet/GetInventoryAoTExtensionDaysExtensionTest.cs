using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTExtensionDaysExtensionTest
    {
        [Fact]
        public async Task GetDaysExtension_ShouldGet_AllGuids()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtension_ShouldGet_AllGuids));

            var excepted = await Helper.SetExampleDays(15, db);

            excepted = [.. excepted.OrderBy(x => x.SelectedDateTicks)];

            var ids = excepted.Select(x => x.UserCreatedId).ToArray();

            var aot = new GetInventoryAoT(db);

            var result = await aot.Days(excepted.First().SelectedDateTicks,
                                        excepted.Last().SelectedDateTicks,
                                        ids);

            result = [.. result.OrderBy(x => x.SelectedDateTicks)];

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(exceptedJson, resultJson);
        }

        [Fact]
        public async Task GetDaysExtension_ShouldGet_SingleGuid()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtension_ShouldGet_SingleGuid));

            var excepted = await Helper.SetExampleDays(15, db);

            excepted = [.. excepted.OrderBy(x => x.SelectedDateTicks)];

            var ids = excepted.Select(x => x.UserCreatedId).First();

            excepted = [.. excepted.Where(x => x.UserCreatedId == ids)];

            var aot = new GetInventoryAoT(db);

            var result = await aot.Days(excepted.First().SelectedDateTicks,
                                        excepted.Last().SelectedDateTicks,
                                        [ids]);

            result = [.. result.OrderBy(x => x.SelectedDateTicks)];

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(exceptedJson, resultJson);
        }

        [Fact]
        public async Task GetDaysExtension_ShouldGet_NoGuids_GetAll()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetDaysExtension_ShouldGet_NoGuids_GetAll));

            var excepted = await Helper.SetExampleDays(15, db);

            excepted = [.. excepted.OrderBy(x => x.SelectedDateTicks)];

            var aot = new GetInventoryAoT(db);

            var result = await aot.Days(excepted.First().SelectedDateTicks,
                                        excepted.Last().SelectedDateTicks,
                                        []);

            result = [.. result.OrderBy(x => x.SelectedDateTicks)];

            var exceptedJson = System.Text.Json.JsonSerializer.Serialize(excepted);
            var resultJson = System.Text.Json.JsonSerializer.Serialize(result);

            Assert.Equal(exceptedJson, resultJson);
        }

    }
}
