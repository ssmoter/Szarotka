// 1. Interfejs dla aktywnej transakcji
using DataBase.Mappers;

using Microsoft.Data.Sqlite;
namespace DataBase.Data.MySqliteConnection;

public interface IMyDbAsyncTransaction : IAsyncDisposable, IDisposable
{
    Task<int> ExecuteAsync(string sql, object? param = null);
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
    Task CommitAsync();
    Task RollbackAsync();
}

// 2. Implementacja transakcji
public class MyDbAsyncTransaction : IMyDbAsyncTransaction
{
    private readonly SqliteConnection _connection;
    private SqliteTransaction? _transaction;

    public MyDbAsyncTransaction(SqliteConnection connection, SqliteTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<int> ExecuteAsync(string sql, object? param = null)
    {
        if (_transaction == null) throw new InvalidOperationException("Transaction closed.");

        // Bezpośrednie użycie SqliteCommand
        await using var command = _connection.CreateCommand();
        command.CommandText = sql;
        command.Transaction = _transaction;

        SqliteConnectionFactory.AddParameters(param, command);
        return await command.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        if (_transaction == null) throw new InvalidOperationException("Transaction closed.");

        var results = new List<T>();
        var mapFunction = DbMappers.Get<T>();

        // Bezpośrednie użycie SqliteCommand
        await using var command = _connection.CreateCommand();
        command.CommandText = sql;
        command.Transaction = _transaction;

        SqliteConnectionFactory.AddParameters(param, command);

        await using var reader = await command.ExecuteReaderAsync();

        object? ordinalsInstance = null;
        if (reader.HasRows)
        {
            var ordinalsType = typeof(DbMappers).GetNestedType($"{typeof(T).Name}Ordinals");
            if (ordinalsType != null)
            {
                ordinalsInstance = Activator.CreateInstance(ordinalsType, reader);
            }
        }

        while (await reader.ReadAsync())
        {
            results.Add(mapFunction(reader, ordinalsInstance!));
        }

        return results;
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await DisposeTransactionAsync();
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
        await _connection.DisposeAsync();
    }

    public async ValueTask DisposeAsync() => await DisposeTransactionAsync();
    public void Dispose() { _transaction?.Dispose(); _connection.Dispose(); }
}
