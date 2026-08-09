using DataBase.Model.EntitiesInventory;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (Product)
    public class ProductOrdinals : BaseOrdinals
    {
        public int DayId { get; }
        public int ProductNameId { get; }
        public int ProductPriceId { get; }
        public int Description { get; }
        public int PriceTotal { get; }
        public int PriceTotalCorrect { get; }
        public int PriceTotalAfterCorrect { get; }
        public int Number { get; }
        public int NumberEdit { get; }
        public int NumberReturn { get; }

        public ProductOrdinals(SqliteDataReader reader) : base(reader)
        {
            // Tutaj GetOrdinal uruchamia się TYLKO RAZ przed pętlą!
            DayId = reader.GetOrdinal(nameof(Product.DayId));
            ProductNameId = reader.GetOrdinal(nameof(Product.ProductNameId));
            ProductPriceId = reader.GetOrdinal(nameof(Product.ProductPriceId));
            Description = reader.GetOrdinal(nameof(Product.Description));
            PriceTotal = reader.GetOrdinal(nameof(Product.PriceTotal));
            PriceTotalCorrect = reader.GetOrdinal(nameof(Product.PriceTotalCorrect));
            PriceTotalAfterCorrect = reader.GetOrdinal(nameof(Product.PriceTotalAfterCorrect));
            Number = reader.GetOrdinal(nameof(Product.Number));
            NumberEdit = reader.GetOrdinal(nameof(Product.NumberEdit));
            NumberReturn = reader.GetOrdinal(nameof(Product.NumberReturn));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla Product (EntitiesInventory)
    private static void InitProductMappers()
    {
        _registry.Add(typeof(Product), (Func<SqliteDataReader, object, Product>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not ProductOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for Product mapper", nameof(ordinalsObj));
            }

            var product = new Product();

            if (!reader.IsDBNull(ords.DayId))
            {
                var dayIdStr = reader.GetString(ords.DayId);
                product.DayId = string.IsNullOrEmpty(dayIdStr) ? Guid.Empty : new Guid(dayIdStr);
            }
            else
            {
                product.DayId = Guid.Empty;
            }

            if (!reader.IsDBNull(ords.ProductNameId))
            {
                var pnIdStr = reader.GetString(ords.ProductNameId);
                product.ProductNameId = string.IsNullOrEmpty(pnIdStr) ? Guid.Empty : new Guid(pnIdStr);
            }
            else
            {
                product.ProductNameId = Guid.Empty;
            }

            if (!reader.IsDBNull(ords.ProductPriceId))
            {
                var ppIdStr = reader.GetString(ords.ProductPriceId);
                product.ProductPriceId = string.IsNullOrEmpty(ppIdStr) ? Guid.Empty : new Guid(ppIdStr);
            }
            else
            {
                product.ProductPriceId = Guid.Empty;
            }

            if (!reader.IsDBNull(ords.Description))
            {
                product.Description = reader.GetString(ords.Description);
            }
            else
            {
                product.Description = string.Empty;
            }

            if (!reader.IsDBNull(ords.PriceTotal))
                product.PriceTotal = reader.GetInt32(ords.PriceTotal);
            else
                product.PriceTotal = 0;

            if (!reader.IsDBNull(ords.PriceTotalCorrect))
                product.PriceTotalCorrect = reader.GetInt32(ords.PriceTotalCorrect);
            else
                product.PriceTotalCorrect = 0;

            if (!reader.IsDBNull(ords.PriceTotalAfterCorrect))
                product.PriceTotalAfterCorrect = reader.GetInt32(ords.PriceTotalAfterCorrect);
            else
                product.PriceTotalAfterCorrect = 0;

            if (!reader.IsDBNull(ords.Number))
                product.Number = reader.GetInt32(ords.Number);
            else
                product.Number = 0;

            if (!reader.IsDBNull(ords.NumberEdit))
                product.NumberEdit = reader.GetInt32(ords.NumberEdit);
            else
                product.NumberEdit = 0;

            if (!reader.IsDBNull(ords.NumberReturn))
                product.NumberReturn = reader.GetInt32(ords.NumberReturn);
            else
                product.NumberReturn = 0;

            MapBaseFields(reader, product, ords);

            return product;
        }));
    }
}
