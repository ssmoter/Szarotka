using DataBase.Data;
using DataBase.Helper;
using DataBase.Model.EntitiesInventory;

using Inventory.Helper;
using Inventory.Service;

using System.Collections.ObjectModel;
using System.Text;

namespace Inventory.Data
{
    public class SelectDayService(AccessDataBase db) : ISelectDayService
    {
        readonly AccessDataBase _db = db;

        public async Task<(Driver[] drivers, Day[] days)> GetDaysAndDrivers(long from, long to, Guid[] selectedDriverName, bool moreData)
        {
            (Driver[] drivers, Day[] days) value = new();
            if (selectedDriverName is null || selectedDriverName.Length == 0)
            {
                value.days = await _db.DataBaseAsync.Table<Day>().
                    Where(x => x.SelectedDateTicks >= from && x.SelectedDateTicks <= to).
                    OrderByDescending(x => x.SelectedDateTicks).ToArrayAsync();
            }
            else
            {
                var sb = new StringBuilder();

                sb.Append("SELECT * FROM Day ");
                sb.Append($"WHERE {nameof(Day.SelectedDateTicks)} >= ");
                sb.Append(from);
                sb.Append($" AND {nameof(Day.SelectedDateTicks)} <= ");
                sb.Append(to);

                if (selectedDriverName.Length > 0)
                {
                    sb.Append(" AND (");
                }

                for (int i = 0; i < selectedDriverName.Length; i++)
                {
                    if (i == 0)
                        sb.Append($" {nameof(Day.DriverGuid)} == '");
                    else if (i < selectedDriverName.Length)
                        sb.Append($" OR {nameof(Day.DriverGuid)} == '");

                    sb.Append(selectedDriverName[i]);
                    sb.Append('\'');
                }

                if (selectedDriverName.Length > 0)
                {
                    sb.Append(" )");
                }
                sb.Append($"ORDER BY {nameof(Day.SelectedDateTicks)} DESC");

                var result = await _db.DataBaseAsync.QueryAsync<Day>(sb.ToString());
                value.days = [.. result];
                result.Clear();

            }

            value.drivers = new Driver[value.days.Length];
            for (int i = 0; i < value.days.Length; i++)
            {
                var guid = value.days[i].DriverGuid;
                var driver = await _db.DataBaseAsync.Table<Driver>().FirstOrDefaultAsync(x => x.Id == guid);
                if (driver is not null)
                {
                    value.drivers[i] = driver;
                }
            }

            if (moreData)
            {
                for (int i = 0; i < value.days.Length; i++)
                {
                    value.days[i] = await GetDayProcedure(value.days[i].Id);
                }
            }

            return value;
        }

        public async Task<Day> GetDayProcedure(DateTime createdDate)
        {
            var id = await DateFindId(createdDate);
            if (id != Guid.Empty)
            {
                var dayId = await GetSingleDay(id);
                return dayId;
            }

            var day = await GetSingleDay(id);
            if (day.Id == Guid.Empty)
            {
                day.SelectedDate = createdDate;
                if (day.SelectedDate.Hour == 0 && day.SelectedDate.Minute == 0)
                    day.SelectedDate = new DateTime(createdDate.Year, createdDate.Month, createdDate.Day, 12, 0, 0);
            }
            return day;
        }
        public async Task<Day> GetDayProcedure(Guid id)
        {
            var day = await GetSingleDay(id);
            if (day.Id == Guid.Empty)
            {
                day.SelectedDate = DateTime.Now;
            }
            return day;
        }


        private async Task<Day> GetSingleDay(Guid id)
        {
            var day = await _db.DataBaseAsync.Table<Day>().FirstOrDefaultAsync(x => x.Id == id);

            if (day is not null)
            {
                var products = await _db.DataBaseAsync.QueryAsync<GetProduct>(StoredProcedure.GetProductWitchPriceAndName(), id.ToString());
                for (int i = 0; i < products.Count; i++)
                {
                    day.Products.Add(new(products[i]));
                    day.Products[i].Name = System.Text.Json.JsonSerializer.Deserialize<ProductName>(products[i].JsonName
                        , JsonOptions.JsonSerializeOptions);
                    day.Products[i].Price = System.Text.Json.JsonSerializer.Deserialize<ProductPrice>(products[i].JsonPrice
                        , JsonOptions.JsonSerializeOptions);
                }
                var cakes = await _db.DataBaseAsync.Table<Cake>().Where(x => x.DayId == id).ToArrayAsync();
                day.Cakes = new ObservableCollection<Cake>(cakes);
                for (int i = 0; i < day.Cakes.Count; i++)
                    day.Cakes[i].Index = i + 1;

            }
            else
            {
                day = new();
                var products = await _db.DataBaseAsync.QueryAsync<GetProductName>(StoredProcedure.GetAllProductsNameAndPrice());
                for (int i = 0; i < products.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(products[i].JsonPrice))
                        continue;

                    var price = System.Text.Json.JsonSerializer.Deserialize<ProductPrice>(products[i].JsonPrice
                        , JsonOptions.JsonSerializeOptions);
                    day.Products.Add(new Product()
                    {
                        Name = products[i],
                        Price = price,
                        DayId = day.Id,
                        ProductNameId = products[i].Id,
                        ProductPriceId = price.Id,
                    });
                }
            }
            return day;
        }
        private async Task<Guid> DateFindId(DateTime createdDate)
        {
            var guid = new Guid(Helper.SelectedDriver.Id);
            var createdString = createdDate.ToString("dd.MM.yyyy");
            var id = await _db.DataBaseAsync.Table<Day>().FirstOrDefaultAsync(x => x.SelectedDateString == createdString && x.DriverGuid == guid);
            if (id != null)
                return id.Id;
            return Guid.Empty;
        }


        class GetProductName : ProductName
        {
            public string JsonPrice { get; set; }
        }
        class GetProduct : Product
        {
            public string JsonPrice { get; set; }
            public string JsonName { get; set; }
        }

    }
}
