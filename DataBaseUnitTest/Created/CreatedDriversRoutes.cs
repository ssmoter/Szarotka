using DataBase.Data;

using FluentAssertions;

namespace DataBaseUnitTest.Created
{
    public class CreatedDriversRoutes : IAsyncLifetime
    {
        private readonly AccessDataBase _db;
        private readonly DriversRoutesTables _driversRoutesTables;
        public CreatedDriversRoutes()
        {
            _db = new AccessDataBase(CreatedDataBaseMain.Path);
            _driversRoutesTables = new DriversRoutesTables(_db);
        }

        public Task DisposeAsync()
        {
            _db.DataBase.DropTable<DataBase.Model.EntitiesRoutes.CustomerRoutes>();
            _db.DataBase.DropTable<DataBase.Model.EntitiesRoutes.ResidentialAddress>();
            _db.DataBase.DropTable<DataBase.Model.EntitiesRoutes.Routes>();
            _db.DataBase.DropTable<DataBase.Model.EntitiesRoutes.SelectedDayOfWeekRoutes>();

            _db.Dispose();
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            await _driversRoutesTables.Update(0, 1, null);
        }
        [Fact]
        public void CreatedExist()
        {
            List<bool> list = [];
            var obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesRoutes.CustomerRoutes));
            var exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesRoutes.ResidentialAddress));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesRoutes.Routes));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesRoutes.SelectedDayOfWeekRoutes));
            exist = obj.Count > 0;
            list.Add(exist);


            list.Should().HaveCountGreaterThanOrEqualTo(4);
        }

        [Fact]
        public void CreatedDefaultRoutes()
        {
            var obj = _db.DataBase.Table<DataBase.Model.EntitiesRoutes.Routes>().ToArray();

            var length = DriversRoutesTables.GetDefaultRoutes().Length;

            obj.Should().HaveCount(length);


        }



    }
}
