using DataBase.Model.EntitiesInventory;

namespace DataBaseUnitTest.DataSave
{
    public class SaveProductPriceInventoryAoTTests
    {
        [Fact]
        private async Task SaveProductPrice_ShouldSave()
        {
            var dbName = Helper.GetPath + nameof(SaveProductPrice_ShouldSave) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();

            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new ProductPrice()
            {
            };
            await saveInventory.SaveProductPrice(item, driverId.ToByteArray());

            var expected = await db.DataBaseAsync.FindAsync<ProductPrice>(x => x.Id == item.Id);

            Assert.Equal(expected.Id, item.Id);
            Assert.Equal(expected.Created, item.Created);
            Assert.Equal(expected.Updated, item.Updated);
        }
        [Fact]
        private async Task SaveProductPrice_DriverId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveProductPrice_DriverId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.Empty;
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new ProductPrice()
            {
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveProductPrice(item, driverId.ToByteArray()));
        }
    }
}
