using DataBase.Model.EntitiesRoutes;

using FluentAssertions;

using Shared.Data;

namespace DataBaseUnitTest.Created
{
    public class CreatedDriversRoutes
    {
        [Fact]
        public async Task CreatedExist()
        {
            var _db = await DataBaseUnitTest.DataGet.Helper.CreateDataBaseForTest(nameof(CreatedExist));

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
        public async Task CreatedDefaultRoutes()
        {
            var _db = await DataBaseUnitTest.DataGet.Helper.CreateDataBaseForTest(nameof(CreatedDefaultRoutes));

            var obj = _db.DataBase.Table<Routes>().ToArray();

            var length = DriversRoutesTables.GetDefaultRoutes().Length;

            obj.Should().HaveCount(length);


        }



    }
}
