namespace DataBase.Helper
{
    public class Constants
    {
        public static string DatabaseName
        {
            get
            {
#if WINDOWS
                return @"Szarotka\InventorySQLite.db3";
#else
                return "InventorySQLite.db3";
#endif
            }
        }

        public const SQLite.SQLiteOpenFlags Flags =
        // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
        // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string GetPathFolder
        {
            get
            {
#if WINDOWS
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Szarotka");
#else
                return "";
#endif

            }
        }
        public static string DatabasePath
        {
            get
            {
#if WINDOWS
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DatabaseName);
#else
                return Path.Combine(FileSystem.Current.AppDataDirectory, DatabaseName);
#endif

            }
        }
    }
}
