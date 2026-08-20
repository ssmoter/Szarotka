

using DataBase.Data.MySqliteConnection;
using DataBase.Service;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
        private readonly ILogger<AccessDataBaseAoT> _logger;



        public AccessDataBaseAoT(IAccessDataBase db,
                                 IMyDbConnection dbSyncAoT,
                                 IMyDbAsyncConnection dbAsyncAoT,
                                 ITimeService timeService,
                                 ILogger<AccessDataBaseAoT>? logger)
        {
            this.db = db;
            DbSyncAoT = dbSyncAoT;
            DbAsyncAoT = dbAsyncAoT;
            TimeService = timeService;
            _logger = logger ?? NullLogger<AccessDataBaseAoT>.Instance;
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
            _ = DbSyncAoT.Execute(SetSqlError, new()
            {
                [nameof(Model.LogsModel.StackTrace)] = log.StackTrace,
                [nameof(Model.LogsModel.Message)] = log.Message,
                [nameof(Model.LogsModel.Created)] = log.Created
            });

            _logger.LogError(ex, "Error occurred at {CreatedDateTime}: {Message}", log.CreatedDateTime, ex.Message);
        }

        public async Task SaveLogAsync(Exception ex)
        {
            Model.LogsModel log = new()
            {
                CreatedDateTime = TimeService.UtcNow(),
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };
            _ = await DbAsyncAoT.ExecuteAsync(SetSqlError, new()
            {
                [nameof(Model.LogsModel.StackTrace)] = log.StackTrace,
                [nameof(Model.LogsModel.Message)] = log.Message,
                [nameof(Model.LogsModel.Created)] = log.Created
            });
            _logger.LogError(ex, "Error occurred at {CreatedDateTime}: {Message}", log.CreatedDateTime, ex.Message);
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
