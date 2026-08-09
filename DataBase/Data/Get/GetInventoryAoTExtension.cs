using DataBase.Model.EntitiesInventory;
using DataBase.Model.SourceGenerator;

using System.Text;
using System.Text.Json;

namespace DataBase.Data.Get
{
    public static class GetInventoryAoTExtension
    {
        public static async Task<Day?> Day(this IGetInventoryAoT get, Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(Id));
            }

            var where = $"WHERE {nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.Id)} = @{nameof(Id)}";

            var result = await get.Days(where, new { Id });

            return result?.FirstOrDefault();
        }
        public static async Task<Day?> DaySelectedDateString(
            this IGetInventoryAoT get, string selectedDateString, Guid UserId)
        {
            if (string.IsNullOrWhiteSpace(selectedDateString))
            {
                throw new ArgumentNullException(nameof(selectedDateString));
            }

            var where = $@"
WHERE 
{nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.SelectedDateString)} = @{nameof(selectedDateString)}
AND
{nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.UserCreatedId)} = @{nameof(UserId)}";

            var result = await get.Days(where, new { selectedDateString, UserId });
            return result?.FirstOrDefault();
        }


        public static async Task<IList<Day>> Days(this IGetInventoryAoT get, long from, long to, IList<Guid> userIds)
        {
            StringBuilder where = new();
            if (to > 0 || userIds.Count > 0)
            {
                where.AppendLine(" WHERE ");
            }
            if (to > 0)
            {
                where.AppendLine($" {nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.SelectedDateTicks)} >= @{nameof(from)} AND {nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.SelectedDateTicks)} <= @{nameof(to)} ");
            }

            string jsonUserIds = JsonSerializer.Serialize(userIds,SzarotkaJsonSerializerContext.Default.IListGuid);
            if (userIds.Count > 0)
            {
                if (to > 0)
                {
                    where.Append(" AND ");
                }
                where.AppendLine(" ( ");
                where.Append(nameof(Model.EntitiesInventory.Day));
                where.Append('.');
                where.Append(nameof(Model.EntitiesInventory.Day.UserCreatedId));
                where.Append(" IN (SELECT value FROM json_each(@");
                where.Append(nameof(jsonUserIds));
                where.Append(")) ");
            }
            if (userIds.Count > 0)
            {
                where.Append(')');
            }

            object? args = null!;

            if (to > 0)
            {
                args = new { from, to };
            }
            if (userIds.Count > 0)
            {
                args = new { jsonUserIds };
            }
            if (to > 0 && userIds.Count > 0)
            {
                args = new { from, to, jsonUserIds };
            }

            var result = await get.Days(where.ToString(), args);
            return result;
        }


        public static async Task<IList<Product>> EmptyProducts(this IGetInventoryAoT get, bool isDelete = false)
        {
            IList<(ProductName name, IList<ProductPrice> price)> result = await get.EmptyProductsNameAndPrices(isDelete);

            int count = result.Count;
            Product[] products = new Product[count];

            for (int i = 0; i < count; i++)
            {
                products[i] = new()
                {
                    Name = result[i].name,
                    ProductNameId = result[i].name.Id
                };
                ProductPrice? price = result[i].price?.FirstOrDefault();
                if (price is not null)
                {
                    products[i].Price = result[i].price[0];
                    products[i].ProductPriceId = result[i].price[0].Id;
                }

            }
            return [.. products.OrderBy(x => x.Name.Arrangement)];
        }



    }
}
