using DataBase.Mappers;

namespace DataBase.Data.MySqliteConnection
{
    public interface IMyDbAsyncConnection
    {
        Task<IMyDbAsyncTransaction> BeginTransactionAsync();
        Task<int> ExecuteAsync(string sql, Dictionary<string, object?> param);
        Task<int> ExecuteAsync(string sql);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object?> param);
        Task<IEnumerable<T>> QueryAsync<T>(string sql);
    }
    public class MyDbAsyncConnection : IMyDbAsyncConnection
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public MyDbAsyncConnection(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Wersja Z parametrami (TParam) - dla operacji typu: _db.ExecuteAsync(sql, new { Id })
        public async Task<int> ExecuteAsync(string sql, Dictionary<string, object?> param)
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            await using var transaction = await connection.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);

            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = (Microsoft.Data.Sqlite.SqliteTransaction)transaction;

            // Automatyczne dodawanie parametrów z obiektu anonimowego (AOT-safe)
            SqliteConnectionFactory.AddParameters(param, command);

            int affectedRows = await command.ExecuteNonQueryAsync();

            await transaction.CommitAsync();

            // Wykonuje zapytanie i zwraca liczbę zmodyfikowanych wierszy (np. 1 jeśli dodano użytkownika)
            return affectedRows;
        }

        // Wersja BEZ parametrów - dla operacji typu: _db.ExecuteAsync(sql)
        public async Task<int> ExecuteAsync(string sql)
        {
            // Przekierowujemy do metody powyżej, podając pusty obiekt jako TParam
            return await ExecuteAsync(sql, null!);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object?> param)
        {
            var results = new List<T>();
            // Pobieramy napisany przez Ciebie mapper dla typu T
            var mapFunction = DbMappers.Get<T>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();
            await using var command = connection.CreateCommand();
            command.CommandText = sql;

            // Bindowanie parametrów z obiektu anonimowego
            SqliteConnectionFactory.AddParameters(param, command);

            await using var reader = await command.ExecuteReaderAsync();

            object? ordinalsInstance = null;
            if (reader.HasRows)
            {
                // Dynamiscznie szukamy Twojej zagnieżdżonej klasy indeksów (np. LogsOrdinals)
                var ordinalsType = typeof(DbMappers).GetNestedType($"{typeof(T).Name}Ordinals");
                if (ordinalsType != null)
                {
                    // Tworzymy klasę ordinals JEDEN raz przed pętlą – tu odpali się Twój GetOrdinal
                    ordinalsInstance = Activator.CreateInstance(ordinalsType, reader);
                }
            }

            // Błyskawiczna pętla bez narzutu na GetOrdinal
            while (await reader.ReadAsync())
            {
                results.Add(mapFunction(reader, ordinalsInstance!));
            }

            return results;
        }

        // Przeciążenie dla zapytań bez parametrów
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
        {
            return await QueryAsync<T>(sql, null!);
        }


        public async Task<IMyDbAsyncTransaction> BeginTransactionAsync()
        {
            // Tworzymy nowe połączenie dedykowane dla tej transakcji
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            // Uruchamiamy transakcję na poziomie bazy danych
            var transaction = connection.BeginTransaction();

            // Zwracamy obiekt opakowujący
            return new MyDbAsyncTransaction(connection, transaction);
        }
    }
}
