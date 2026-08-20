using DataBase.Data;
using DataBase.Data.MySqliteConnection;
using DataBase.Service;

using Microsoft.Extensions.Logging;

using Moq;

namespace DataBaseUnitTest.DataSave
{
    public static class Helper
    {
        public static string GetPath => AppDomain.CurrentDomain.BaseDirectory;
        public static async void Delete(string dbName)
        {
            if (System.IO.File.Exists(dbName))
            {
                // 1. Zmuszamy sterownik Microsoftu do zamknięcia wszelkich uśpionych połączeń w pamięci podręcznej
                Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();

                // 2. Próbujemy usunąć plik maksymalnie 10 razy, czekając po 50ms między próbami
                bool deleted = false;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        System.IO.File.Delete(dbName);

                        // W trybie WAL SQLite tworzy też pliki tymczasowe, je też warto sprzątnąć, jeśli istnieją
                        if (System.IO.File.Exists($"{dbName}-wal")) System.IO.File.Delete($"{dbName}-wal");
                        if (System.IO.File.Exists($"{dbName}-shm")) System.IO.File.Delete($"{dbName}-shm");

                        deleted = true;
                        break; // Sukces, plik usunięty, wychodzimy z pętli for
                    }
                    catch (System.IO.IOException)
                    {
                        // Plik wciąż jest blokowany przez OS - czekamy 50ms i próbujemy ponownie
                        await Task.Delay(50);
                    }
                }

                // 3. Jeśli po 10 próbach nadal plik jest zablokowany, wyrzucamy czytelny błąd
                if (!deleted)
                {
                    throw new System.IO.IOException($"Nie można wyczyścić bazy danych '{dbName}', ponieważ plik jest stale zablokowany przez proces testowy.");
                }
            }

            //if (System.IO.File.Exists(dbName))
            //{
            //    System.IO.File.Delete(dbName);
            //}
        }
        public static string AddDBIfDontHave(string dbName)
        {
            return dbName.EndsWith(".db3", StringComparison.OrdinalIgnoreCase) ? dbName : dbName + ".db3";
        }
        public static async Task<AccessDataBaseAoT> CreateDataBaseInventoryForTest(string dbName)
        {
            return await CreatedDataBaseUpdateLogForTest(dbName);
        }
        public static async Task<AccessDataBaseAoT> CreateDataBaseRoutesForTest(string dbName)
        {
            return await CreatedDataBaseUpdateLogForTest(dbName);
        }
        public static async Task<AccessDataBaseAoT> CreatedDataBaseUpdateLogForTest(string dbName)
        {
            dbName = AddDBIfDontHave(dbName);
            Delete(dbName);

            var factory = new SqliteConnectionFactory(dbName);
            var sync = new MyDbConnection(factory);
            var async = new MyDbAsyncConnection(factory);
            var adb = new AccessDataBase(dbName, new CurrentUtc());
            var log = new Mock<ILogger<AccessDataBaseAoT>>();

            var db = new AccessDataBaseAoT(adb, sync, async, new CurrentUtc(), log.Object);
            var update = new Shared.Data.CreatedDataBase(db);
            await update.UpdateDataBase(data, data, data);
            static void data(double a, int b) { }
            ;
            return db;
        }
    }
}
