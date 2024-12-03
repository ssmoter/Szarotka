using FluentAssertions;

using Shared.Data;
using DataBase.Model.EntitiesRoutes;
using DataBase.Data;

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
            _db.DataBase.DropTable<CustomerRoutes>();
            _db.DataBase.DropTable<ResidentialAddress>();
            _db.DataBase.DropTable<Routes>();
            _db.DataBase.DropTable<SelectedDayOfWeekRoutes>();

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
            var obj = _db.DataBase.GetTableInfo(nameof(CustomerRoutes));
            var exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(ResidentialAddress));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(Routes));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(SelectedDayOfWeekRoutes));
            exist = obj.Count > 0;
            list.Add(exist);


            list.Should().HaveCountGreaterThanOrEqualTo(4);
        }

        [Fact]
        public void CreatedDefaultRoutes()
        {
            var obj = _db.DataBase.Table<Routes>().ToArray();

            var length = DriversRoutesTables.GetDefaultRoutes().Length;

            obj.Should().HaveCount(length);


        }



    }
}
