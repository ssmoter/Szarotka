using DataBase.Model.EntitiesRoutes;

namespace DataBaseUnitTest.DataSave
{
    public class SaveRoutesDriversRoutesAoTTests
    {
        [Fact]
        private async Task SaveRoutes_ShouldSave()
        {
            var dbName = Helper.GetPath + nameof(SaveRoutes_ShouldSave) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.CreateVersion7();

            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new Routes()
            {
            };
            await save.SaveRoutes(item, driverId.ToByteArray());

            var expected = await db.DataBaseAsync.FindAsync<Routes>(x => x.Id == item.Id);

            Assert.Equal(expected.Id, item.Id);
            Assert.Equal(expected.Created, item.Created);
            Assert.Equal(expected.Updated, item.Updated);
        }
        [Fact]
        private async Task SaveRoutes_DriverId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveRoutes_DriverId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.Empty;
            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new Routes()
            {
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => save.SaveRoutes(item, driverId.ToByteArray()));
        }

    }
}
