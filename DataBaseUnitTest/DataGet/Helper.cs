using DataBase.Data;
using DataBase.Model.EntitiesInventory;
using DataBase.Model.EntitiesRoutes;

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
                var time = DateTime.Now.AddDays(rnd.Next(0, 100));
                var user = Guid.CreateVersion7();
                var day = new Day
                {
                    Id = id,
                    SelectedDate = time,
                    Updated = time,
                    Created = time,
                    UserCreatedId = user,
                    DriverGuid = user,
                    UserUpdatedId = user,
                    TotalPriceMoney = rnd.Next(0, 1000),
                    Products = [.. GetExampleProducts(names, prices, id,user)],
                    Cakes = [.. GetExampleCakes(id, rnd.Next(0, 5),user)]
                };

                day.CalculatePrice();
                days.Add(day);

                db.DataBase.Insert(day);
                db.DataBase.InsertAll(day.Products);
                db.DataBase.InsertAll(day.Cakes);
            }
            return days;
        }

        public static IList<Product> GetExampleProducts(IList<ProductName> names, IList<ProductPrice> prices, Guid dayId, Guid userId)
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
                    Number = rnd.Next(0, 40),
                    UserCreatedId = userId,
                    UserUpdatedId = userId
                };
                product.CalculatePrice();
                products.Add(product);
            }
            return products;
        }

        public static IList<Cake> GetExampleCakes(Guid dayId, int n, Guid userId)
        {
            var cakes = new List<Cake>();

            for (int i = 0; i < n; i++)
            {
                var cake = new Cake()
                {
                    DayId = dayId,
                    Id = Guid.CreateVersion7(),
                    PriceDecimal = rnd.Next(0, 50),
                    UserCreatedId=userId,
                    UserUpdatedId=userId,                    
                };
                cakes.Add(cake);
            }
            return cakes;
        }

        public static IList<CustomerRoutes> SetExampleCustomerRoutes(int n, IAccessDataBase db, int sameRouteId = 5)
        {
            var customers = new List<CustomerRoutes>();
            var rnd = new Random();
            var routeId = Guid.CreateVersion7();
            for (int i = 0; i < n; i++)
            {
                var id = Guid.CreateVersion7();
                var time = DateTime.Now.AddDays(rnd.Next(0, 100));
                var user = Guid.CreateVersion7();

                var customer = new CustomerRoutes()
                {
                    Id = id,
                    RoutesId = routeId,
                    Created = time,
                    Updated = time,
                    UserCreatedId = user,
                    UserUpdatedId = user,
                    ResidentialAddress = SetExampleResidentialAddress(id, user, time),
                    DayOfWeek = SetExampleDayOfWeek(id, user, time, rnd)
                };

                customers.Add(customer);
                db.DataBase.Insert(customer);
                db.DataBase.Insert(customer.ResidentialAddress);
                db.DataBase.Insert(customer.DayOfWeek);
                if (i % sameRouteId == 0 && i > 0)
                {
                    routeId = Guid.CreateVersion7();
                }
            }
            return customers;
        }

        public static ResidentialAddress SetExampleResidentialAddress(Guid id, Guid user, DateTime time)
        {
            var obj = new ResidentialAddress()
            {
                CustomerId = id,
                UserCreatedId = user,
                UserUpdatedId = user,
                Created = time,
                Updated = time,
                Id = Guid.CreateVersion7(),
            };
            return obj;
        }
        public static SelectedDayOfWeekRoutes SetExampleDayOfWeek(Guid id, Guid user, DateTime time, Random rnd)
        {
            var obj = new SelectedDayOfWeekRoutes()
            {
                CustomerId = id,
                UserCreatedId = user,
                UserUpdatedId = user,
                Created = time,
                Updated = time,
                Id = Guid.CreateVersion7(),
                Monday = RandomBool(rnd),
                MondayTimeSpan = RandomTime(rnd),
                Thursday = RandomBool(rnd),
                ThursdayTimeSpan = RandomTime(rnd),
                Wednesday = RandomBool(rnd),
                WednesdayTimeSpan = RandomTime(rnd),
                Tuesday = RandomBool(rnd),
                TuesdayTimeSpan = RandomTime(rnd),
                Friday = RandomBool(rnd),
                FridayTimeSpan = RandomTime(rnd),
                Saturday = RandomBool(rnd),
                SaturdayTimeSpan = RandomTime(rnd),
                Sunday = RandomBool(rnd),
                SundayTimeSpan = RandomTime(rnd),
                Optional = RandomBool(rnd),
                SetAll = false,
                SetAllTimeSpan = new TimeSpan()
            };
            return obj;

            static bool RandomBool(Random rnd)
            {
                var next = rnd.Next(0, 10);
                if (next > 5)
                {
                    return true;
                }
                return false;
            }
            static TimeSpan RandomTime(Random rnd)
            {
                TimeSpan time = new(
                    rnd.Next(0, 24),
                    rnd.Next(0, 60),
                    rnd.Next(0, 60));
                return time;
            }
        }
    }
}
