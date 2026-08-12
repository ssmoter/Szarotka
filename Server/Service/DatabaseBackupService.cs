using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

public class DatabaseBackupService : BackgroundService
{
    private readonly string _connectionString;
    private readonly string _persistentDbPath;
    private readonly string _tempBackupPath;
    private readonly ILogger<DatabaseBackupService> _logger;
    private readonly TimeSpan _backupInterval = TimeSpan.FromSeconds(5); // Synchronizacja co 5 sekund

    public DatabaseBackupService(string connectionString, string persistentDbPath, ILogger<DatabaseBackupService> _logger)
    {
        _connectionString = connectionString;
        _persistentDbPath = persistentDbPath;
        _tempBackupPath = persistentDbPath + ".tmp";
        this._logger = _logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Usługa automatycznego backupu SQLite (PeriodicTimer) wystartowała.");

        // 1. Inicjalizacja timera na 5 sekund
        using var timer = new PeriodicTimer(_backupInterval);
        int ostatniaWersja = 0;

        // 2. Pętla wykonuje się dopóki chmura nie wyłączy serwera
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                // Wykonujemy bezpieczną kopię bazy w locie
                using (var connection = new Microsoft.Data.Sqlite.SqliteConnection(_connectionString))
                {
                    await connection.OpenAsync(stoppingToken);

                    // Sprawdzamy wersję danych w SQLite
                    using var checkCmd = connection.CreateCommand();
                    checkCmd.CommandText = "PRAGMA data_version;";
                    int aktualnaWersja = Convert.ToInt32(await checkCmd.ExecuteScalarAsync(stoppingToken));

                    // Jeśli aktualnaWersja == ostatniaWersja, oznacza to, że NIKT nic nie zapisał!
                    if (aktualnaWersja == ostatniaWersja)
                    {
                        continue; // Pomijamy ten cykl, nie robimy drogiego VACUUM INTO
                    }

                    // Jeśli są zmiany - wykonujemy kopię bezpieczeństwa
                    using var cmd = connection.CreateCommand();
                    cmd.CommandText = $"VACUUM INTO '{_tempBackupPath}';";
                    await cmd.ExecuteNonQueryAsync(stoppingToken);

                    ostatniaWersja = aktualnaWersja; // Zapisujemy nową wersję jako zsynchronizowaną
                }

                if (File.Exists(_tempBackupPath))
                {
                    File.Move(_tempBackupPath, _persistentDbPath, overwrite: true);
                }
            }
            catch (OperationCanceledException)
            {
                // Ignorujemy – to naturalny sygnał zamknięcia aplikacji przez Google Cloud
            }
            catch (Exception ex)
            {
                _logger.LogError($"Błąd podczas automatycznej synchronizacji bazy: {ex.Message}");
            }
        }
    }
}
