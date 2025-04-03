using DataBase.Model;

using FluentAssertions;

using Shared.Data;

namespace DataBaseUnitTest.Created
{
    public class CreatedDataBaseMain
    {
        [Fact]
        public async Task CheckOnlyLogExist()
        {
            var _db = await DataBaseUnitTest.DataGet.Helper.CreateDataBaseForTest(nameof(CheckOnlyLogExist));
            var _createdDataBase = new CreatedDataBase(_db);

            await _createdDataBase.Update(0, 1, null);

            var obj = _db.DataBase.GetTableInfo(nameof(LogsModel));

            bool exist = obj.Count > 0;

            exist.Should().BeTrue();
        }

    }
}
