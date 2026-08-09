using DataBase.Model.EntitiesInventory;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (Cake)
    public class CakeOrdinals : BaseOrdinals
    {
        public int DayId { get; }
        public int IsSell { get; }
        public int Price { get; }

        public CakeOrdinals(SqliteDataReader reader) : base(reader)
        {
            // Tutaj GetOrdinal uruchamia się TYLKO RAZ przed pętlą!
            DayId = reader.GetOrdinal(nameof(Cake.DayId));
            IsSell = reader.GetOrdinal(nameof(Cake.IsSell));
            Price = reader.GetOrdinal(nameof(Cake.Price));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla Cake (EntitiesInventory)
    private static void InitCakeMappers()
    {
        _registry.Add(typeof(Cake), (Func<SqliteDataReader, object, Cake>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            // Rzutujemy ogólny obiekt na naszą strukturę indeksów (AOT-safe)
            if (ordinalsObj is not CakeOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for Cake mapper", nameof(ordinalsObj));
            }

            var cake = new Cake();

            // Mapowanie z gotowych indeksów liczbowych - ZERO wywołań GetOrdinal w pętli!
            // Zabezpieczenia przed NULL (IsDBNull) — przypisujemy wartości domyślne gdy kolumna jest NULL.
            if (!reader.IsDBNull(ords.Id))
            {
                var idStr = reader.GetString(ords.Id);
                cake.Id = string.IsNullOrEmpty(idStr) ? Guid.Empty : new Guid(idStr);
            }
            else
            {
                cake.Id = Guid.Empty;
            }

            if (!reader.IsDBNull(ords.DayId))
            {
                var dayIdStr = reader.GetString(ords.DayId);
                cake.DayId = string.IsNullOrEmpty(dayIdStr) ? Guid.Empty : new Guid(dayIdStr);
            }
            else
            {
                cake.DayId = Guid.Empty;
            }

            if (!reader.IsDBNull(ords.IsSell))
            {
                cake.IsSell = reader.GetInt32(ords.IsSell) == 1;
            }
            else
            {
                cake.IsSell = false;
            }

            if (!reader.IsDBNull(ords.Price))
            {
                cake.Price = reader.GetInt32(ords.Price);
            }
            else
            {
                cake.Price = 0;
            }

            if (!reader.IsDBNull(ords.CreatedTicks))
            {
                cake.CreatedTicks = reader.GetInt64(ords.CreatedTicks);
            }
            else
            {
                cake.CreatedTicks = 0L;
            }
            MapBaseFields(reader, cake, ords);

            return cake;
        }));
    }
}
