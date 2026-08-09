using DataBase.Data.Get;
using DataBase.Model.EntitiesInventory;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // Ordinals dla ProductNameAndPrice (dziedziczy ProductNameOrdinals)
    public class ProductNameAndPriceOrdinals : ProductNameOrdinals
    {
        public int JsonPrice { get; }

        public ProductNameAndPriceOrdinals(SqliteDataReader reader) : base(reader)
        {
            JsonPrice = reader.GetOrdinal(nameof(GetInventoryAoT.ProductNameAndPrice.JsonPrice));
        }
    }

    private static void InitProductNameAndPriceMappers()
    {
        _registry.Add(typeof(GetInventoryAoT.ProductNameAndPrice), (Func<SqliteDataReader, object, GetInventoryAoT.ProductNameAndPrice>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not ProductNameAndPriceOrdinals ords)
                throw new ArgumentException("Invalid ordinals object for ProductNameAndPrice mapper", nameof(ordinalsObj));

            var pn = new GetInventoryAoT.ProductNameAndPrice();

            // map ProductName fields
            if (!reader.IsDBNull(ords.Arrangement)) pn.Arrangement = reader.GetInt32(ords.Arrangement); else pn.Arrangement = 0;
            if (!reader.IsDBNull(ords.Name)) pn.Name = reader.GetString(ords.Name); else pn.Name = string.Empty;
            if (!reader.IsDBNull(ords.Description)) pn.Description = reader.GetString(ords.Description); else pn.Description = string.Empty;
            if (!reader.IsDBNull(ords.Img)) pn.Img = reader.GetString(ords.Img); else pn.Img = string.Empty;
            if (!reader.IsDBNull(ords.IsVisible)) pn.IsVisible = reader.GetInt32(ords.IsVisible) == 1; else pn.IsVisible = true;

            // map JsonPrice
            if (!reader.IsDBNull(ords.JsonPrice)) pn.JsonPrice = reader.GetString(ords.JsonPrice); else pn.JsonPrice = string.Empty;

            MapBaseFields(reader, pn, ords);

            return pn;
        }));
    }
}
