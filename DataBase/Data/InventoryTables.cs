using DataBase.Helper.Img;
using DataBase.Model.EntitiesInventory;

namespace DataBase.Data
{
    public class InventoryTables(AccessDataBase db)
    {
        readonly AccessDataBase _db = db;
        static readonly Random _random = new(2137);

        static Guid GetGuidSed()
        {
            byte[] guidBytes = new byte[16];
            _random.NextBytes(guidBytes);
            return new Guid(guidBytes);
        }

        public async Task UpdateInventory(int oldVersion, int newVersion, Action<double, int> uppdateInventory)
        {
            double progressBar = 0;
            double uppdateProgressBar = newVersion - oldVersion;
            uppdateProgressBar = 1 / uppdateProgressBar;


            if (oldVersion < 1)
            {
                await CreateInventoryTables();
                progressBar += uppdateProgressBar;
                oldVersion = 1;
                uppdateInventory?.Invoke(progressBar, oldVersion);
            }




            uppdateInventory?.Invoke(1, oldVersion);
        }

        private async Task CreateInventoryTables()
        {
            var driver = _db.DataBaseAsync.CreateTableAsync<Driver>();
            var selectedDriver = _db.DataBaseAsync.CreateTableAsync<SelectedDriver>();
            var name = _db.DataBaseAsync.CreateTableAsync<ProductName>();
            var price = _db.DataBaseAsync.CreateTableAsync<ProductPrice>();
            var product = _db.DataBaseAsync.CreateTableAsync<Product>();
            var cake = _db.DataBaseAsync.CreateTableAsync<Cake>();
            var day = _db.DataBaseAsync.CreateTableAsync<Day>();

            await Task.WhenAll(driver, selectedDriver, name, price, product, cake, day);

            await CreateDefaultProduct();
        }

        private async Task CreateDefaultProduct()
        {
            var products = DefaultProducts;
            var taskTableName = new Task[products.Length];
            var taskTablePrice = new Task[products.Length];

            for (int i = 0; i < products.Length; i++)
            {
                var (name, price) = TaskProduct(products[i], i);
                taskTableName[i] = name;
                taskTablePrice[i] = price;
            }
            await Task.WhenAll(taskTableName);
            await Task.WhenAll(taskTablePrice);


            //for (int i = 0; i < products.Length; i++)
            //{
            //    products[i].Name.Arrangement = i;
            //    var id = _db.DataBaseAsync.InsertAsync(products[i].Name);
            //    var name = products[i].Name.Name;
            //    products[i].Name = _db.DataBase.Table<ProductName>().FirstOrDefault(x => x.Name == name);
            //    products[i].Price.ProductNameId = products[i].Name.Id;
            //    _db.DataBase.Insert(products[i].Price);
            //}
        }

        private (Task name, Task price) TaskProduct(Product product, int Arrangement)
        {
            product.Name.Arrangement = Arrangement;
            product.Price.ProductNameId = product.Name.Id;
            (Task name, Task price) task = new()
            {
                name = _db.DataBaseAsync.InsertOrReplaceAsync(product.Name),
                price = _db.DataBaseAsync.InsertOrReplaceAsync(product.Price)
            };
            return task;
        }

        #region Products

        public static Product[] DefaultProducts { get => defoultProducts; }
        private static readonly Product[] defoultProducts =
            [
                    new()
                    {
                      Name = new ProductName(){ Name ="Chleb",Img=ImgBread.chleb,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=6m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Duży chleb",Img="chleb.png", Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=12m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Bułka",Img=ImgBread.Bulka,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=1.5m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Mała bułka",Img=ImgBread.Bulka,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=0.7m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Bułka z serem",Img=ImgBread.Bulka,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Drożdżówka",Img=ImgBuns.Ser,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Bułka maślana",Img=ImgPath.Logo,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=1.5m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Drożdżówki na wagę (opak.)",Img=ImgBuns.Ser,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=9m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Kołacz",Img=ImgBuns.Ser,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=5m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Chałka",Img=ImgPath.Logo,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=5m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Ciastka francuskie",Img=ImgOther.KrucheZSerem,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Ciastka (opak. 400g)",Img=ImgCookies.Czekolada,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=10m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Piernik/Babka",Img=ImgPath.Logo,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=12m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Makowiec",Img=ImgPath.Logo,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=14m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Wafle (opak. 400g)",Img=ImgCookies.Andrut,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=12m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Bułka tarta",Img=ImgPath.Logo,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3.5m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Parówka w cieście",Img=ImgPath.Logo,Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3.5m,Id = GetGuidSed()},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Chleb suchy",Img="chleb.png", Id = GetGuidSed(),Created=DateTime.Now,Updated=DateTime.Now},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed()},
                    },
            ];
        #endregion
    }
}
