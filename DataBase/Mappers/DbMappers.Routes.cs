using DataBase.Model.EntitiesRoutes;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (Routes)
    public class RoutesOrdinals : BaseOrdinals
    {
        public int Name { get; }

        public RoutesOrdinals(SqliteDataReader reader) : base(reader)
        {
            // Tutaj GetOrdinal uruchamia się TYLKO RAZ przed pętlą!
            Name = reader.GetOrdinal(nameof(Routes.Name));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla Routes (EntitiesRoutes)
    private static void InitRoutesMappers()
    {
        _registry.Add(typeof(Routes), (Func<SqliteDataReader, object, Routes>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            if (ordinalsObj is not RoutesOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for Routes mapper", nameof(ordinalsObj));
            }

            var r = new Routes();

            if (!reader.IsDBNull(ords.Name))
                r.Name = reader.GetString(ords.Name);
            else
                r.Name = string.Empty;

            MapBaseFields(reader, r, ords);

            return r;
        }));
    }
}
