using DataBase.Mappers;

using Microsoft.Data.Sqlite;
namespace DataBase.Data.MySqliteConnection
{

    // Interfejs dla aktywnej transakcji synchronicznej
    public interface IMyDbTransaction : IDisposable
    {
        int Execute(string sql, Dictionary<string, object?> param);
        IEnumerable<T> Query<T>(string sql, Dictionary<string, object?> param);
        void Commit();
        void Rollback();
    }

    // Implementacja transakcji specyficzna dla Microsoft.Data.Sqlite
    public class MyDbTransaction : IMyDbTransaction
    {
        private readonly SqliteConnection _connection;
        private SqliteTransaction? _transaction;

        public MyDbTransaction(SqliteConnection connection, SqliteTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public int Execute(string sql, Dictionary<string, object?> param)
        {
            if (_transaction == null) throw new InvalidOperationException("Transaction closed.");

            using var command = _connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = _transaction;

            SqliteConnectionFactory.AddParameters(param, command);
            return command.ExecuteNonQuery();
        }

        public IEnumerable<T> Query<T>(string sql, Dictionary<string, object?> param)
        {
            if (_transaction == null) throw new InvalidOperationException("Transaction closed.");

            var results = new List<T>();
            var mapFunction = DbMappers.Get<T>();

            using var command = _connection.CreateCommand();
            command.CommandText = sql;
            command.Transaction = _transaction;

            SqliteConnectionFactory.AddParameters(param, command);

            using var reader = command.ExecuteReader();

            object? ordinalsInstance = null;
            if (reader.HasRows)
            {
                var ordinalsType = typeof(DbMappers).GetNestedType($"{typeof(T).Name}Ordinals");
                if (ordinalsType != null)
                {
                    ordinalsInstance = Activator.CreateInstance(ordinalsType, reader);
                }
            }

            while (reader.Read())
            {
                results.Add(mapFunction(reader, ordinalsInstance!));
            }

            return results;
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                DisposeTransaction();
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                DisposeTransaction();
            }
        }
        private void DisposeTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
            _connection.Dispose();
        }

        public void Dispose() => DisposeTransaction();
    }

}
