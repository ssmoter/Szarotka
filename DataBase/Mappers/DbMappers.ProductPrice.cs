using DataBase.Model.EntitiesInventory;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (ProductPrice)
    public class ProductPriceOrdinals : BaseOrdinals
    {
        public int ProductNameId { get; }
        public int Price { get; }

        public ProductPriceOrdinals(SqliteDataReader reader) : base(reader)
        {
            // Tutaj GetOrdinal uruchamia się TYLKO RAZ przed pętlą!
            ProductNameId = reader.GetOrdinal(nameof(ProductPrice.ProductNameId));
            Price = reader.GetOrdinal(nameof(ProductPrice.Price));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla ProductPrice (EntitiesInventory)
    private static void InitProductPriceMappers()
    {
        _registry.Add(typeof(ProductPrice), (Func<SqliteDataReader, object, ProductPrice>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not ProductPriceOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for ProductPrice mapper", nameof(ordinalsObj));
            }

            var pp = new ProductPrice();

            if (!reader.IsDBNull(ords.ProductNameId))
            {
                var pnIdStr = reader.GetString(ords.ProductNameId);
                pp.ProductNameId = string.IsNullOrEmpty(pnIdStr) ? Guid.Empty : new Guid(pnIdStr);
            }
            else
            {
                pp.ProductNameId = Guid.Empty;
            }

            if (!reader.IsDBNull(ords.Price))
                pp.Price = reader.GetInt32(ords.Price);
            else
                pp.Price = 0;

            MapBaseFields(reader, pp, ords);

            return pp;
        }));
    }
}
