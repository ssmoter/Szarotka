using DataBase.Model.EntitiesInventory;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (ProductName)
    public class ProductNameOrdinals : BaseOrdinals
    {
        public int Arrangement { get; }
        public int Name { get; }
        public int Description { get; }
        public int Img { get; }
        public int IsVisible { get; }

        public ProductNameOrdinals(SqliteDataReader reader) : base(reader)
        {
            // Tutaj GetOrdinal uruchamia się TYLKO RAZ przed pętlą!
            Arrangement = reader.GetOrdinal(nameof(ProductName.Arrangement));
            Name = reader.GetOrdinal(nameof(ProductName.Name));
            Description = reader.GetOrdinal(nameof(ProductName.Description));
            Img = reader.GetOrdinal(nameof(ProductName.Img));
            IsVisible = reader.GetOrdinal(nameof(ProductName.IsVisible));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla ProductName (EntitiesInventory)
    private static void InitProductNameMappers()
    {
        _registry.Add(typeof(ProductName), (Func<SqliteDataReader, object, ProductName>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not ProductNameOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for ProductName mapper", nameof(ordinalsObj));
            }

            var pn = new ProductName();

            if (!reader.IsDBNull(ords.Arrangement))
                pn.Arrangement = reader.GetInt32(ords.Arrangement);
            else
                pn.Arrangement = 0;

            if (!reader.IsDBNull(ords.Name))
                pn.Name = reader.GetString(ords.Name);
            else
                pn.Name = string.Empty;

            if (!reader.IsDBNull(ords.Description))
                pn.Description = reader.GetString(ords.Description);
            else
                pn.Description = string.Empty;

            if (!reader.IsDBNull(ords.Img))
                pn.Img = reader.GetString(ords.Img);
            else
                pn.Img = string.Empty;

            if (!reader.IsDBNull(ords.IsVisible))
                pn.IsVisible = reader.GetInt32(ords.IsVisible) == 1;
            else
                pn.IsVisible = true;

            MapBaseFields(reader, pn, ords);

            return pn;
        }));
    }
}
