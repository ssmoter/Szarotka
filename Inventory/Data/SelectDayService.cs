using DataBase.Data;

using Inventory.Helper;
using Inventory.Helper.Parse;
using Inventory.Model.MVVM;

using System.Collections.ObjectModel;

namespace Inventory.Service
{
    public class SelectDayService : ISelectDayService
    {
        readonly DataBase.Data.AccessDataBase _db;

        public SelectDayService(AccessDataBase db)
        {
            _db = db;
        }

        public async Task<DayM> GetDay(DateTime createdDate)
        {
            try
            {
                await SnackbarAsToats.OnShow("Pobieranie danego dnia ");
                var DayM = new DayM
                {
                    CanUpadte = false
                };
                var guid = Helper.SelectedDriver.Id;
                var createdString = createdDate.ToString("dd.MM.yyyy");
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.CreatedDate == createdString && x.DriverGuid == guid).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                DayM.Products = await GetProductTable(DayM);
                DayM.Cakes = await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = createdDate;
                }
                if (DayM.DriverGuid == Guid.Empty)
                {
                    DayM.DriverGuid = Helper.SelectedDriver.Id;
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
                await SnackbarAsToats.OnShow("Pobieranie danego dnia ");
                var DayM = new DayM
                {
                    CanUpadte = false
                };
                var guid = Helper.SelectedDriver.Id;
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.CreatedDate == createdDate && x.DriverGuid == guid).FirstOrDefaultAsync();
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
                    DayM.DriverGuid = Helper.SelectedDriver.Id;
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
                await SnackbarAsToats.OnShow("Pobieranie danego dnia ");
                var DayM = new DayM
                {
                    CanUpadte = false
                };
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.Id == id).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                DayM.Products = await GetProductTable(DayM);
                DayM.Cakes = await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = DateTime.Now;
                }
                if (DayM.DriverGuid == Guid.Empty)
                {
                    DayM.DriverGuid = Helper.SelectedDriver.Id;
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
                await SnackbarAsToats.OnShow("Pobieranie danego dnia ");
                var DayM = new DayM
                {
                    CanUpadte = false
                };
                var time = DateTime.Now.ToString("dd.MM.yyyy");
                var guid = Helper.SelectedDriver.Id;
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.CreatedDate == time && x.DriverGuid == guid).FirstOrDefaultAsync();
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

                    DayM.DriverGuid = Helper.SelectedDriver.Id;
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
            var product = await _db.DataBaseAsync.Table<Model.Product>().Where(x => x.DayId == dayM.Id).ToArrayAsync();
            dayM.Products.Clear();

            for (int i = 0; i < product.Length; i++)
            {
                var productNameId = product[i].ProductNameId;
                product[i].Name = await _db.DataBaseAsync.Table<Model.ProductName>().FirstOrDefaultAsync(x => x.Id == productNameId);
                var priceId = product[i].ProductPriceId;
                product[i].Price = await _db.DataBaseAsync.Table<Model.ProductPrice>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.ProductNameId == productNameId && x.Id == priceId);
                dayM.Products.Add(product[i].ParseAsProductM());
            }
            var length = await _db.DataBaseAsync.Table<Model.ProductName>().CountAsync();

            if (product.Length < length)
            {
                var allProduct = await _db.DataBaseAsync.Table<Model.ProductName>().ToArrayAsync();

                for (int i = 0; i < length; i++)
                {
                    var id = allProduct[i].Id;
                    if (dayM.Products.Any(x => x.Name.Id == id))
                    {
                        continue;
                    }
                    else
                    {
                        var price = await _db.DataBaseAsync.Table<Model.ProductPrice>().OrderByDescending(x => x.Created).FirstOrDefaultAsync(x => x.ProductNameId == id);
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
                var productName = await _db.DataBaseAsync.Table<Model.ProductName>().ToArrayAsync();
                for (int i = 0; i < productName.Length; i++)
                {
                    var nameId = productName[i].Id;
                    var price = await _db.DataBaseAsync.Table<Model.ProductPrice>().OrderByDescending(x => x.Created).FirstOrDefaultAsync(x => x.ProductNameId == nameId);
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
            var cake = await _db.DataBaseAsync.Table<Model.Cake>().Where(x => x.DayId == dayM.Id).ToArrayAsync();
            dayM.Cakes.Clear();
            for (int i = 0; i < cake.Length; i++)
            {
                dayM.Cakes.Add(cake[i].PareseAsCakeM());
            }
            return dayM.Cakes;
        }


    }
}
