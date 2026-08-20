using DataBase.Model.EntitiesServer;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // 1. Struktura przechowująca indeksy kolumn dla TEGO konkretnego zapytania (RefreshToken)
    public class RefreshTokenOrdinals
    {
        public int Id { get; }
        public int UserId { get; }
        public int Value { get; }
        public int ExpireDate { get; }

        public RefreshTokenOrdinals(SqliteDataReader reader)
        {
            // Tutaj GetOrdinal uruchamia się TYLKO RAZ przed pętlą!
            Id = reader.GetOrdinal(nameof(RefreshToken.Id));
            UserId = reader.GetOrdinal(nameof(RefreshToken.UserId));
            Value = reader.GetOrdinal(nameof(RefreshToken.Value));
            ExpireDate = reader.GetOrdinal(nameof(RefreshToken.ExpireDate));
        }
    }

    // 2. Rejestracja mappera dedykowanego dla RefreshToken (EntitiesServer)
    private static void InitRefreshTokenMappers()
    {
        _registry.Add(typeof(RefreshToken), (Func<SqliteDataReader, object, RefreshToken>)((reader, ordinalsObj) =>
        {
            ArgumentNullException.ThrowIfNull(reader);
            ArgumentNullException.ThrowIfNull(ordinalsObj);

            // Rzutujemy ogólny obiekt na naszą strukturę indeksów (AOT-safe)
            if (ordinalsObj is not RefreshTokenOrdinals ords)
            {
                throw new ArgumentException("Invalid ordinals object for RefreshToken mapper", nameof(ordinalsObj));
            }

            var refreshToken = new RefreshToken();

            // Mapowanie z gotowych indeksów liczbowych - ZERO wywołań GetOrdinal w pętli!
            // Zabezpieczenia przed NULL (IsDBNull) — przypisujemy wartości domyślne gdy kolumna jest NULL.
            if (!reader.IsDBNull(ords.Id))
            {
                refreshToken.Id = reader.GetInt32(ords.Id);
            }
            else
            {
                refreshToken.Id = 0;
            }

            if (!reader.IsDBNull(ords.UserId))
            {
                refreshToken.UserId = reader.GetString(ords.UserId);
            }
            else
            {
                refreshToken.UserId = "";
            }

            if (!reader.IsDBNull(ords.Value))
            {
                refreshToken.Value = reader.GetString(ords.Value);
            }
            else
            {
                refreshToken.Value = "";
            }

            if (!reader.IsDBNull(ords.ExpireDate))
            {
                refreshToken.ExpireDate = reader.GetInt64(ords.ExpireDate);
            }
            else
            {
                refreshToken.ExpireDate = 0L;
            }

            return refreshToken;
        }));
    }
}
