using DataBase.Data;

using FluentAssertions;

namespace DataBaseUnitTest.Created
{
    public class CreatedDataBaseMain : IAsyncLifetime
    {
        private readonly AccessDataBase _db;
        private readonly CreatedDataBase _createdDataBase;
        public static string Path => DataBase.Helper.Constants.GetPathFolder + "\\DataBaseSzarotkaSQLiteUnitTest.db3";
        public CreatedDataBaseMain()
        {
            _db = new AccessDataBase(Path);
            _createdDataBase = new CreatedDataBase(_db);

        }
        public Task DisposeAsync()
        {
            _db.DataBase.DropTable<DataBase.Model.DataBaseVersion>();
            _db.DataBase.DropTable<DataBase.Model.LogsModel>();

            _db.Dispose();
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            await _createdDataBase.Update(0, 1, null);
        }

        [Fact]
        public void CheckOnlyDataBaseVersion()
        {
            var _db = new AccessDataBase(Path);
            var _createdDataBase = new CreatedDataBase(_db);
            var obj = _createdDataBase.GetCurrentVersion();

            obj.Should().Be(new DataBase.Model.DataBaseVersion()
            {
                DataBase = 0,
                DriversRoutes = 0,
                Inventory = 0
            });
        }
        [Fact]
        public async void CheckOnlyLogExist()
        {
            var _db = new AccessDataBase(Path);
            var _createdDataBase = new CreatedDataBase(_db);
            await _createdDataBase.Update(0, 1, null);

            var obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.LogsModel));

            bool exist = obj.Count > 0;

            exist.Should().BeTrue();
        }

    }
}
