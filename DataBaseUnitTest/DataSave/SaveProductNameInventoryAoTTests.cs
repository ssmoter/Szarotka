using DataBase.Model.EntitiesInventory;

namespace DataBaseUnitTest.DataSave
{
    public class SaveProductNameInventoryAoTTests
    {
        [Fact]
        private async Task SaveProductName_ShouldSave()
        {
            var dbName = Helper.GetPath + nameof(SaveProductName_ShouldSave) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();

            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new ProductName()
            {
            };
            await saveInventory.SaveProductName(item, driverId.ToByteArray());

            var expected = await db.DataBaseAsync.FindAsync<ProductName>(x => x.Id == item.Id);

            Assert.Equal(expected.Id, item.Id);
            Assert.Equal(expected.Created, item.Created);
            Assert.Equal(expected.Updated, item.Updated);
        }
        [Fact]
        private async Task SaveProductName_DriverId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveProductName_DriverId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.Empty;
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new ProductName()
            {
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveProductName(item, driverId.ToByteArray()));
        }
    }
}
