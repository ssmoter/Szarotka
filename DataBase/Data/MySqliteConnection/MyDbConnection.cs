using DataBase.Mappers;

using Microsoft.Data.Sqlite;

namespace DataBase.Data.MySqliteConnection
{
    public interface IMyDbConnection
    {
        void BackupDatabase(SqliteConnection destination);
        IMyDbTransaction BeginTransaction();
        int Execute(string sql, object? param);
        int Execute(string sql);
        IEnumerable<T> Query<T>(string sql, object? param);
        IEnumerable<T> Query<T>(string sql);
    }
    public class MyDbConnection : IMyDbConnection
    {
        private readonly ISqliteConnectionFactory _connectionFactory;

        public MyDbConnection(ISqliteConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        // 1. WERSJA Z PARAMETRAMI (Dla zapytań typu: _db.Query<Model>(sql, new { Id }))
        public IEnumerable<T> Query<T>(string sql, object? param)
        {
            var results = new List<T>();
            // Pobieramy Twój własny, rozbity na pliki mapper z rejestru DbMappers
            var mapFunction = DbMappers.Get<T>();

            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = sql;

            // Mapowanie parametrów z obiektu anonimowego (100% bezpieczne dla AOT)
            SqliteConnectionFactory.AddParameters(param, command);


            using var reader = command.ExecuteReader();

            // --- OPTYMALIZACJA PRZED PĘTLĄ (GetOrdinal uruchomi się tylko raz!) ---
            object? ordinalsInstance = null;
            if (reader.HasRows)
            {
                // Dynamicznie szukamy Twojej klasy indeksów (np. LogsOrdinals) w plikach partial
                var ordinalsType = typeof(DbMappers).GetNestedType($"{typeof(T).Name}Ordinals");
                if (ordinalsType != null)
                {
                    // Tworzymy obiekt indeksów - tu odpali się konstruktor z Twoim GetOrdinal
                    ordinalsInstance = Activator.CreateInstance(ordinalsType, reader);
                }
            }

            // --- ULTRA SZYBKA PĘTLA GŁÓWNA ---
            while (reader.Read())
            {
                // Przekazujemy surowy reader oraz przygotowane wcześniej indeksy do Twojego mappera
                results.Add(mapFunction(reader, ordinalsInstance!));
            }

            return results;
        }

        // 2. WERSJA BEZ PARAMETRÓW (Dla zapytań typu: _db.Query<Model>(sql))
        public IEnumerable<T> Query<T>(string sql)
        {
            // Przekierowujemy do metody z parametrami, podając pusty obiekt jako TParam
            return Query<T>(sql, null!);
        }
        public int Execute(string sql, object? param)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable);

            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = transaction;

            SqliteConnectionFactory.AddParameters(param, command);
            int affectedRows = command.ExecuteNonQuery();

            transaction.Commit();

            return affectedRows;
        }

        public int Execute(string sql)
        {
            return Execute(sql, null!);
        }

        public void BackupDatabase(SqliteConnection destination)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();
            connection.BackupDatabase(destination);
        }


        public IMyDbTransaction BeginTransaction()
        {
            // Rzutujemy na SqliteConnection, jeśli fabryka zwraca typ bazowy DbConnection
            var connection = _connectionFactory.CreateConnection();
            connection.Open();

            var transaction = connection.BeginTransaction();

            return new MyDbTransaction(connection, transaction);
        }
    }
}
