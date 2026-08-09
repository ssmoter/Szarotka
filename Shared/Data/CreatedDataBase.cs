using CommunityToolkit.Maui.Alerts;

using DataBase.Data;
using DataBase.Model;
using DataBase.Service;

using Microsoft.Data.Sqlite;

using Shared.Model;
using Shared.Service;

namespace Shared.Data
{
    public class CreatedDataBase : ICreatedDataBase, IUpdateDataBase
    {
        private readonly IAccessDataBaseAoT _db;
        private readonly InventoryTables _inventoryTables;
        private readonly DriversRoutesTables _driversRoutesTables;
        private readonly IUpdateLogService _updateLogService;
        public CreatedDataBase(IAccessDataBaseAoT accessData, IUpdateLogService updateLogService = null)
        {
            this._db = accessData;
            _inventoryTables ??= new InventoryTables(_db);
            _driversRoutesTables ??= new DriversRoutesTables(_db);
            if (updateLogService is not null)
            {
                _updateLogService = updateLogService;
            }
            _updateLogService ??= new UpdateLogService(_db, new CurrentUtc());
        }

        const string SQlCreatedDataBaseVersion = $@"CREATE TABLE IF NOT EXISTS [{nameof(DataBaseVersion)}] (" +
           $"[{nameof(DataBaseVersion.Id)}] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
           $"[{nameof(DataBaseVersion.DataBase)}] INTEGER NOT NULL, " +
           $"[{nameof(DataBaseVersion.DriversRoutes)}] INTEGER NOT NULL, " +
           $"[{nameof(DataBaseVersion.Inventory)}] INTEGER NOT NULL, " +
           $"[{nameof(DataBaseVersion.LastBackup)}] INTEGER NOT NULL)";

        const string SQlSelectDataBaseVersion = $@"SELECT 
            [{nameof(DataBaseVersion.Id)}],
            [{nameof(DataBaseVersion.DataBase)}],
            [{nameof(DataBaseVersion.DriversRoutes)}],
            [{nameof(DataBaseVersion.Inventory)}],
            [{nameof(DataBaseVersion.LastBackup)}]
        FROM [{nameof(DataBaseVersion)}]";

        const string SQlInsertOrReplaceDataBaseVersion = $@"INSERT INTO [{nameof(DataBaseVersion)}] (
            [{nameof(DataBaseVersion.Id)}],
            [{nameof(DataBaseVersion.DataBase)}],
            [{nameof(DataBaseVersion.DriversRoutes)}],
            [{nameof(DataBaseVersion.Inventory)}],
            [{nameof(DataBaseVersion.LastBackup)}]
        ) VALUES (
            @{nameof(DataBaseVersion.Id)},
            @{nameof(DataBaseVersion.DataBase)},
            @{nameof(DataBaseVersion.DriversRoutes)},
            @{nameof(DataBaseVersion.Inventory)},
            @{nameof(DataBaseVersion.LastBackup)}
        )
        ON CONFLICT([{nameof(DataBaseVersion.Id)}]) DO UPDATE SET
            [{nameof(DataBaseVersion.DataBase)}] = excluded.[{nameof(DataBaseVersion.DataBase)}],
            [{nameof(DataBaseVersion.DriversRoutes)}] = excluded.[{nameof(DataBaseVersion.DriversRoutes)}],
            [{nameof(DataBaseVersion.Inventory)}] = excluded.[{nameof(DataBaseVersion.Inventory)}],
            [{nameof(DataBaseVersion.LastBackup)}] = excluded.[{nameof(DataBaseVersion.LastBackup)}];";
        public DataBaseVersion GetCurrentVersion()
        {
            _db.DbSyncAoT.Execute(SQlCreatedDataBaseVersion);

            var version = _db.DbSyncAoT.Query<DataBaseVersion>(SQlSelectDataBaseVersion).FirstOrDefault();

            version ??= new DataBaseVersion()
            {
                DataBase = 0,
                DriversRoutes = 0,
                Inventory = 0
            };
            _db.DbSyncAoT.Execute(SQlCreatedDataBaseVersion);

            UpdateCurrentVersion(version);
            return version;
        }

