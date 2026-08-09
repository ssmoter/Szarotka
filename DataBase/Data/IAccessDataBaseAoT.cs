

using DataBase.Data.MySqliteConnection;
using DataBase.Service;

using SQLite;


namespace DataBase.Data
{
    public interface IAccessDataBaseAoT : IAccessDataBase
    {
        new ISQLiteConnection DataBase { get; }
        new ISQLiteAsyncConnection DataBaseAsync { get; }
        IMyDbAsyncConnection DbAsyncAoT { get; }
        IMyDbConnection DbSyncAoT { get; }
        ITimeService TimeService { get; }

        new void Dispose();
        new void SaveLog(Exception ex);
        new Task SaveLogAsync(Exception ex);
    }

    public class AccessDataBaseAoT : IAccessDataBaseAoT
    {
        public ISQLiteConnection DataBase => db.DataBase;

        public ISQLiteAsyncConnection DataBaseAsync => db.DataBaseAsync;

        private readonly IAccessDataBase db;

        public IMyDbConnection DbSyncAoT { get; private set; }
        public IMyDbAsyncConnection DbAsyncAoT { get; private set; }
        public ITimeService TimeService { get; private set; }

        

        public AccessDataBaseAoT(IAccessDataBase db,
                                 IMyDbConnection dbSyncAoT,
                                 IMyDbAsyncConnection dbAsyncAoT,
                                 ITimeService timeService)
        {
            this.db = db;
            DbSyncAoT = dbSyncAoT;
            DbAsyncAoT = dbAsyncAoT;
            TimeService = timeService;
        }

        public void Dispose()
        {
            db.Dispose();
        }


        public void SaveLog(Exception ex)
        {
            Model.LogsModel log = new()
            {
                CreatedDateTime = TimeService.UtcNow(),
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };
            _ = DbSyncAoT.Execute(SetSqlError, new { log.StackTrace, log.Message, log.Created });

            Console.WriteLine($@"
            Error {log.CreatedDateTime}{Environment.NewLine}
            {ex.Message}{Environment.NewLine}
            {ex.StackTrace}");
        }

        public async Task SaveLogAsync(Exception ex)
        {
            Model.LogsModel log = new()
            {
                CreatedDateTime = TimeService.UtcNow(),
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };
            _ = await DbAsyncAoT.ExecuteAsync(SetSqlError, new { log.StackTrace, log.Message, log.Created });

            Console.WriteLine($@"
            Error {log.CreatedDateTime}{Environment.NewLine}
            {ex.Message}{Environment.NewLine}
            {ex.StackTrace}");
        }
        private const string SetSqlError =
                 $@"
                INSERT INTO {nameof(Model.LogsModel)}(
                {nameof(Model.LogsModel.StackTrace)},
                {nameof(Model.LogsModel.Message)},
                {nameof(Model.LogsModel.Created)})
                VALUES(
                @{nameof(Model.LogsModel.StackTrace)},
                @{nameof(Model.LogsModel.Message)},
                @{nameof(Model.LogsModel.Created)}
                )";
    }








}
