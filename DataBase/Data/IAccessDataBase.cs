using DataBase.Helper;
using DataBase.Service;

using SQLite;

#if DEBUG
#endif
namespace DataBase.Data
{
    public interface IAccessDataBase
    {
        ISQLiteConnection DataBase { get; }
        ISQLiteAsyncConnection DataBaseAsync { get; }

        void Dispose();
        string GetServerUrl();
        Task<string> GetServerUrlAsync();
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
            var path = Constants.DatabasePath;
            DataBaseAsync ??= new SQLiteAsyncConnection(path, Constants.Flags);
            DataBase ??= new SQLiteConnection(path, Constants.Flags);
            _timeService = new CurrentUtc();
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
        }
        public AccessDataBase(string path, ITimeService time)
        {
            DataBaseAsync ??= new SQLiteAsyncConnection(path, Constants.Flags);
            DataBase ??= new SQLiteConnection(path, Constants.Flags);
            _timeService = time;
        }

        public void SaveLog(Exception ex)
        {
            var log = new Model.LogsModel()
            {
                CreatedDateTime = _timeService.UtcNow(),
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };

            DataBase.Insert(log);

            Console.WriteLine($@"
Error {log.CreatedDateTime}{Environment.NewLine}
{ex.Message}{Environment.NewLine}
{ex.StackTrace}");
        }

        public async Task SaveLogAsync(Exception ex)
        {
            var log = new Model.LogsModel()
            {
                CreatedDateTime = _timeService.UtcNow(),
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };
            await DataBaseAsync.InsertAsync(log);

            Console.WriteLine($@"
Error {log.CreatedDateTime}{Environment.NewLine}
{ex.Message}{Environment.NewLine}
{ex.StackTrace}");
        }


        public string GetServerUrl()
        {
            return Constants.ServerUrl;
        }
        public async Task<string> GetServerUrlAsync()
        {
            return await Task.FromResult(Constants.ServerUrl);
        }

        public void Dispose()
        {
            DataBase.Dispose();
            DataBaseAsync.CloseAsync();
            GC.SuppressFinalize(this);
        }
    }
}
