using DataBase.Model.EntitiesRoutes;

namespace DataBaseUnitTest.DataSave
{
    public class SaveResidentialAddressDriversRoutesAoTTests
    {
        [Fact]
        private async Task SaveResidentialAddress_ShouldSave()
        {
            var dbName = Helper.GetPath + nameof(SaveResidentialAddress_ShouldSave) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var customerId = Guid.CreateVersion7();

            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new ResidentialAddress()
            {
                CustomerId = customerId,
            };
            await save.SaveResidentialAddress(item, driverId.ToByteArray());

            var expected = await db.DataBaseAsync.FindAsync<ResidentialAddress>(x => x.Id == item.Id);

            Assert.Equal(expected.Id, item.Id);
            Assert.Equal(expected.Created, item.Created);
            Assert.Equal(expected.Updated, item.Updated);
        }
        [Fact]
        private async Task SaveResidentialAddress_DriverId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveResidentialAddress_DriverId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.Empty;
            var customerId = Guid.CreateVersion7();
            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new ResidentialAddress()
            {
                CustomerId = customerId
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => save.SaveResidentialAddress(item, driverId.ToByteArray()));
        }

        [Fact]
        private async Task SaveResidentialAddress_CustomerId_IsEmpty()
        {
            var dbName = Helper.GetPath + nameof(SaveResidentialAddress_CustomerId_IsEmpty) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseRoutesForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var customerId = Guid.Empty;
            var save = new DataBase.Data.Save.SaveDriverRoutesAoT(db);

            var item = new ResidentialAddress()
            {
                CustomerId = customerId
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => save.SaveResidentialAddress(item, driverId.ToByteArray()));
        }
    }
}
