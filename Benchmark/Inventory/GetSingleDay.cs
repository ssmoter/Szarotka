using BenchmarkDotNet.Attributes;

using DataBase.Model.EntitiesInventory;

using Inventory.Data;
using Inventory.Service;

using SelectedDriver = Inventory.Helper.SelectedDriver;

namespace Benchmark.Inventory
{
    [Config(typeof(AntiVirusFriendlyConfig))]
    [MemoryDiagnoser]
    public class GetSingleDay
    {
        private readonly DataBase.Data.AccessDataBase _db;
        private readonly ISelectDayService _selectDay;

        private readonly Guid _id = new Guid("98e3e70f-8ae9-4ff9-b045-a62a4444578c");
        private readonly DateTime _dateTime = new DateTime(638472044271180544);
        public GetSingleDay()
        {
            _db = new();
            _selectDay = new SelectDayService(_db);
            SelectedDriver.Id = "ae86aaeb-1080-4ae2-9226-62cf3a042ca8";
        }

        [Benchmark]
        public async Task<Day> GetDayProcedureDateTime()
        {
            var result = await _selectDay.GetDayProcedure(_id);
            return result;
        }
        [Benchmark]
        public async Task<Day> GetDayProcedureGuid()
        {
            var result = await _selectDay.GetDayProcedure(_dateTime);
            return result;
        }
        [Benchmark]
        public async Task<Day> GetDayWhenAllGuid()
        {
            var result = await _db.DataBaseAsync.Table<Day>().FirstOrDefaultAsync(x => x.Id == _id);

            var products = _db.DataBaseAsync.Table<Product>().Where(x => x.DayId == result.Id).ToArrayAsync();
            var cakes = _db.DataBaseAsync.Table<Cake>().Where(x => x.DayId == result.Id).ToArrayAsync();

            await Task.WhenAll(products, cakes);


            int productsCount = products.Result.Length;
            var nameTask = new Task<ProductName>[productsCount];
            var priceTask = new Task<ProductPrice>[productsCount];


            for (int i = 0; i < productsCount; i++)
            {
                var productNameId = products.Result[i].ProductNameId;
                var productPriceId = products.Result[i].ProductPriceId;
                nameTask[i] = _db.DataBaseAsync.Table<ProductName>().FirstOrDefaultAsync(x => x.Id == productNameId);
                priceTask[i] = _db.DataBaseAsync.Table<ProductPrice>().FirstOrDefaultAsync(x => x.Id == productPriceId);
            }
            await Task.WhenAll(nameTask);
            await Task.WhenAll(priceTask);


            for (int i = 0; i < productsCount; i++)
            {
                if (products.Result[i].ProductNameId == nameTask[i].Result.Id)
                {
                    products.Result[i].Name = nameTask[i].Result;
                }
                if (products.Result[i].ProductPriceId == priceTask[i].Result.Id)
                {
                    products.Result[i].Price = priceTask[i].Result;
                }
            }

            result.Cakes = new System.Collections.ObjectModel.ObservableCollection<Cake>(cakes.Result);
            result.Products = new System.Collections.ObjectModel.ObservableCollection<Product>(products.Result);

            return result;
        }
        [Benchmark]
        public async Task<Day> GetDayOnlyAwaitGuid()
        {
            var result = await _db.DataBaseAsync.Table<Day>().FirstOrDefaultAsync(x => x.Id == _id);

            var products = await _db.DataBaseAsync.Table<Product>().Where(x => x.DayId == result.Id).ToArrayAsync();
            var cakes = await _db.DataBaseAsync.Table<Cake>().Where(x => x.DayId == result.Id).ToArrayAsync();



            int productsCount = products.Length;
            var nameTask = new ProductName[productsCount];
            var priceTask = new ProductPrice[productsCount];


            for (int i = 0; i < productsCount; i++)
            {
                var productNameId = products[i].ProductNameId;
                var productPriceId = products[i].ProductPriceId;
                nameTask[i] = await _db.DataBaseAsync.Table<ProductName>().FirstOrDefaultAsync(x => x.Id == productNameId);
                priceTask[i] = await _db.DataBaseAsync.Table<ProductPrice>().FirstOrDefaultAsync(x => x.Id == productPriceId);
            }


            for (int i = 0; i < productsCount; i++)
            {
                if (products[i].ProductNameId == nameTask[i].Id)
                {
                    products[i].Name = nameTask[i];
                }
                if (products[i].ProductPriceId == priceTask[i].Id)
                {
                    products[i].Price = priceTask[i];
                }
            }

            result.Cakes = new System.Collections.ObjectModel.ObservableCollection<Cake>(cakes);
            result.Products = new System.Collections.ObjectModel.ObservableCollection<Product>(products);

            return result;
        }


    }
}
