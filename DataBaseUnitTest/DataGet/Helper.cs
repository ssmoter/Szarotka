using DataBase.Data;
using DataBase.Model.EntitiesInventory;

namespace DataBaseUnitTest.DataGet
{
    public static class Helper
    {
        public static void Delete(string dbName) => DataSave.Helper.Delete(dbName);
        private static Random rnd = new();

        public static async Task<IAccessDataBase> CreateDataBaseForTest(string dbName)
        {
            var db = DataSave.Helper.GetPath + dbName + ".db3";
            Delete(db);
            return await DataSave.Helper.CreateDataBaseInventoryForTest(db);
        }


        public static async Task<IList<Day>> SetExampleDays(int n, IAccessDataBase db)
        {
            var days = new List<Day>();

            var names = await db.DataBaseAsync.Table<ProductName>().ToArrayAsync();
            var prices = await db.DataBaseAsync.Table<ProductPrice>().ToArrayAsync();
            var rnd = new Random();
            for (int i = 0; i < n; i++)
            {
                var id = Guid.CreateVersion7();
                var time = DateTime.Now.AddDays(rnd.Next(0,100));
                var user = Guid.CreateVersion7();
                var day = new Day
                {
                    Id = id,
                    SelectedDate = time,
                    Updated = time,
                    Created = time,
                    UserCreatedId = user,
                    UserUpdatedId = user,
                    TotalPriceMoney = rnd.Next(0, 1000),
                    Products = [.. GetExampleProducts(names, prices, id)],
                    Cakes = [.. GetExampleCakes(id, rnd.Next(0, 5))]
                };

                day.UpdateTotalPrice();
                days.Add(day);

                db.DataBase.Insert(day);
                db.DataBase.InsertAll(day.Products);
                db.DataBase.InsertAll(day.Cakes);
            }


            return days;
        }

        public static IList<Product> GetExampleProducts(IList<ProductName> names, IList<ProductPrice> prices, Guid dayId)
        {
            var products = new List<Product>();

            for (int i = 0; i < names.Count; i++)
            {
                var price = prices.FirstOrDefault(x => x.ProductNameId == names[i].Id);
                if (price is null)
                {
                    continue;
                }
                names[i].IsVisible = true;
                var product = new Product()
                {
                    Id = Guid.CreateVersion7(),
                    DayId = dayId,
                    Name = names[i],
                    ProductNameId = names[i].Id,
                    Price = price,
                    ProductPriceId = price.Id,
                    Number = rnd.Next(0, 40)
                };
                product.CalculatePrice();
                products.Add(product);
            }
            return products;
        }

        public static IList<Cake> GetExampleCakes(Guid dayId, int n)
        {
            var cakes = new List<Cake>();

            for (int i = 0; i < n; i++)
            {
                var cake = new Cake()
                {
                    DayId = dayId,
                    Id = Guid.CreateVersion7(),
                    PriceDecimal = rnd.Next(0, 50)
                };
                cakes.Add(cake);
            }
            return cakes;
        }

    }
}
