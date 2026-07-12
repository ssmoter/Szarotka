using DataBase.Helper;
using DataBase.Service;

using SQLite;

namespace DataBase.Data
{
    public interface IAccessDataBase
    {
        ISQLiteConnection DataBase { get; }
        ISQLiteAsyncConnection DataBaseAsync { get; }

        void Dispose();
        void SaveLog(Exception ex);
        Task SaveLogAsync(Exception ex);
    }

    public class AccessDataBase : IDisposable, IAccessDataBase
    {
        public ISQLiteAsyncConnection DataBaseAsync { get; private set; }
        public ISQLiteConnection DataBase { get; private set; }
        private readonly ITimeService _timeService;

        public AccessDataBase()
        {
            CreatedFolderPath();

            var path = Constants.DatabasePath;
            DataBaseAsync ??= new SQLiteAsyncConnection(path, Constants.Flags);
            DataBase ??= new SQLiteConnection(path, Constants.Flags);
            _timeService = new CurrentUtc();
            ConfigureDatabase();

        }
        public AccessDataBase(ITimeService time)
        {
            CreatedFolderPath();

            var path = Constants.DatabasePath;
            DataBaseAsync ??= new SQLiteAsyncConnection(path, Constants.Flags);
            DataBase ??= new SQLiteConnection(path, Constants.Flags);
            _timeService = time;
            ConfigureDatabase();

        }
        /// <summary>
        /// Tylko dla testów i serwera        
        /// </summary>
        /// <param name="path"></param>
        public AccessDataBase(string path)
        {

            DataBaseAsync ??= new SQLiteAsyncConnection(path, Constants.Flags);
            DataBase ??= new SQLiteConnection(path, Constants.Flags);
            _timeService = new CurrentUtc();
            ConfigureDatabase();

        }
        /// <summary>
        /// Tylko dla testów i serwera        
        /// </summary>
        /// <param name="path"></param>
        public AccessDataBase(string path, ITimeService time)
        {

            DataBaseAsync ??= new SQLiteAsyncConnection(path, Constants.Flags);
            DataBase ??= new SQLiteConnection(path, Constants.Flags);
            _timeService = time;
            ConfigureDatabase();

        }
        /// <summary>
        /// Tylko dla testów        
        /// </summary>
        /// <param name="path"></param>
        public AccessDataBase(string path, ITimeService time, SQLiteOpenFlags Flags)
        {
            DataBaseAsync ??= new SQLiteAsyncConnection(path, Flags);
            DataBase ??= new SQLiteConnection(path, Flags);
            _timeService = time;
            ConfigureDatabase();
        }
        public void SaveLog(Exception ex)
        {
            Model.LogsModel log = new()
            {
                CreatedDateTime = _timeService.UtcNow(),
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };

            _ = DataBase.Execute(SetSqlError(log), log.StackTrace, log.Message, log.Created);
            //DataBase.Insert(log);

            Console.WriteLine($@"
            Error {log.CreatedDateTime}{Environment.NewLine}
            {ex.Message}{Environment.NewLine}
            {ex.StackTrace}");
        }

        public async Task SaveLogAsync(Exception ex)
        {
            Model.LogsModel log = new()
            {
                CreatedDateTime = _timeService.UtcNow(),
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };
            _ = await DataBaseAsync.ExecuteAsync(SetSqlError(log), log.StackTrace, log.Message, log.Created);

            //            await DataBaseAsync.InsertAsync(log);

            Console.WriteLine($@"
            Error {log.CreatedDateTime}{Environment.NewLine}
            {ex.Message}{Environment.NewLine}
            {ex.StackTrace}");
        }

        private static string SetSqlError(Model.LogsModel log)
        {
            return $@"
                INSERT INTO {nameof(Model.LogsModel)}(
                {nameof(Model.LogsModel.StackTrace)},
                {nameof(Model.LogsModel.Message)},
                {nameof(Model.LogsModel.Created)})
                VALUES(
                @{nameof(log.StackTrace)},
                @{nameof(log.Message)},
                @{nameof(log.Created)}
                )";
        }

        private static void CreatedFolderPath()
        {
            if (!Directory.Exists(Constants.GetPathFolder))
            {
                Directory.CreateDirectory(Constants.GetPathFolder);
            }
        }

        private void ConfigureDatabase()
        {
            // Włączenie trybu WAL - klucz do współbieżności czytelnicy/pisarze
            DataBase.ExecuteScalar<string>("PRAGMA journal_mode=WAL;");

            // Ustawienie synchronizacji na NORMAL (błyskawiczny zapis w trybie WAL)
            DataBase.ExecuteScalar<string>("PRAGMA synchronous=NORMAL;");

            // Busy Timeout na 5000 ms - kolejkuje zapisy 5 użytkowników, zapobiega blokadom
            DataBase.ExecuteScalar<string>("PRAGMA busy_timeout=5000;");
        }

        public void Dispose()
        {
            DataBase.Dispose();
            DataBaseAsync.CloseAsync();
            GC.SuppressFinalize(this);
        }
    }
}
