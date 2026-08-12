
using DataBase.Data.SqlQuery;
using DataBase.Model.EntitiesInventory;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace DataBase.Data.Get
{
    public interface IGetInventoryAoT
    {
        Task<IList<Day>> Days(string where, Dictionary<string, object?>? param = null);
        Task<IList<(ProductName, IList<ProductPrice>)>> EmptyProductsNameAndPrices(bool isDelete = false);

        Task<ProductName?> GetProductName(Guid id);
        Task<ProductPrice?> GetProductPrice(Guid id);
        Task<IList<ProductPrice>> GetProductPrices(Guid nameId);
    }

    public class GetInventoryAoT(IAccessDataBaseAoT db) : IGetInventoryAoT
    {
        private readonly IAccessDataBaseAoT _db = db;


        public async Task<IList<Day>> Days(string where, Dictionary<string, object?>? param = null)
        {
            var sql = DayQuery.GetFullDaysProcedureWithoutWhere() + where;
            IEnumerable<DayFromQuery> result = [];
            if (param is null)
            {
                result = await _db.DbAsyncAoT.QueryAsync<DayFromQuery>(sql);
            }
            if (param is not null)
            {
                result = await _db.DbAsyncAoT.QueryAsync<DayFromQuery>(sql, param!);
            }

            foreach (DayFromQuery day in result)
            {
                var products =
                    System.Text.Json.JsonSerializer.Deserialize(
                        day.JsonProducts,
                        DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.ObservableCollectionProduct);
                var cakes =
                    System.Text.Json.JsonSerializer.Deserialize(
                        day.JsonCakes,
                        DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.ObservableCollectionCake);
                if (products is not null)
                {
                    day.Products = products;
                }
                if (cakes is not null)
                {
                    day.Cakes = cakes;
                }
            }

            return [.. result.Select(x => new Day(x))];
        }


        public async Task<IList<(ProductName, IList<ProductPrice>)>> EmptyProductsNameAndPrices(bool isDelete = false)
        {
            var sql = ProductNameQuery.GetNameAndPrice(isDelete);

            var result = await _db.DbAsyncAoT.QueryAsync<ProductNameAndPrice>(sql);
            List<(ProductName name, IList<ProductPrice> prices)> product = new(26);
            foreach (ProductNameAndPrice item in result)
            {
                if (item is null)
                {
                    continue;
                }
                IList<ProductPrice>? price =
                     System.Text.Json.JsonSerializer.Deserialize(
                         item.JsonPrice,
                         DataBase.Model.SourceGenerator.SzarotkaJsonSerializerContext.Default.IListProductPrice);
                if (price is not null)
                {
                    product.Add(new() { name = item, prices = [.. price.OrderByDescending(x => x.CreatedTicks)] });
                }
                else
                {
                    product.Add(new() { name = item });
                }
            }

            return product;
        }

        [StringSyntax("Sql")]
        const string _sqlGetProductName = $"SELECT * FROM {nameof(ProductName)} WHERE {nameof(ProductName)}.{nameof(ProductName.Id)} == @{nameof(ProductName.Id)}";

        public async Task<ProductName?> GetProductName(Guid Id)
        {
            var result = await _db.DbAsyncAoT.QueryAsync<ProductName>(_sqlGetProductName, new() { [nameof(ProductName.Id)] = Id });
            return result?.FirstOrDefault();
        }

        [StringSyntax("Sql")]
        const string _sqlGetProductPrices = $"SELECT * FROM {nameof(ProductPrice)} WHERE {nameof(ProductPrice)}.{nameof(ProductPrice.ProductNameId)} == @{nameof(ProductPrice.ProductNameId)}";

        public async Task<IList<ProductPrice>> GetProductPrices(Guid ProductNameId)
        {

            var result = await _db.DbAsyncAoT.QueryAsync<ProductPrice>(_sqlGetProductPrices, new() { [nameof(ProductPrice.ProductNameId)] = ProductNameId });
            return [.. result];
        }

        [StringSyntax("Sql")]
        const string _sqlGetProductPrice = $"SELECT * FROM {nameof(ProductPrice)} WHERE {nameof(ProductPrice)}.{nameof(ProductPrice.Id)} == @{nameof(ProductPrice.Id)}";

        public async Task<ProductPrice?> GetProductPrice(Guid Id)
        {
            var result = await _db.DbAsyncAoT.QueryAsync<ProductPrice>(_sqlGetProductPrice, new() { [nameof(ProductPrice.Id)] = Id });
            return result?.FirstOrDefault();
        }


        public partial class ProductNameAndPrice : ProductName
        {
            [JsonIgnore]
            public string JsonPrice { get; set; } = "";
        }
        public partial class DayFromQuery : Day
        {
            [JsonIgnore]
            public string JsonProducts { get; set; } = "";
            [JsonIgnore]
            public string JsonCakes { get; set; } = "";
        }
    }
}
