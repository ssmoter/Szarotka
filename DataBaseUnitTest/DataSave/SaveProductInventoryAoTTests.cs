using DataBase.Model.EntitiesInventory;

namespace DataBaseUnitTest.DataSave
{
    public class SaveProductInventoryAoTTests
    {

        [Fact]
        private async Task SaveProduct_ShouldSave()
        {
            var dbName = Helper.GetPath + nameof(SaveProduct_ShouldSave) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var idDay = Guid.CreateVersion7();
            var ProductNameId = Guid.CreateVersion7();
            var ProductPriceId = Guid.CreateVersion7();
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Product()
            {
                DayId = idDay,
                ProductNameId = ProductNameId,
                ProductPriceId = ProductPriceId,
            };
            await saveInventory.SaveProduct(item, driverId.ToByteArray());

            var expected = await db.DataBaseAsync.FindAsync<Product>(x => x.Id == item.Id);

            Assert.Equal(expected.Id, item.Id);
            Assert.Equal(expected.Created, item.Created);
            Assert.Equal(expected.Updated, item.Updated);
        }
        [Fact]
        private async Task SaveProduct_DayId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveProduct_DayId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var idDay = Guid.CreateVersion7();
            var ProductNameId = Guid.CreateVersion7();
            var ProductPriceId = Guid.CreateVersion7();
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Product()
            {
                ProductNameId = ProductNameId,
                ProductPriceId = ProductPriceId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveProduct(item, driverId.ToByteArray()));
        }

        [Fact]
        private async Task SaveProduct_ProductName_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveProduct_ProductName_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var idDay = Guid.CreateVersion7();
            var ProductNameId = Guid.CreateVersion7();
            var ProductPriceId = Guid.CreateVersion7();
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Product()
            {
                DayId = idDay,
                ProductPriceId = ProductPriceId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveProduct(item, driverId.ToByteArray()));
        }

        [Fact]
        private async Task SaveProduct_ProductPrice_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveProduct_ProductPrice_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var idDay = Guid.CreateVersion7();
            var ProductNameId = Guid.CreateVersion7();
            var ProductPriceId = Guid.CreateVersion7();
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Product()
            {
                DayId = idDay,
                ProductNameId = ProductNameId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveProduct(item, driverId.ToByteArray()));
        }

        [Fact]
        private async Task SaveProduct_DriverId_IsNull()
        {
            var dbName = Helper.GetPath + nameof(SaveProduct_DriverId_IsNull) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            byte[]? driverId = null;
            var idDay = Guid.CreateVersion7();
            var ProductNameId = Guid.CreateVersion7();
            var ProductPriceId = Guid.CreateVersion7();
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var item = new Product()
            {
                DayId = idDay,
                ProductNameId = ProductNameId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveProduct(item, driverId!));
        }






    }
}
