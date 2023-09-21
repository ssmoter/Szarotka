using DataBase.Helper;

using SQLite;

namespace DataBase.Data
{
    public class AccessDataBase
    {
        public SQLiteAsyncConnection DataBaseAsync { get; private set; }
        public SQLiteConnection DataBase { get; private set; }

        public AccessDataBase()
        {

            if (DataBaseAsync is null)
            {
                DataBaseAsync = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                DataBase = new SQLiteConnection(Constants.DatabasePath, Constants.Flags);
            }
            Task.Run(async () =>
            {


#if WINDOWS
                if (!System.IO.Directory.Exists(Constants.GetPathFolder))
                {
                    System.IO.Directory.CreateDirectory(Constants.GetPathFolder);                    
                }
#else

#endif
                var tableInfo = await DataBaseAsync.GetTableInfoAsync(nameof(Model.LogsModel));
                bool exist = tableInfo.Count > 0;
                if (!exist)
                {
                    await DataBaseAsync.CreateTableAsync<Model.LogsModel>();
                }
            });
        }

        public void SaveLog(Exception ex)
        {
            var log = new Model.LogsModel()
            {
                CreatedDateTime = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };
            DataBase.Insert(log);
        }

        public async Task SaveLogAsync(Exception ex)
        {
            var log = new Model.LogsModel()
            {
                CreatedDateTime = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
            };
            await DataBaseAsync.InsertAsync(log);
        }

    }
}
