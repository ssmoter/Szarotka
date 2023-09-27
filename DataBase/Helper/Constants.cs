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
#else
                return "DataBaseSzarotkaSQLite.db3";
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
#elif ANDROID
                var doc = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
                return Path.Combine(doc.Path);
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
#elif ANDROID
                var doc = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDocuments);
                return Path.Combine(doc.Path,DatabaseName);
#else
return "";
#endif

            }
        }

        public static CultureInfo CultureInfo = new CultureInfo("pl");

    }
}
