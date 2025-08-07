using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesInventory;

namespace DataBase.Data.Get
{
    public interface IGetInventoryAoT
    {
        Task<IList<Day>> Days(string where, params object[] args);
        Task<IList<(ProductName, IList<ProductPrice>)>> EmptyProductsNameAndPrices(bool isDelete = false);
    }

    public class GetInventoryAoT(IAccessDataBase db) : IGetInventoryAoT
    {
        private readonly IAccessDataBase _db = db;
        public async Task<IList<Day>> Days(string where, params object[] args)
        {
            var sql = DayQuery.GetFullDaysProcedureWithoutWhere() + where;
            List<DayFromQuery> result = [];
            if (args is null)
            {
                result = await _db.DataBaseAsync.QueryAsync<DayFromQuery>(sql);
            }
            else
            {
                result = await _db.DataBaseAsync.QueryAsync<DayFromQuery>(sql, args);
            }
            for (int i = 0; i < result.Count; i++)
            {
                var products =
                    System.Text.Json.JsonSerializer.Deserialize(
                        result[i].JsonProducts,
                        DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.ObservableCollectionProduct);
                var cakes =
                    System.Text.Json.JsonSerializer.Deserialize(
                        result[i].JsonCakes,
                        DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.ObservableCollectionCake);
                if (products is not null)
                {
                    result[i].Products = products;
                }
                if (cakes is not null)
                {
                    result[i].Cakes = cakes;
                }
            }

            return [.. result.Select(x => x as Day)];
        }
        public async Task<IList<(ProductName, IList<ProductPrice>)>> EmptyProductsNameAndPrices(bool isDelete = false)
        {
            var sql = ProductNameQuery.GetNameAndPrice(isDelete);

            var result = await _db.DataBaseAsync.QueryAsync<ProductNameAndPrice>(sql);
            int count = result.Count;
            (ProductName name, IList<ProductPrice> prices)[] product = new (ProductName name, IList<ProductPrice> prices)[count];


            for (int i = 0; i < count; i++)
            {
                if (result[i] is null)
                {
                    continue;
                }

                IList<ProductPrice>? price =
                     System.Text.Json.JsonSerializer.Deserialize(
                         result[i].JsonPrice,
                         DataBase.Model.JsonContext.SzarotkaJsonSerializerContext.Default.IListProductPrice);

                if (price is not null)
                {
                    product[i].prices = [.. price.OrderByDescending(x => x.CreatedTicks)];
                }
                product[i].name = result[i];
            }
            return product;
        }


        private partial class ProductNameAndPrice : ProductName
        {
            public string JsonPrice { get; set; } = "";
        }
        private partial class DayFromQuery : Day
        {
            public string JsonProducts { get; set; } = "";
            public string JsonCakes { get; set; } = "";
        }
    }
}
