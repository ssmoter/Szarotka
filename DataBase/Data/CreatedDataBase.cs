using DataBase.Model;
using DataBase.Service;

namespace DataBase.Data
{
    public class CreatedDataBase : ICreatedDataBase
    {
        readonly AccessDataBase _db;
        readonly InventoryTables _inventoryTables;
#if ANDROID
        readonly DriversRoutesTables _driversRoutesTables;
#endif

        public CreatedDataBase(AccessDataBase accessData)
        {
            this._db = accessData;
            _inventoryTables ??= new InventoryTables(_db);
#if ANDROID
            _driversRoutesTables ??= new DriversRoutesTables(_db);
#endif
        }

        public async Task<bool> UpdateDataBase(Action<double, int> uppdateDataBase, Action<double, int> uppdateInventory, Action<double, int> uppdateDriverRoutes)
        {
            var oldVersion = GetCurrentVersion();
            var newVersion = new DataBaseVersion();


            double progressBar = 0;
            double uppdateProgressBar = newVersion.DataBase - oldVersion.DataBase;
            uppdateProgressBar /= uppdateProgressBar.ToString().Length * 10;


            if (oldVersion.DataBase < 1)
            {
                await _db.DataBaseAsync.CreateTableAsync<LogsModel>();
                progressBar += uppdateProgressBar;
                oldVersion.DataBase = 1;
                uppdateDataBase?.Invoke(progressBar, oldVersion.DataBase);
            }



            await _inventoryTables.UpdateInventory(oldVersion.Inventory, newVersion.Inventory, uppdateInventory);
#if ANDROID
           await _driversRoutesTables.UpdateDriversRoutes(oldVersion.DriversRoutes,newVersion.DriversRoutes,uppdateDriverRoutes);
#endif
            uppdateDataBase?.Invoke(1, newVersion.DataBase);
            await _db.DataBaseAsync.InsertOrReplaceAsync(newVersion);

            return true;
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

            if (version == null)
            {
                version = new DataBaseVersion()
                {
                    DataBase = 0,
                    DriversRoutes = 0,
                    Inventory = 0
                };
            }
            _db.DataBase.InsertOrReplace(version);
            return version;
        }


    }
}
