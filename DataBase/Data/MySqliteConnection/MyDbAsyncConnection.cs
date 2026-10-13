using DataBase.Mappers;

namespace DataBase.Data.MySqliteConnection
{
    public interface IMyDbAsyncConnection
    {
        Task<IMyDbAsyncTransaction> BeginTransactionAsync(CancellationToken token = default);
        Task<int> ExecuteAsync(string sql, Dictionary<string, object?> param, CancellationToken token = default);
        Task<int> ExecuteAsync(string sql, CancellationToken token = default);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object?> param, CancellationToken token = default);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken token = default);
    }
    public class MyDbAsyncConnection : IMyDbAsyncConnection
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public MyDbAsyncConnection(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Wersja Z parametrami (TParam) - dla operacji typu: _db.ExecuteAsync(sql, new { Id })
        public async Task<int> ExecuteAsync(string sql, Dictionary<string, object?> param, CancellationToken token = default)
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(token);

            await using var transaction = await connection.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, token);

            await using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = (Microsoft.Data.Sqlite.SqliteTransaction)transaction;

            // Automatyczne dodawanie parametrów z obiektu anonimowego (AOT-safe)
            SqliteConnectionFactory.AddParameters(param, command);

            int affectedRows = await command.ExecuteNonQueryAsync(token);

            await transaction.CommitAsync(token);

            // Wykonuje zapytanie i zwraca liczbę zmodyfikowanych wierszy (np. 1 jeśli dodano użytkownika)
            return affectedRows;
        }

        // Wersja BEZ parametrów - dla operacji typu: _db.ExecuteAsync(sql)
        public async Task<int> ExecuteAsync(string sql, CancellationToken token = default)
        {
            // Przekierowujemy do metody powyżej, podając pusty obiekt jako TParam
            return await ExecuteAsync(sql, null!, token);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, Dictionary<string, object?> param, CancellationToken token = default)
        {
            var results = new List<T>();
            // Pobieramy napisany przez Ciebie mapper dla typu T
            var mapFunction = DbMappers.Get<T>();

            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(token);
            await using var command = connection.CreateCommand();
            command.CommandText = sql;

            // Bindowanie parametrów z obiektu anonimowego
            SqliteConnectionFactory.AddParameters(param, command);

            await using var reader = await command.ExecuteReaderAsync(token);

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
            while (await reader.ReadAsync(token))
            {
                results.Add(mapFunction(reader, ordinalsInstance!));
            }

            return results;
        }

        // Przeciążenie dla zapytań bez parametrów
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, CancellationToken token = default)
        {
            return await QueryAsync<T>(sql, null!, token);
        }


        public async Task<IMyDbAsyncTransaction> BeginTransactionAsync(CancellationToken token = default)
        {
            // Tworzymy nowe połączenie dedykowane dla tej transakcji
            var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(token);

            // Uruchamiamy transakcję na poziomie bazy danych
            var transaction = connection.BeginTransaction();

            // Zwracamy obiekt opakowujący
            return new MyDbAsyncTransaction(connection, transaction);
        }
    }
}
