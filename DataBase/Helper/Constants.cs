using System.Globalization;

namespace DataBase.Helper
{
    public class Constants
    {
        public static string DatabaseName
        {
            get
            {
#if WINDOWS 
                return @"Szarotka\DataBaseSzarotkaSQLite.db3";
#elif ANDROID
                return "DataBaseSzarotkaSQLite.db3";
#else
                return @"Szarotka\DataBaseSzarotkaSQLite.db3";
#endif
            }
        }
        public static string BackupName
        {
            get
            {
#if WINDOWS
                return @"Szarotka\BackupDataBaseSzarotkaSQLite.db3";
#elif ANDROID
                return "BackupDataBaseSzarotkaSQLite.db3";
#else
                return @"Szarotka\BackupDataBaseSzarotkaSQLite.db3";

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
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Szarotka");
#elif ANDROID
                var doc = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
                return Path.Combine(doc.Path);
#else
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Szarotka");
#endif
            }
        }
        public static string DatabasePath
        {
            get
            {
#if WINDOWS
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DatabaseName);
#elif ANDROID
                var doc = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
                return Path.Combine(doc.Path, DatabaseName);
#else
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DatabaseName);
#endif

            }
        }
        public static string BackupPath
        {
            get
            {
#if WINDOWS
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), BackupName);
#elif ANDROID
                var doc = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
                return Path.Combine(doc.Path, BackupName);
#else
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), BackupName);
#endif
            }
        }

        public static CultureInfo CultureInfo => new("pl");
    }
}
