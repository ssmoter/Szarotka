using DataBase.Data;

using Inventory.Helper;
using Inventory.Helper.Parse;
using Inventory.Model.MVVM;

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
                var DayM = new DayM();
                Service.ProductUpdatePriceService.EnableUpdate = false;
                var createdString = createdDate.ToString("dd.MM.yyyy");
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.CreatedDate == createdString).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                await GetProductTable(DayM);
                await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = createdDate;
                }
                DayM.DriverGuid = Helper.SelectedDriver.Guid;
                return DayM;
            }
            catch (Exception ex)
            {
                await _db.SaveLogAsync(ex);
                throw;
            }
            finally
            {
                Service.ProductUpdatePriceService.EnableUpdate = true;
            }
        }

        public async Task<DayM> GetDay(string createdDate)
        {
            try
            {
                await SnackbarAsToats.OnShow("Pobieranie danego dnia ");
                var DayM = new DayM();
                Service.ProductUpdatePriceService.EnableUpdate = false;
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.CreatedDate == createdDate).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                await GetProductTable(DayM);
                await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = DateTime.Parse(createdDate, DataBase.Helper.Constants.CultureInfo);
                }
                DayM.DriverGuid = Helper.SelectedDriver.Guid;
                return DayM;
            }
            catch (Exception ex)
            {
                await _db.SaveLogAsync(ex);
                throw;
            }
            finally
            {
                Service.ProductUpdatePriceService.EnableUpdate = true;
            }
        }

        public async Task<DayM> GetDay(Guid id)
        {
            try
            {
                await SnackbarAsToats.OnShow("Pobieranie danego dnia ");
                var DayM = new DayM();
                Service.ProductUpdatePriceService.EnableUpdate = false;
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.Id == id).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                await GetProductTable(DayM);
                await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = DateTime.Now;
                }
                DayM.DriverGuid = Helper.SelectedDriver.Guid;
                return DayM;
            }
            catch (Exception ex)
            {
                await _db.SaveLogAsync(ex);
                throw;
            }
            finally
            {
                Service.ProductUpdatePriceService.EnableUpdate = true;
            }
        }

        public async Task<DayM> GetDay()
        {
            try
            {
                await SnackbarAsToats.OnShow("Pobieranie danego dnia ");
                var DayM = new DayM();
                Service.ProductUpdatePriceService.EnableUpdate = false;
                var time = DateTime.Now.ToString("dd.MM.yyyy");
                var today = await _db.DataBaseAsync.Table<Model.Day>().Where(x => x.CreatedDate == time).FirstOrDefaultAsync();
                DayM = today.ParseAsDayM();
                await GetProductTable(DayM);
                await GetCakeTable(DayM);
                if (today is null)
                {
                    DayM.Created = DateTime.Now;
                }
                DayM.DriverGuid = Helper.SelectedDriver.Guid;
                return DayM;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
            finally
            {
                Service.ProductUpdatePriceService.EnableUpdate = true;
            }
        }

        public async Task GetProductTable(DayM dayM)
        {
            var product = await _db.DataBaseAsync.Table<Model.Product>().Where(x => x.DayId == dayM.Id).ToArrayAsync();
            dayM.Products.Clear();

            for (int i = 0; i < product.Length; i++)
            {
                var productNameId = product[i].ProductNameId;
                product[i].Name = await _db.DataBaseAsync.Table<Model.ProductName>().FirstOrDefaultAsync(x => x.Id == productNameId);
                product[i].Price = await _db.DataBaseAsync.Table<Model.ProductPrice>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.ProductNameId == productNameId);

                dayM.Products.Add(product[i].ParseAsProductM());
            }

            if (dayM.Products.Count == 0)
            {
                var productName = await _db.DataBaseAsync.Table<Model.ProductName>().ToArrayAsync();
                for (int i = 0; i < productName.Length; i++)
                {
                    var nameId = productName[i].Id;
                    var price = await _db.DataBaseAsync.Table<Model.ProductPrice>().OrderByDescending(x => x.Id).FirstOrDefaultAsync(x => x.ProductNameId == nameId);
                    dayM.Products.Add(new ProductM() { Name = productName[i].PareseAsProductNameM(), ProductNameId = productName[i].Id, Price = price.PareseAsProductPriceM() });
                }
            }
        }
        public async Task GetCakeTable(DayM dayM)
        {
            var cake = await _db.DataBaseAsync.Table<Model.Cake>().Where(x => x.DayId == dayM.Id).ToArrayAsync();
            dayM.Cakes.Clear();
            for (int i = 0; i < cake.Length; i++)
            {
                dayM.Cakes.Add(cake[i].PareseAsCakeM());
            }

        }


    }
}
