using DataBase.Model;

using Microsoft.Data.Sqlite;

public class DatabaseBackupService : BackgroundService
{
    private readonly string _connectionString;
    private readonly string _persistentDbPath;
    private readonly string _tempBackupPath;
    private readonly ILogger<DatabaseBackupService> _logger;
    // Zmieniono na bezpieczne 2 minuty – optymalny balans dla Cloud Run
    private readonly TimeSpan _backupInterval = TimeSpan.FromMinutes(1);
    private int _lastDataVersion = 0;

    public DatabaseBackupService(string connectionString, string persistentDbPath, ILogger<DatabaseBackupService> logger)
    {
        _connectionString = connectionString;
        _persistentDbPath = persistentDbPath;
        _tempBackupPath = persistentDbPath + ".tmp";
        _logger = logger;
    }

    // --- GŁÓWNA PĘTLA USŁUGI (W TRAKCIE DZIAŁANIA APLIKACJI) ---
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Usługa automatycznego backupu SQLite (PeriodicTimer) wystartowała.");

        using var timer = new PeriodicTimer(_backupInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                // Wywołujemy naszą wspólną logikę sprawdzania zmian i czyszczenia JSON-ów
                await CheckCleanAndBackupAsync(cancellationToken: stoppingToken, forceWrite: false);
            }
            catch (OperationCanceledException)
            {
                // Ignorujemy – to naturalny sygnał zamknięcia w pętli
            }
            catch (Exception ex)
            {
                _logger.LogError("Błąd podczas automatycznej synchronizacji bazy: {message}", ex.Message);
            }
        }
    }

    // --- KLUCZOWA MODYFIKACJA: URUCHAMIANE PRZY WYŁĄCZANIU PRZEZ CLOUD RUN ---
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogWarning("Otrzymano sygnał wyłączenia kontenera (SIGTERM). Wymuszam ostateczny backup i czyszczenie danych...");

        try
        {
            // Wymuszamy zapis (forceWrite: true) – bez względu na data_version, 
            // chcemy mieć pewność, że najświeższa baza z /tmp trafi do pamięci trwałej przed zniszczeniem kontenera.
            await CheckCleanAndBackupAsync(cancellationToken: cancellationToken, forceWrite: true);
            _logger.LogInformation("Ostateczny backup i czyszczenie zakończone sukcesem.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Błąd krytyczny podczas ostatecznego backupu na wyłączenie: {message}", ex.Message);
        }

        // Pozwalamy klasie bazowej dokończyć procedurę zamykania
        await base.StopAsync(cancellationToken);
    }

    // --- WSPÓLNA LOGIKA PROCESU BAZODANOWEGO ---
    private async Task CheckCleanAndBackupAsync(bool forceWrite, CancellationToken cancellationToken)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync(cancellationToken);

            // 1. Sprawdzamy wersję danych w SQLite
            using var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "PRAGMA data_version;";
            int currentDataVersion = Convert.ToInt32(await checkCmd.ExecuteScalarAsync(cancellationToken));

            // Jeśli nic się nie zmieniło i nie wymuszamy zapisu, pomijamy cykl
            if (currentDataVersion == _lastDataVersion && !forceWrite)
            {
                return;
            }

            // 2. CZYSZCZENIE: Zerujemy JSON-y starsze niż 30 dni w oryginalnej bazie przed zrobieniem jej kopii
            using var cleanCmd = connection.CreateCommand();

            string sql = $@"
                UPDATE {nameof(UpdateLog)}  
                SET {nameof(UpdateLog.JsonUpdate)} = NULL
                WHERE
                {nameof(UpdateLog.JsonUpdate)} IS NOT NULL
                AND
                {nameof(UpdateLog.UpdatedTicks)} <= {DateTime.UtcNow.AddDays(-30).Ticks}
                ";

            cleanCmd.CommandText = sql;

            int affectedRows = await cleanCmd.ExecuteNonQueryAsync(cancellationToken);
            if (affectedRows > 0)
            {
                _logger.LogInformation("Wyczyszczono stare JSON-y. Liczba wierszy: {count}", affectedRows);
            }

            // Usuwamy stary plik tymczasowy, jeśli istnieje z poprzedniego nieudanego cyklu
            if (File.Exists(_tempBackupPath))
            {
                File.Delete(_tempBackupPath);
            }

            // 3. BACKUP: Wykonujemy bezpieczną kopię bazy metodą VACUUM INTO
            using var backupCmd = connection.CreateCommand();
            backupCmd.CommandText = $"VACUUM INTO '{_tempBackupPath}';";
            await backupCmd.ExecuteNonQueryAsync(cancellationToken);

            _lastDataVersion = currentDataVersion; // Zapisujemy nową wersję jako zsynchronizowaną
        }

        // 4. Podmieniamy plik bazy na dysku trwałym
        if (File.Exists(_tempBackupPath))
        {
            File.Move(_tempBackupPath, _persistentDbPath, overwrite: true);
            _logger.LogInformation("Plik bazy danych został pomyślnie zaktualizowany na dysku trwałym.");
        }
    }
}
