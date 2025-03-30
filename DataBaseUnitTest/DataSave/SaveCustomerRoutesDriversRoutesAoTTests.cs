using DataBase.Model.EntitiesRoutes;

namespace DataBaseUnitTest.DataSave
{
    public class SaveCustomerRoutesDriversRoutesAoTTests
    {
        [Fact]
        private async Task SaveCustomerRoutes_ShouldSave()
        {
            var dbName = Helper.GetPath + nameof(SaveCustomerRoutes_ShouldSave) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var routesId = Guid.CreateVersion7();

            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new CustomerRoutes()
            {
                RoutesId = routesId,
            };
            await save.SaveCustomerRoutes(item, driverId.ToByteArray());

            var expected = await db.DataBaseAsync.FindAsync<CustomerRoutes>(x => x.Id == item.Id);

            Assert.Equal(expected.Id, item.Id);
            Assert.Equal(expected.Created, item.Created);
            Assert.Equal(expected.Updated, item.Updated);
        }
        [Fact]
        private async Task SaveCustomerRoutes_DriverId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveCustomerRoutes_DriverId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.Empty;
            var routesId = Guid.CreateVersion7();
            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new CustomerRoutes()
            {
                RoutesId = routesId
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => save.SaveCustomerRoutes(item, driverId.ToByteArray()));
        }
        [Fact]
        private async Task SaveCustomerRoutes_RoutesId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveCustomerRoutes_RoutesId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var routesId = Guid.Empty;
            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new CustomerRoutes()
            {
                RoutesId = routesId
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => save.SaveCustomerRoutes(item, driverId.ToByteArray()));
        }
    }
}
