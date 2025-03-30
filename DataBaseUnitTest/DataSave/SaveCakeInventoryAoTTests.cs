using DataBase.Model.EntitiesInventory;

namespace DataBaseUnitTest.DataSave
{
    public class SaveCakeInventoryAoTTests
    {

        [Fact]
        private async Task SaveCake_ShouldSave()
        {
            var dbName = Helper.GetPath + nameof(SaveCake_ShouldSave) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var idDay = Guid.CreateVersion7();

            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Cake()
            {
                DayId = idDay,
            };
            await saveInventory.SaveCake(item, driverId.ToByteArray());

            var expected = await db.DataBaseAsync.FindAsync<Cake>(x => x.Id == item.Id);

            Assert.Equal(expected.Id, item.Id);
            Assert.Equal(expected.Created, item.Created);
            Assert.Equal(expected.Updated, item.Updated);
        }
        [Fact]
        private async Task SaveCake_DriverId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveCake_DriverId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.Empty;
            var dayId = Guid.CreateVersion7();
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Cake()
            {
                DayId = dayId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveCake(item, driverId.ToByteArray()));
        }
        [Fact]
        private async Task SaveCake_DriverId_IsNull()
        {
            var dbName = Helper.GetPath + nameof(SaveCake_DriverId_IsNull) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            byte[]? driverId = null;
            var dayId = Guid.CreateVersion7();

            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Cake()
            {
                DayId = dayId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveCake(item, driverId!));
        }
        [Fact]
        private async Task SaveCake_DayId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveCake_DayId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();

            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Cake()
            {
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveCake(item, driverId.ToByteArray()));
        }

    }
}
