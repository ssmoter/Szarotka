using DataBase.Model.EntitiesInventory;

using FluentAssertions;

using Shared.Data;

namespace DataBaseUnitTest.Created
{
    public class CreatedInventory
    {

        [Fact]
        public async Task CreatedExist_ShouldCreate()
        {
            var _db = await DataBaseUnitTest.DataGet.Helper.CreateDataBaseForTest(nameof(CreatedExist_ShouldCreate));

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
        public async Task CreatedDefaultProductsName()
        {
            var _db = await DataBaseUnitTest.DataGet.Helper.CreateDataBaseForTest(nameof(CreatedDefaultProductsName));

            var obj = _db.DataBase.Table<ProductName>().ToArray();

            var length = InventoryTables.DefaultProducts.Length;

            obj.Should().HaveCount(length);
        }
        [Fact]
        public async Task CreatedDefaultProductsPrice()
        {
            var _db = await DataBaseUnitTest.DataGet.Helper.CreateDataBaseForTest(nameof(CreatedDefaultProductsPrice));

            var obj = _db.DataBase.Table<ProductPrice>().ToArray();

            var length = InventoryTables.DefaultProducts.Length;

            obj.Should().HaveCount(length);
        }
    }
}
