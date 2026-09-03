using DataBase.Model;

using Microsoft.Data.Sqlite;
namespace DataBase.Mappers;

public partial class DbMappers
{
    // Słownik przechowuje funkcje przyjmujące: (reader, obiekt_indeksów_kolumn)
    private static readonly Dictionary<Type, Delegate> _registry = [];

    static DbMappers()
    {
        InitAllModules();
    }

    // Tę metodę uzupełnimy w głównym pliku, wywołując w niej metody z plików partial
    private static void InitAllModules()
    {
        // Rejestracja mapperów z poszczególnych modułów (pliki partial)
        InitCakeMappers();
        InitDayMappers();
        InitProductMappers();
        InitProductNameMappers();
        InitProductPriceMappers();
        InitProductNameAndPriceMappers();
        InitCustomerRoutesMappers();
        InitCustomerRoutesFromQueryMappers();
        InitResidentialAddressMappers();
        InitRoutesMappers();
        InitSelectedDayOfWeekMappers();
        InitDayFromQueryMappers();
        InitDataBaseVersionMappers();
        InitLogsModelMappers();
        InitUserMappers();
        InitRegisterUserMappers();
        InitLoginUserMappers();
        InitConfirmCodeMappers();
        InitUpdateLogMappers();
        InitTableInfoMappers();
        InitRefreshTokenMappers();
    }

    public static Func<SqliteDataReader, object, T> Get<T>()
    {
        if (_registry.TryGetValue(typeof(T), out var mapper))
        {
            return (Func<SqliteDataReader, object, T>)mapper;
        }
        throw new InvalidOperationException($"Brak mappera dla typu {typeof(T).Name}.");
    }


    // 1. INDEKSY DLA KLASY BAZOWEJ - wywołają GetOptionalOrdinal tylko raz (kolumny zgodne z BaseEntities)
    public class BaseOrdinals
    {
        public int Id { get; }
        public int CreatedTicks { get; }
        public int UpdatedTicks { get; }
        public int IsDelete { get; }
        public int UserCreatedId { get; }
        public int UserUpdatedId { get; }

        public BaseOrdinals(SqliteDataReader reader)
        {
            // Tutaj GetOptionalOrdinal uruchamia się TYLKO RAZ przed pętlą!
            Id = reader.GetOptionalOrdinal(nameof(BaseEntities<>.Id));
            CreatedTicks = reader.GetOptionalOrdinal(nameof(BaseEntities<>.CreatedTicks));
            UpdatedTicks = reader.GetOptionalOrdinal(nameof(BaseEntities<>.UpdatedTicks));
            IsDelete = reader.GetOptionalOrdinal(nameof(BaseEntities<>.IsDelete));
            UserCreatedId = reader.GetOptionalOrdinal(nameof(BaseEntities<>.UserCreatedId));
            UserUpdatedId = reader.GetOptionalOrdinal(nameof(BaseEntities<>.UserUpdatedId));
        }
    }

    // 2. WSPÓLNA METODA MAPUJĄCA - wywoływana w pętli (bez refleksji, wykorzystuje pattern matching dla znanych typów Id)
    private static BaseEntities<TId> MapBaseFields<TId>(SqliteDataReader reader, BaseEntities<TId> entity, BaseOrdinals ords)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(ords);

        // Id - mapowanie podobne do Cake: rozpoznajemy kilka popularnych typów Id
        if (ords.Id >= 0 && !reader.IsDBNull(ords.Id))
        {
            if (typeof(TId) == typeof(Guid))
            {
                var idStr = reader.GetString(ords.Id);
                entity.Id = (TId)(object)(string.IsNullOrEmpty(idStr) ? Guid.Empty : new Guid(idStr));
            }
            else if (typeof(TId) == typeof(int))
            {
                entity.Id = (TId)(object)reader.GetInt32(ords.Id);
            }
            else if (typeof(TId) == typeof(long))
            {
                entity.Id = (TId)(object)reader.GetInt64(ords.Id);
            }
            else if (typeof(TId) == typeof(string))
            {
                entity.Id = (TId)(object)reader.GetString(ords.Id);
            }
            else
            {
                // fallback: próbujemy pobrać wartość i konwertować
                var val = reader.GetValue(ords.Id);
                entity.Id = (TId)Convert.ChangeType(val, typeof(TId));
            }
        }
        else
        {
            // brak wartości -> ustawienie domyślne
            if (typeof(TId) == typeof(Guid)) entity.Id = (TId)(object)Guid.Empty;
            else if (typeof(TId) == typeof(int)) entity.Id = (TId)(object)default(int);
            else if (typeof(TId) == typeof(long)) entity.Id = (TId)(object)default(long);
            else if (typeof(TId) == typeof(string)) entity.Id = (TId)(object)string.Empty;
            else entity.Id = default!;
        }

        // Pozostałe pola BaseEntities - bez kombinowania, jak w Cake: bezrefleksyjnie
        if (ords.CreatedTicks >= 0 && !reader.IsDBNull(ords.CreatedTicks))
            entity.CreatedTicks = reader.GetInt64(ords.CreatedTicks);
        else
            entity.CreatedTicks = 0L;

        if (ords.UpdatedTicks >= 0 && !reader.IsDBNull(ords.UpdatedTicks))
            entity.UpdatedTicks = reader.GetInt64(ords.UpdatedTicks);
        else
            entity.UpdatedTicks = 0L;

        if (ords.IsDelete >= 0 && !reader.IsDBNull(ords.IsDelete))
            entity.IsDelete = reader.GetInt32(ords.IsDelete) == 1;
        else
            entity.IsDelete = false;

        if (ords.UserCreatedId >= 0 && !reader.IsDBNull(ords.UserCreatedId))
        {
            var uc = reader.GetString(ords.UserCreatedId);
            entity.UserCreatedId = string.IsNullOrEmpty(uc) ? Guid.Empty : new Guid(uc);
        }
        else
            entity.UserCreatedId = Guid.Empty;

        if (ords.UserUpdatedId >= 0 && !reader.IsDBNull(ords.UserUpdatedId))
        {
            var uu = reader.GetString(ords.UserUpdatedId);
            entity.UserUpdatedId = string.IsNullOrEmpty(uu) ? Guid.Empty : new Guid(uu);
        }
        else
            entity.UserUpdatedId = Guid.Empty;

        return entity;
    }
}


public static class SqliteDataReaderExtensions
{
    public static int GetOptionalOrdinal(this SqliteDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }
        return -1; // Zwraca -1, jeśli kolumna nie istnieje
    }
}
