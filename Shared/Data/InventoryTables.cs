

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Shared.Helper.Img;
using Shared.Model;
using Shared.Service;

namespace Shared.Data
{
    public class InventoryTables(IAccessDataBaseAoT db) : IUpdateDataBase
    {
        readonly IAccessDataBaseAoT _db = db;
        public static Guid GetGuidSed(string guid)
        {
            return new Guid(guid);
        }


        public async Task Update(int oldVersion, int newVersion, Action<double, int> updateInventory)
        {
            double progressBar = 0;
            double updateProgressBar = newVersion - oldVersion;
            updateProgressBar = 1 / updateProgressBar;


            if (oldVersion < 1)
            {
                await CreateInventoryTables();
#if DEBUG
                await CreatedDefaultProduct();
#endif
                progressBar += updateProgressBar;
                oldVersion = 1;
                updateInventory?.Invoke(progressBar, oldVersion);
            }
            if (oldVersion < 2)
            {
                progressBar += updateProgressBar;
                oldVersion = 2;
                updateInventory?.Invoke(progressBar, oldVersion);
            }
            if (oldVersion < 3)
            {
                await CopyDriverGuidToUserCreatedIdAndUserUpdateId();
                progressBar += updateProgressBar;
                oldVersion = 3;
                updateInventory?.Invoke(progressBar, oldVersion);
            }


            updateInventory?.Invoke(1, oldVersion);
        }

