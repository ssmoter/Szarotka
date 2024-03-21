using CommunityToolkit.Maui.Alerts;

using DataBase.Data;
using DataBase.Model.EntitiesInventory;

using Inventory.Helper;
using Inventory.Helper.Parse;
using Inventory.Model.MVVM;
using Inventory.Service;

using System.Collections.ObjectModel;

namespace Inventory.Data
{
    public class SelectDayService : ISelectDayService
    {
        readonly AccessDataBase _db;

        public SelectDayService(AccessDataBase db)
        {
            _db = db;
        }

        #region CodeFirst

        public async Task<DayM> GetDay(DateTime createdDate)
        {
            try
            {
                await CommunityToolkit.Maui.Alerts.Toast.Make("Pobieranie danego dnia", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
                var DayM = new DayM
                {
                    CanUpadte = false
                };
                var guid = new Guid(Helper.SelectedDriver.Id);
                var createdString = createdDate.ToString("dd.MM.yyyy");
                var today = await _db.DataBaseAsync.Table<Day>().Where(x => x.SelectedDateString == createdString && x.DriverGuid == guid).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                DayM.Products = await GetProductTable(DayM);
                DayM.Cakes = await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = createdDate;
                }
                if (DayM.DriverGuid == Guid.Empty)
                {
                    DayM.DriverGuid = guid;
                }

                DayM.CanUpadte = true;
                return DayM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DayM> GetDay(string createdDate)
        {
            try
            {
                await CommunityToolkit.Maui.Alerts.Toast.Make("Pobieranie danego dnia", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();

                var DayM = new DayM
                {
                    CanUpadte = false
                };
                var guid = new Guid(Helper.SelectedDriver.Id);
                var today = await _db.DataBaseAsync.Table<Day>().Where(x => x.SelectedDateString == createdDate && x.DriverGuid == guid).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                DayM.Products = await GetProductTable(DayM);
                DayM.Cakes = await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = DateTime.Parse(createdDate, DataBase.Helper.Constants.CultureInfo);
                    DayM.DriverGuid = guid;
                }
                if (DayM.DriverGuid == Guid.Empty)
                {
                    DayM.DriverGuid = guid;
                }
                DayM.CanUpadte = true;
                return DayM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DayM> GetDay(Guid id)
        {
            try
            {
                await CommunityToolkit.Maui.Alerts.Toast.Make("Pobieranie danego dnia", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();

                var DayM = new DayM
                {
                    CanUpadte = false
                };
                var today = await _db.DataBaseAsync.Table<Day>().Where(x => x.Id == id).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                DayM.Products = await GetProductTable(DayM);
                DayM.Cakes = await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = DateTime.Now;
                }
                if (DayM.DriverGuid == Guid.Empty)
                {
                    DayM.DriverGuid = new Guid(Helper.SelectedDriver.Id);
                }
                DayM.CanUpadte = true;
                return DayM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DayM> GetDay()
        {
            try
            {
                await CommunityToolkit.Maui.Alerts.Toast.Make("Pobieranie danego dnia", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();

                var DayM = new DayM
                {
                    CanUpadte = false
                };
                var time = DateTime.Now.ToString("dd.MM.yyyy");
                var guid = new Guid(Helper.SelectedDriver.Id);
                var today = await _db.DataBaseAsync.Table<Day>().Where(x => x.SelectedDateString == time && x.DriverGuid == guid).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                DayM.Products = await GetProductTable(DayM);
                DayM.Cakes = await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = DateTime.Now;
                    DayM.DriverGuid = guid;
                }
                if (DayM.DriverGuid == Guid.Empty)
                {
                    var a = DayM.DriverGuid.ToString();

                    DayM.DriverGuid = new Guid(Helper.SelectedDriver.Id);
                }
                DayM.CanUpadte = true;
                return DayM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<ProductM>> GetProductTable(DayM dayM)
        {
            var product = await _db.DataBaseAsync.Table<Product>().Where(x => x.DayId == dayM.Id).ToArrayAsync();
            dayM.Products.Clear();

            for (int i = 0; i < product.Length; i++)
            {
                var productNameId = product[i].ProductNameId;
                product[i].Name = await _db.DataBaseAsync.Table<ProductName>().FirstOrDefaultAsync(x => x.Id == productNameId);
                var priceId = product[i].ProductPriceId;
                product[i].Price = await _db.DataBaseAsync.Table<ProductPrice>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.ProductNameId == productNameId && x.Id == priceId);
                dayM.Products.Add(product[i].ParseAsProductM());
            }
            var length = await _db.DataBaseAsync.Table<ProductName>().CountAsync();

            if (product.Length < length)
            {
                var allProduct = await _db.DataBaseAsync.Table<ProductName>().ToArrayAsync();

                for (int i = 0; i < length; i++)
                {
                    var id = allProduct[i].Id;
                    if (dayM.Products.Any(x => x.Name.Id == id))
                    {
                        continue;
                    }
                    else
                    {
                        var price = await _db.DataBaseAsync.Table<ProductPrice>().OrderByDescending(x => x.Created).FirstOrDefaultAsync(x => x.ProductNameId == id);
                        if (price is null)
                        {
                            continue;
                        }
                        dayM.Products.Add(new ProductM()
                        {
                            Name = allProduct[i].PareseAsProductNameM(),
                            Price = price.PareseAsProductPriceM(),
                            ProductNameId = id,
                            ProductPriceId = price.Id,
                            CanUpadte = true,
                        });
                    }
                }
            }

            if (dayM.Products.Count == 0)
            {
                var productName = await _db.DataBaseAsync.Table<ProductName>().ToArrayAsync();
                for (int i = 0; i < productName.Length; i++)
                {
                    var nameId = productName[i].Id;
                    var price = await _db.DataBaseAsync.Table<ProductPrice>().OrderByDescending(x => x.Created).FirstOrDefaultAsync(x => x.ProductNameId == nameId);
                    if (price is null)
                    {
                        continue;
                    }
                    dayM.Products.Add(new ProductM()
                    {
                        Name = productName[i].PareseAsProductNameM(),
                        ProductNameId = productName[i].Id,
                        Price = price.PareseAsProductPriceM(),
                        ProductPriceId = price.Id,
                        CanUpadte = true,

                    });
                }
            }

            return dayM.Products;
        }
        public async Task<ObservableCollection<CakeM>> GetCakeTable(DayM dayM)
        {
            var cake = await _db.DataBaseAsync.Table<Cake>().Where(x => x.DayId == dayM.Id).ToArrayAsync();
            dayM.Cakes.Clear();
            for (int i = 0; i < cake.Length; i++)
            {
                dayM.Cakes.Add(cake[i].PareseAsCakeM());
            }
            return dayM.Cakes;
        }

        #endregion


        #region Procedure

        public async Task<DayM> GetDayProcedure(DateTime createdDate)
        {
            var id = await DateFindId(createdDate);
            if (id != Guid.Empty)
            {
                var queryId = StoredProcedure.GetSingleDay(id);
                var dayId = await GetDateProcedureLogic(queryId);
                return dayId;
            }

            var query = StoredProcedure.GetSingleDay(createdDate.ToShortDateString(), Helper.SelectedDriver.Id);
            var day = await GetDateProcedureLogic(query);
            if (day.Id == Guid.Empty)
            {
                day.Id = new();
                day.Created = createdDate;
                if (day.Created.Hour == 0)                
                    day.Created = new DateTime(createdDate.Year, createdDate.Month, createdDate.Day, 12, 0, 0);
                                
            }
            return day;
        }
        public async Task<DayM> GetDayProcedure(Guid id)
        {
            var query = StoredProcedure.GetSingleDay(id);
            var day = await GetDateProcedureLogic(query);
            if (day.Id == Guid.Empty)
            {
                day.Id = new();
                day.Created = DateTime.Now;
            }
            return day;
        }

        //public async Task<DayM> GetDaysProcedure(long from, long to, Guid[] selectedDriverName, bool moreData)
        //{


        //}


        async Task<DayM> GetDateProcedureLogic(string query)
        {
            var response = await _db.DataBaseAsync.FindWithQueryAsync<GetDayM>(query);
            Day dayM = response;
            if (response is not null)
            {
                var product = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(response.ProductsJson);

                dayM.Products = product;
                dayM.Cakes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cake>>(response.CakesJson);
            }
            else
            {
                dayM ??= new();
                var products = await _db.DataBaseAsync.QueryAsync<GetProductNameM>(StoredProcedure.GetAllProductsNameAndPrice());
                for (int i = 0; i < products.Count; i++)
                {
                    var price = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductPrice>(products[i].JsonPrice);
                    dayM.Products.Add(new Product()
                    {
                        Name = products[i],
                        Price = price,
                        DayId = dayM.Id,
                        ProductNameId = products[i].Id,
                        ProductPriceId = price.Id,
                    });
                }


                return dayM.ParseAsDayM();
            }
            var allOfProducts = await _db.DataBaseAsync.QueryAsync<GetProductNameM>(StoredProcedure.GetAllProductsNameAndPrice());

            if (dayM.Products.Count != allOfProducts.Count)
            {
                for (int i = 0; i < allOfProducts.Count; i++)
                {
                    if (dayM.Products.Any(x => x.Name.Id == allOfProducts[i].Id))
                        continue;

                    var price = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductPrice>(allOfProducts[i].JsonPrice);
                    dayM.Products.Add(new Product()
                    {
                        Name = allOfProducts[i],
                        Price = price,
                        DayId = dayM.Id,
                        ProductNameId = allOfProducts[i].Id,
                        ProductPriceId = price.Id,
                    });
                }
            }

            return dayM.ParseAsDayM();
        }

        async Task<Guid> DateFindId(DateTime createdDate)
        {
            var guid = new Guid(Helper.SelectedDriver.Id);
            var createdString = createdDate.ToString("dd.MM.yyyy");
            var id = await _db.DataBaseAsync.Table<Day>().FirstOrDefaultAsync(x => x.SelectedDateString == createdString && x.DriverGuid == guid);
            if (id != null)
                return id.Id;
            return Guid.Empty;
        }


        class GetDayM : Day
        {
            public string ProductsJson { get; set; }
            public string CakesJson { get; set; }
        }
        class GetProductNameM : ProductName
        {
            public string JsonPrice { get; set; }
        }

        #endregion

    }
}
