using DataBase.Model.EntitiesRoutes;

namespace DataBaseUnitTest.DataSave
{
    public class SaveSelectedDayOfWeekDriversRoutesAoTTests
    {
        [Fact]
        private async Task SaveSelectedDayOfWeekRoutes_ShouldSave()
        {
            var dbName = Helper.GetPath + nameof(SaveSelectedDayOfWeekRoutes_ShouldSave) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var customerId = Guid.CreateVersion7();

            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new SelectedDayOfWeekRoutes()
            {
                CustomerId = customerId,
            };
            await save.SaveSelectedDayOfWeekRoutes(item, driverId.ToByteArray());

            var expected = await db.DataBaseAsync.FindAsync<SelectedDayOfWeekRoutes>(x => x.Id == item.Id);

            Assert.Equal(expected.Id, item.Id);
            Assert.Equal(expected.Created, item.Created);
            Assert.Equal(expected.Updated, item.Updated);
        }
        [Fact]
        private async Task SaveSelectedDayOfWeekRoutes_DriverId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveSelectedDayOfWeekRoutes_DriverId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.Empty;
            var customerId = Guid.CreateVersion7();

            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new SelectedDayOfWeekRoutes()
            {
                CustomerId = customerId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => save.SaveSelectedDayOfWeekRoutes(item, driverId.ToByteArray()));
        }
        [Fact]
        private async Task SaveSelectedDayOfWeekRoutes_CustomerId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveSelectedDayOfWeekRoutes_CustomerId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var customerId = Guid.Empty;

            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new SelectedDayOfWeekRoutes()
            {
                CustomerId = customerId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => save.SaveSelectedDayOfWeekRoutes(item, driverId.ToByteArray()));
        }
    }
}
