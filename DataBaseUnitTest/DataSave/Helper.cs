using DataBase.Data;

namespace DataBaseUnitTest.DataSave
{
    public static class Helper
    {
        public static string GetPath => AppDomain.CurrentDomain.BaseDirectory;
        public static void Delete(string dbName)
        {
            if (System.IO.File.Exists(dbName))
            {
                System.IO.File.Delete(dbName);
            }
        }
        public static async Task<AccessDataBase> CreateDataBaseInventoryForTest(string dbName)
        {
            var db = new AccessDataBase(dbName);
            var update = new Shared.Data.CreatedDataBase(db);
            await update.UpdateDataBase(data, data, data);
            static void data(double a, int b) { }
            ;
            return db;
        }
        public static async Task<AccessDataBase> CreateDataBaseRoutesForTest(string dbName)
        {
            var db = new AccessDataBase(dbName);
            var update = new Shared.Data.CreatedDataBase(db);
            await update.UpdateDataBase(data, data, data);
            static void data(double a, int b) { }
            ;
            return db;
        }

    }
}
