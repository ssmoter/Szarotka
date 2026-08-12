using DataBase.Helper;

using Microsoft.Data.Sqlite;

using System.Diagnostics.CodeAnalysis;

namespace DataBase.Data.MySqliteConnection
{
    public interface ISqliteConnectionFactory
    {
        // Metoda zwraca gotowy obiekt połączenia
        SqliteConnection CreateConnection();
    }

    public class SqliteConnectionFactory : ISqliteConnectionFactory
    {
        private readonly string _connectionString;

        public SqliteConnectionFactory(string path)
        {
            CreatedFolderPath();
            _connectionString = SetConnetioncString(path);
        }
        public SqliteConnectionFactory()
        {
            CreatedFolderPath();
            // Centralne zarządzanie ścieżką do bazy danych .3db w bezpiecznym folderze MAUI
            _connectionString = SetConnetioncString(Constants.DatabasePath);
        }

        private static string SetConnetioncString(string path)
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = path,
                Mode = SqliteOpenMode.ReadWriteCreate,
                Pooling = false,
                DefaultTimeout = 10,
                ForeignKeys = true,
                
            };
            return builder.ConnectionString;
        }
        public SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // Wstrzykujemy brakujące parametry bazy, których builder nie obsługuje
            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA journal_mode='wal'; PRAGMA busy_timeout = 10000;"; // Milisekundy dla SQLite
            command.ExecuteNonQuery();

            // Zwracamy nową instancję połączenia ze skonfigurowanym connection stringiem
            return new SqliteConnection(_connectionString);
        }

        private static void CreatedFolderPath()
        {
            if (!Directory.Exists(Constants.GetPathFolder))
            {
                Directory.CreateDirectory(Constants.GetPathFolder);
            }
        }

        public static void AddParameters(Dictionary<string, object?>? param, Microsoft.Data.Sqlite.SqliteCommand command)
        {
            if (param != null)
            {
                foreach (var kvp in param)
                {
                    var value = kvp.Value ?? DBNull.Value;

                    // WYJĄTEK DLA GUID: Jeśli wartość to Guid, zamień ją na string
                    if (value is Guid guidValue)
                    {
                        value = guidValue.ToString();
                    }
                    // Opcjonalnie: Jeśli używasz Nullable<Guid> (Guid?), obsłuż też sytuację gdy ma wartość
                    else if (value is Guid?)
                    {
                        value = ((Guid)value).ToString();
                    }
                    command.Parameters.AddWithValue($"@{kvp.Key}", value);
                }
            }
        }




    }
}
