using DataBase.Model.EntitiesInventory;

namespace DataBaseUnitTest.DataSave
{
    public class SaveDayInventoryAoTTests
    {
        readonly string _path = "";

        public SaveDayInventoryAoTTests()
        {
            _path = Helper.GetPath;
        }

        [Fact]
        private async Task SaveDay_ShouldSaveDay()
        {
            var dbName = _path + nameof(SaveDay_ShouldSaveDay) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            var driverId = Guid.CreateVersion7();
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var day = new Day()
            {
                DriverGuid = driverId,
            };
            await saveInventory.SaveDay(day, driverId.ToByteArray());

            var saveDay = await db.DataBaseAsync.FindAsync<Day>(x => x.Id == day.Id);

            Assert.Equal(saveDay.Id, day.Id);
            Assert.Equal(saveDay.Created, day.Created);
            Assert.Equal(saveDay.Updated, day.Updated);
        }
        [Fact]
        private async Task SaveDay_DriverIdIsNull()
        {
            var dbName = _path + nameof(SaveDay_DriverIdIsNull) + ".db3";
            Helper.Delete(dbName);
            var db = await Helper.CreateDataBaseInventoryForTest(dbName);

            Guid driverId = Guid.Empty;
            var saveInventory = new DataBase.Data.Save.SaveInventoryAoT(db);

            var day = new Day()
            {
                DriverGuid = driverId,
            };
            await Assert.ThrowsAsync<ArgumentNullException>(
                 () => saveInventory.SaveDay(day, driverId.ToByteArray()));
        }
    }
}
