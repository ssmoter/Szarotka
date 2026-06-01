using DataBase.Model.EntitiesInventory;

using System.Text;

namespace DataBase.Data.Get
{
    public static class GetInventoryAoTExtension
    {
        public static async Task<Day?> Day(this IGetInventoryAoT get, Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var where = $"WHERE {nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.Id)} = ?";

            var result = await get.Days(where, id);

            return result?.FirstOrDefault();
        }
        public static async Task<Day?> DaySelectedDateString(
            this IGetInventoryAoT get, string selectedDateString, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(selectedDateString))
            {
                throw new ArgumentNullException(nameof(selectedDateString));
            }

            var where = $@"
WHERE 
{nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.SelectedDateString)} = ?
AND
{nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.UserCreatedId)} = ?";

            var result = await get.Days(where, selectedDateString, userId);
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
                where.AppendLine($" {nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.SelectedDateTicks)} >= ? AND {nameof(Model.EntitiesInventory.Day)}.{nameof(Model.EntitiesInventory.Day.SelectedDateTicks)} <= ? ");
            }
            for (int i = 0; i < userIds.Count; i++)
            {
                if (i == 0)
                {
                    if (to > 0)
                    {
                        where.Append(" AND ");
                    }
                    where.AppendLine(" ( ");
                    where.Append(nameof(Model.EntitiesInventory.Day));
                    where.Append('.');
                    where.Append(nameof(Model.EntitiesInventory.Day.UserCreatedId));
                    where.Append(" = ? ");
                }
                if (i > 0)
                {
                    where.AppendLine(" OR ");
                    where.Append(nameof(Model.EntitiesInventory.Day));
                    where.Append('.');
                    where.Append(nameof(Model.EntitiesInventory.Day.UserCreatedId));
                    where.Append(" = ? ");
                }
            }
            if (userIds.Count > 0)
            {
                where.Append(')');
            }

            object[]? args = null!;

            if (to > 0)
            {
                args = [from, to];
            }
            if (userIds.Count > 0)
            {
                args = [.. userIds];
            }
            if (to > 0 && userIds.Count > 0)
            {
                args = [from, to, .. userIds];
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
                products[i] = new();
                products[i].Name = result[i].name;
                products[i].ProductNameId = result[i].name.Id;
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
