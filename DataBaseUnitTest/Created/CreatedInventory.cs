using Shared.Data;
using DataBase.Model.EntitiesInventory;

using FluentAssertions;
using DataBase.Data;

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

            _db.DataBase.DropTable<Cake>();
            _db.DataBase.DropTable<Day>();
            _db.DataBase.DropTable<Driver>();
            _db.DataBase.DropTable<Product>();
            _db.DataBase.DropTable<ProductName>();
            _db.DataBase.DropTable<ProductPrice>();

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
            var obj = _db.DataBase.GetTableInfo(nameof(Cake));
            var exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(Day));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(Driver));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(Product));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(ProductName));
            exist = obj.Count > 0;
            list.Add(exist);
            obj = _db.DataBase.GetTableInfo(nameof(ProductPrice));
            exist = obj.Count > 0;
            list.Add(exist);

            list.Should().HaveCountGreaterThanOrEqualTo(6);
        }
        [Fact]
        public void CreatedDefaultProductsName()
        {
            var obj = _db.DataBase.Table<ProductName>().ToArray();

            var length = InventoryTables.DefaultProducts.Length;

            obj.Should().HaveCount(length);
        }
        [Fact]
        public void CreatedDefaultProductsPrice()
        {
            var obj = _db.DataBase.Table<ProductPrice>().ToArray();

            var length = InventoryTables.DefaultProducts.Length;

            obj.Should().HaveCount(length);
        }
    }
}
