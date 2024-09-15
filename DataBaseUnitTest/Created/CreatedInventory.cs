using DataBase.Data;

using FluentAssertions;

namespace DataBaseUnitTest.Created
{
    public class CreatedInventory : IAsyncLifetime
    {
        private readonly AccessDataBase _db;
        private readonly InventoryTables _inventoryTables;
        public CreatedInventory()
        {
            _db = new AccessDataBase(CreatedDataBaseMain.Path);
            _inventoryTables = new InventoryTables(_db);

        }

        public Task DisposeAsync()
        {

            _db.DataBase.DropTable<DataBase.Model.EntitiesInventory.Cake>();
            _db.DataBase.DropTable<DataBase.Model.EntitiesInventory.Day>();
            _db.DataBase.DropTable<DataBase.Model.EntitiesInventory.Driver>();
            _db.DataBase.DropTable<DataBase.Model.EntitiesInventory.Product>();
            _db.DataBase.DropTable<DataBase.Model.EntitiesInventory.ProductName>();
            _db.DataBase.DropTable<DataBase.Model.EntitiesInventory.ProductPrice>();

            _db.Dispose();
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            await _inventoryTables.Update(0, 1, null);
        }


        [Fact]
        public void CreatedExist()
        {
            List<bool> list = [];
            var obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesInventory.Cake));
            var exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesInventory.Day));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesInventory.Driver));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesInventory.Product));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesInventory.ProductName));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(DataBase.Model.EntitiesInventory.ProductPrice));
            exist = obj.Count > 0;
            list.Add(exist);

            list.Should().HaveCountGreaterThanOrEqualTo(6);
        }
        [Fact]
        public void CreatedDefaultProductsName()
        {
            var obj = _db.DataBase.Table<DataBase.Model.EntitiesInventory.ProductName>().ToArray();

            var length = InventoryTables.DefaultProducts.Length;

            obj.Should().HaveCount(length);
        }
        [Fact]
        public void CreatedDefaultProductsPrice()
        {
            var obj = _db.DataBase.Table<DataBase.Model.EntitiesInventory.ProductPrice>().ToArray();

            var length = InventoryTables.DefaultProducts.Length;

            obj.Should().HaveCount(length);
        }
    }
}
