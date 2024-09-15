using CommunityToolkit.Maui.Alerts;

using DataBase.Model;
using DataBase.Service;

namespace DataBase.Data
{
    public class CreatedDataBase : ICreatedDataBase, IUpdateDataBase
    {
        private readonly AccessDataBase _db;
        private readonly InventoryTables _inventoryTables;
        private readonly DriversRoutesTables _driversRoutesTables;

        public CreatedDataBase(AccessDataBase accessData)
        {
            this._db = accessData;
            _inventoryTables ??= new InventoryTables(_db);
            _driversRoutesTables ??= new DriversRoutesTables(_db);
        }
        public DataBaseVersion GetCurrentVersion()
        {

            var tableInfo = _db.DataBase.GetTableInfo(nameof(DataBaseVersion));

            bool exist = tableInfo.Count > 0;
            if (!exist)
            {
                _db.DataBase.CreateTable<DataBaseVersion>();
            }

            var version = _db.DataBase.Table<DataBaseVersion>().FirstOrDefault();

            version ??= new DataBaseVersion()
            {
                DataBase = 0,
                DriversRoutes = 0,
                Inventory = 0
            };
            _db.DataBase.InsertOrReplace(version);
            return version;

        }
        public async Task CreateBackUp()
        {

            var lastUpdate = await _db.DataBaseAsync.Table<DataBaseVersion>().FirstOrDefaultAsync();
            if (lastUpdate.LastBackup >= DateTime.Today.Ticks)
            {
                return;
            }
            lastUpdate.LastBackup = DateTime.Today.Ticks;
            await _db.DataBaseAsync.BackupAsync(Helper.Constants.BackupPath);
            var update = _db.DataBaseAsync.UpdateAsync(lastUpdate);
            var toast = Toast.Make("Utworzono kopie bazy danych", duration: CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
            await Task.WhenAll(update, toast);
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


            updateDataBase?.Invoke(1, newVersion.DataBase);
            await _db.DataBaseAsync.InsertOrReplaceAsync(newVersion);

            return true;
        }

        public async Task Update(int oldVersion, int newVersion, Action<double, int> updateAction)
        {
            double progressBar = 0;
            double updateProgressBar = newVersion - oldVersion;
            updateProgressBar /= updateProgressBar.ToString().Length * 10;


            if (oldVersion < 1)
            {
                await _db.DataBaseAsync.CreateTableAsync<LogsModel>();
                progressBar += updateProgressBar;
                oldVersion = 1;
                updateAction?.Invoke(progressBar, oldVersion);
            }

        }
    }
}