        private async Task CopyDriverGuidToUserCreatedIdAndUserUpdateId()
        {
            var result = CreatedDataBase.GetTableInfo(_db, nameof(Day));

            if (result.Any(row => row.Name == "DriverGuid"))
            {
                await _db.DbAsyncAoT.ExecuteAsync(@"
                                            UPDATE Day
                                            SET 
                                            UserCreatedId = DriverGuid,
                                            UserUpdatedId = DriverGuid
                                            ");
            }
        }



        private async Task CreateInventoryTables()
        {
            var name = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedProductName);
            var price = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedProductPrice);
            var product = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedProduct);
            var cake = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedCake);
            var day = _db.DbAsyncAoT.ExecuteAsync(SQlCreatedDay);

            await Task.WhenAll(name, price, product, cake, day);
        }

        private async Task CreatedDefaultProduct()
        {
            var products = DefaultProducts;
            var taskTableName = new Task[products.Length];
            var taskTablePrice = new Task[products.Length];

            for (int i = 0; i < products.Length; i++)
            {
                var (name, price) = TaskProduct(products[i], i + 1);

                taskTableName[i] = name;
                taskTablePrice[i] = price;
            }
            await Task.WhenAll(taskTableName);
            await Task.WhenAll(taskTablePrice);
        }

        private (Task name, Task price) TaskProduct(Product product, int Arrangement)
        {
            product.Name.Arrangement = Arrangement;
            product.Price.ProductNameId = product.Name.Id;

            (Task name, Task price) task = new()
            {
                name = _db.DbAsyncAoT.ExecuteAsync(DataBase.Data.SqlQuery.ProductNameQuery.SaveOrUpdate(product.Name.Id,
                                                                                                        product.Name.Arrangement,
                                                                                                        product.Name.Name,
                                                                                                        product.Name.Description,
                                                                                                        product.Name.Img,
                                                                                                        product.Name.IsVisible,
                                                                                                        product.Name.CreatedTicks,
                                                                                                        product.Name.UpdatedTicks,
                                                                                                        product.Name.UserCreatedId,
                                                                                                        product.Name.UserUpdatedId,
                                                                                                        product.Name.IsDelete), new()
                                                                                                        {
                                                                                                            [nameof(product.Name.Id)] = product.Name.Id,
                                                                                                            [nameof(product.Name.Arrangement)] = product.Name.Arrangement,
                                                                                                            [nameof(product.Name.Name)] = product.Name.Name,
                                                                                                            [nameof(product.Name.Description)] = product.Name.Description,
                                                                                                            [nameof(product.Name.Img)] = product.Name.Img,
                                                                                                            [nameof(product.Name.IsVisible)] = product.Name.IsVisible,
                                                                                                            [nameof(product.Name.CreatedTicks)] = product.Name.CreatedTicks,
                                                                                                            [nameof(product.Name.UpdatedTicks)] = product.Name.UpdatedTicks,
                                                                                                            [nameof(product.Name.UserCreatedId)] = product.Name.UserCreatedId,
                                                                                                            [nameof(product.Name.UserUpdatedId)] = product.Name.UserUpdatedId,
                                                                                                            [nameof(product.Name.IsDelete)] = product.Name.IsDelete
                                                                                                        }),
                price = _db.DbAsyncAoT.ExecuteAsync(DataBase.Data.SqlQuery.ProductPriceQuery.SaveOrUpdate(product.Price.Id,
                                                                                                          product.Price.Price,
                                                                                                          product.Price.CreatedTicks,
                                                                                                          product.Price.UpdatedTicks,
                                                                                                          product.Price.UserCreatedId,
                                                                                                          product.Price.UserUpdatedId,
                                                                                                          product.Price.ProductNameId,
                                                                                                          product.Price.IsDelete), new()
                                                                                                          {
                                                                                                              [nameof(product.Price.Id)] = product.Price.Id,
                                                                                                              [nameof(product.Price.Price)] = product.Price.Price,
                                                                                                              [nameof(product.Price.CreatedTicks)] = product.Price.CreatedTicks,
                                                                                                              [nameof(product.Price.UpdatedTicks)] = product.Price.UpdatedTicks,
                                                                                                              [nameof(product.Price.UserCreatedId)] = product.Price.UserCreatedId,
                                                                                                              [nameof(product.Price.UserUpdatedId)] = product.Price.UserUpdatedId,
                                                                                                              [nameof(product.Price.ProductNameId)] = product.Price.ProductNameId,
                                                                                                              [nameof(product.Price.IsDelete)] = product.Price.IsDelete
                                                                                                          })
            };
            return task;
        }

        const string SQlCreatedDay = $@"CREATE TABLE IF NOT EXISTS [{nameof(Day)}] (
            [{nameof(Day.Id)}] TEXT PRIMARY KEY,
            [{nameof(Day.Description)}] TEXT,
            [{nameof(Day.DriverGuid)}] TEXT,
            [{nameof(Day.SelectedDateString)}] TEXT,
            [{nameof(Day.SelectedDateTicks)}] INTEGER,
            [{nameof(Day.TotalPriceProducts)}] INTEGER,
            [{nameof(Day.TotalPriceCake)}] INTEGER,
            [{nameof(Day.TotalPrice)}] INTEGER,
            [{nameof(Day.TotalPriceCorrect)}] INTEGER,
            [{nameof(Day.TotalPriceAfterCorrect)}] INTEGER,
            [{nameof(Day.TotalPriceMoney)}] INTEGER,
            [{nameof(Day.TotalPriceDifference)}] INTEGER,
            {HelperTable.AdditionalColumns})";

        const string SQlCreatedProductName = $@"CREATE TABLE IF NOT EXISTS [{nameof(ProductName)}] (
            [{nameof(ProductName.Id)}] TEXT PRIMARY KEY,
            [{nameof(ProductName.Arrangement)}] INTEGER,
            [{nameof(ProductName.Name)}] TEXT,
            [{nameof(ProductName.Description)}] TEXT,
            [{nameof(ProductName.Img)}] TEXT,
            [{nameof(ProductName.IsVisible)}] INTEGER,
            {HelperTable.AdditionalColumns})";

        const string SQlCreatedProductPrice = $@"CREATE TABLE IF NOT EXISTS [{nameof(ProductPrice)}] (
            [{nameof(ProductPrice.Id)}] TEXT PRIMARY KEY,
            [{nameof(ProductPrice.ProductNameId)}] TEXT,
            [{nameof(ProductPrice.Price)}] INTEGER,
            {HelperTable.AdditionalColumns})";


        const string SQlCreatedProduct = $@"CREATE TABLE IF NOT EXISTS [{nameof(Product)}] (
            [{nameof(Product.Id)}] TEXT PRIMARY KEY,
            [{nameof(Product.DayId)}] TEXT,
            [{nameof(Product.ProductNameId)}] TEXT,
            [{nameof(Product.ProductPriceId)}] TEXT,
            [{nameof(Product.Description)}] TEXT,
            [{nameof(Product.PriceTotal)}] INTEGER,
            [{nameof(Product.PriceTotalCorrect)}] INTEGER,
            [{nameof(Product.PriceTotalAfterCorrect)}] INTEGER,
            [{nameof(Product.Number)}] INTEGER,
            [{nameof(Product.NumberEdit)}] INTEGER,
            [{nameof(Product.NumberReturn)}] INTEGER,
            {HelperTable.AdditionalColumns})";

        const string SQlCreatedCake = $@"CREATE TABLE IF NOT EXISTS [{nameof(Cake)}] (
            [{nameof(Cake.Id)}] TEXT PRIMARY KEY,
            [{nameof(Cake.DayId)}] TEXT,
            [{nameof(Cake.IsSell)}] INTEGER,
            [{nameof(Cake.Price)}] INTEGER,
            {HelperTable.AdditionalColumns})";

        public static Product[] DefaultProducts { get => defaultProducts; }
        private static readonly Product[] defaultProducts =
            [
                    new()
                    {
                      Name = new ProductName(){ Name ="Chleb",Img=ImgBread.chleb,Id = GetGuidSed("d2034a6a-3a0d-9b49-05a3-bc49adfc329a"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=6m,Id = GetGuidSed("8012d48e-5f9b-439f-a213-ed2e997b9432"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Duży chleb",Img="chleb.png", Id = GetGuidSed("24ebc827-bb4e-6fac-4eb4-8f88f524eaf1"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=12m,Id = GetGuidSed("3200c8fa-be20-449b-9a82-84ff2e9598b5"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Bułka",Img=ImgBread.Bulka,Id = GetGuidSed("e5226138-0e10-9f4c-cd7b-b91b87ef9f24"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=1.5m,Id = GetGuidSed("c884e0af-680a-aa9f-2398-50e032390792"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Mała bułka",Img=ImgBread.Bulka,Id = GetGuidSed("944f2566-0502-fae1-39f1-3540c389cbe4"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=0.7m,Id = GetGuidSed("87c365cd-8536-cf5f-c177-c177bd6c6d94"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Bułka z serem",Img=ImgBread.Bulka,Id = GetGuidSed("61b42773-0b9f-aa21-fe8a-8e19335fe799"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed("cd126ccd-8075-e15b-c580-d28b43693ccd"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Drożdżówka",Img=ImgBuns.Ser,Id = GetGuidSed("9bcca084-69d5-c245-4319-88acf6469c62"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed("51aa278f-a059-f31e-cbd3-9b1f39966694"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Bułka maślana",Img=ImgPath.Logo,Id = GetGuidSed("9295c14d-4b92-63c4-2650-895cf7afea7f"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=1.5m,Id = GetGuidSed("1bfb70e3-eabf-e373-82d9-d001cd2689ac"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Drożdżówki na wagę (opak.)",Img=ImgBuns.Ser,Id = GetGuidSed("345ef3b2-0ab1-44d0-6347-2b0916c1fc1c"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=9m,Id = GetGuidSed("c93c1ce9-4b9b-62d5-4eb1-0fb97bc39500"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Kołacz",Img=ImgBuns.Ser,Id = GetGuidSed("03aaddc6-b550-d8cd-9fb7-b878bf69cdc0"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=5m,Id = GetGuidSed("b118b4dc-c0ea-6710-1efb-e600fb17c8a0"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Chałka",Img=ImgPath.Logo,Id = GetGuidSed("38422bb1-f816-33cb-4e63-29e4925da2e4"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=5m,Id = GetGuidSed("c7dc4ee3-e4aa-0615-cc43-e554cfcddd88"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Ciastka francuskie",Img=ImgOther.KrucheZSerem,Id = GetGuidSed("94c617ee-9426-e4a9-e8e4-6286962cd4c1"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3m,Id = GetGuidSed("ac1c0158-013b-cdf5-153b-6cd313766481"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Ciastka (opak. 400g)",Img=ImgCookies.Czekolada,Id = GetGuidSed("7af5a086-0e96-4fbd-a4f8-dec5818e70a0"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=10m,Id = GetGuidSed("cdfb236f-30b1-ec4d-22f8-7fea28d6d072"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Piernik/Babka",Img=ImgPath.Logo,Id = GetGuidSed("f500e080-5936-4a2a-43cd-074323304b86"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=12m,Id = GetGuidSed("ef63cbcb-977a-55d0-5309-589e8fd226ce"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Makowiec",Img=ImgPath.Logo,Id = GetGuidSed("a1456c27-7a8e-1669-f9b1-89e3290ac9f2"),Created=DateTime.Now,Updated=DateTime.Now, UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=14m,Id = GetGuidSed("5c1ff8ac-e072-e9f7-10aa-a22d008bbb70"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Wafle (opak. 400g)",Img=ImgCookies.Andrut,Id = GetGuidSed("b71b38a6-aaeb-51f7-624d-f6c90dec2c49"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=12m,Id = GetGuidSed("d289404c-2bcf-4b01-86d0-39721960fda5"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Bułka tarta",Img=ImgPath.Logo,Id = GetGuidSed("f5d38837-3bae-2d5b-5ff0-95e6cf365d46"),Created=DateTime.Now,Updated=DateTime.Now,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3.5m,Id = GetGuidSed("7f656b80-7b4e-d1d3-5b60-131acf77e6ed"),UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Parówka w cieście",Img=ImgPath.Logo,Id = GetGuidSed("60edd453-e65a-4375-fdc0-3aa98fd60da6"),Created=DateTime.Now,Updated=DateTime.Now, UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3.5m,Id = GetGuidSed("af0ffb76-fbad-bcd6-b502-994e27405de7"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Chleb suchy",Img="chleb.png", Id = GetGuidSed("fa93a809-fc49-99ec-f522-818e412a4184"),Created=DateTime.Now,Updated=DateTime.Now, UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed("8bbf1b67-2d21-4ecf-9599-b39bca9a4187"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Mini Pizza",Img=ImgPath.Logo, Id = GetGuidSed("f03d18f5-12ee-0106-00cd-fa25e92ddb24"),Created=DateTime.Now,Updated=DateTime.Now, UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3m,Id = GetGuidSed("99ed587e-2499-dcbd-ddd3-428f1a137a25"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Precel",Img=ImgPath.Logo, Id = GetGuidSed("4024c248-2c8e-4fcf-ac83-c3d3059b4322"),Created=DateTime.Now,Updated=DateTime.Now, UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=2.5m,Id = GetGuidSed("9956ad34-69ea-4bf1-8d32-4581c4f6c264"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Babka duża",Img=ImgPath.Logo, Id = GetGuidSed("e1c7d127-8f86-49e4-bbdf-26b035d893f6"),Created=DateTime.Now,Updated=DateTime.Now,IsVisible=false,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=12m,Id = GetGuidSed("323ecff4-5b51-4218-9a0d-0dac28a94fce"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Babka średnia",Img=ImgPath.Logo, Id = GetGuidSed("7112cea4-6f83-4cfe-9175-d425c94d1651"),Created=DateTime.Now,Updated=DateTime.Now,IsVisible=false,UserCreatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=8m,Id = GetGuidSed("a3db562d-2943-4873-a94d-9c870509daaa"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Babka Mała",Img=ImgPath.Logo, Id = GetGuidSed("1d793cea-742c-4b6a-ad4a-cb567a64bf1a"),Created=DateTime.Now,Updated=DateTime.Now,IsVisible=false, UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3m,Id = GetGuidSed("2bad3bff-1f81-4ced-8215-d97c2bfebbae"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Baranek",Img=ImgPath.Logo, Id = GetGuidSed("60acb83d-3141-4f7d-9d15-714ad357a88d"),Created=DateTime.Now,Updated=DateTime.Now,IsVisible=false, UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3m,Id = GetGuidSed("440c664c-3fb3-48b4-bd94-6e0458910f80"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
                    new()
                    {
                      Name = new ProductName(){ Name ="Chleb do świecenia",Img=ImgPath.Logo, Id = GetGuidSed("3af7e393-b6af-4015-b5f7-c56d85f375a7"),Created=DateTime.Now,Updated=DateTime.Now,IsVisible=false, UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                      Price = new ProductPrice(){Created=DateTime.Now,Updated=DateTime.Now,PriceDecimal=3.5m,Id = GetGuidSed("5183158d-dea2-4f16-a227-647554417656"), UserCreatedId = GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8"),UserUpdatedId=GetGuidSed("ae86aaeb-1080-4ae2-9226-62cf3a042ca8")},
                    },
            ];

    }
}