        public void UpdateCurrentVersion(DataBaseVersion version)
        {
            _db.DbSyncAoT.Execute(SQlInsertOrReplaceDataBaseVersion, version);
        }
        public async Task CreateBackUp()
        {
            var lastUpdate = GetCurrentVersion();
            if (lastUpdate.LastBackup >= DateTime.Today.Ticks)
            {
                return;
            }
            lastUpdate.LastBackup = _db.TimeService.UtcNow().Ticks;

            string backupConnectionString = $"Data Source={DataBase.Helper.Constants.BackupPath}";
            using var backupConnection = new SqliteConnection(backupConnectionString);
            backupConnection.Open();
            _db.DbSyncAoT.BackupDatabase(backupConnection);

            UpdateCurrentVersion(lastUpdate);
            await Toast.Make("Utworzono kopie bazy danych", duration: CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
        }

        public async Task<bool> UpdateDataBase(Action<double, int> updateDataBase, Action<double, int> updateInventory, Action<double, int> updateDriverRoutes)
        {
            var oldVersion = GetCurrentVersion();
            var newVersion = new DataBaseVersion();

            //zawsze pierwsze
            await Update(oldVersion.DataBase, newVersion.DataBase, updateDataBase);

            var inventory = _inventoryTables.Update(oldVersion.Inventory, newVersion.Inventory, updateInventory);
            var driversRoutes = _driversRoutesTables.Update(oldVersion.DriversRoutes, newVersion.DriversRoutes, updateDriverRoutes);
            await Task.WhenAll(inventory, driversRoutes);

            UpdateCurrentVersion(newVersion);
            return true;
        }


        const string SQlCreatedLogsModel = $@"CREATE TABLE IF NOT EXISTS [{nameof(LogsModel)}] (
            [{nameof(LogsModel.Id)}] INTEGER PRIMARY KEY AUTOINCREMENT ,
            [{nameof(LogsModel.StackTrace)}] TEXT ,
            [{nameof(LogsModel.Message)}] TEXT ,
            [{nameof(LogsModel.Created)}] TEXT )";

        const string SQlCreatedHelperTable = $@"CREATE TABLE IF NOT EXISTS [{nameof(HelperTable)}] (
            [{nameof(HelperTable.Id)}] TEXT PRIMARY KEY ,
            [{nameof(HelperTable.Name)}] TEXT UNIQUE,
            [{nameof(HelperTable.Value)}] TEXT ,
            {HelperTable.AdditionalColumns})";

        const string SQlCreateUpdateLog = $@"CREATE TABLE IF NOT EXISTS [{nameof(UpdateLog)}] (
            [{nameof(UpdateLog.Id)}] TEXT PRIMARY KEY ,
            [{nameof(UpdateLog.UpdateEnum)}] INTEGER ,
            [{nameof(UpdateLog.UpdateId)}] TEXT ,
            [{nameof(UpdateLog.JsonUpdate)}] TEXT ,
            [{nameof(UpdateLog.IsServer)}] INTEGER ,
            {HelperTable.AdditionalColumns})";


        public async Task Update(int oldVersion, int newVersion, Action<double, int> updateAction)
        {
            double progressBar = 0;
            double updateProgressBar = newVersion - oldVersion;
            updateProgressBar /= updateProgressBar.ToString().Length * 10;


            if (oldVersion < 1)
            {
                ConfiguredDatabase();
                await _db.DbAsyncAoT.ExecuteAsync(SQlCreatedLogsModel);
                await _db.DbAsyncAoT.ExecuteAsync(SQlCreatedHelperTable);
                await _db.DbAsyncAoT.ExecuteAsync(SQlCreateUpdateLog);

                progressBar += updateProgressBar;
                oldVersion = 1;
                updateAction?.Invoke(progressBar, oldVersion);
            }
            if (oldVersion < 2)
            {
                progressBar += updateProgressBar;
                oldVersion = 2;
                updateAction?.Invoke(progressBar, oldVersion);
            }
            if (oldVersion < 3)
            {
                progressBar += updateProgressBar;
                oldVersion = 3;
                updateAction?.Invoke(progressBar, oldVersion);
            }


            updateAction?.Invoke(1, oldVersion);
        }




        private void ConfiguredDatabase()
        {
            var sql = "PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL; PRAGMA cache_size=-2000;";
            _db.DbSyncAoT.Execute(sql);
        }



        public static DataBaseVersion GetDataBaseVersion(IAccessDataBaseAoT db)
        {
            var version = db.DbSyncAoT.Query<DataBaseVersion>(SQlSelectDataBaseVersion).FirstOrDefault();
            return version;
        }
        public static IEnumerable<TableInfo> GetTableInfo(IAccessDataBaseAoT db, string tableName)
        {
            var query = $"PRAGMA table_info('{tableName}')";
            var result = db.DbSyncAoT.Query<TableInfo>(query);
            return result;
        }
    }
}
