using DataBase.Helper;

using SQLite;

#if DEBUG
#endif
namespace DataBase.Data
{
    public class AccessDataBase : IDisposable
    {
        public SQLiteAsyncConnection DataBaseAsync { get; private set; }
        public SQLiteConnection DataBase { get; private set; }

        public AccessDataBase()
        {
            var path = Constants.DatabasePath;
            DataBaseAsync ??= new SQLiteAsyncConnection(path, Constants.Flags);
            DataBase ??= new SQLiteConnection(path, Constants.Flags);
        }
        /// <summary>
        /// Tylko dla testów
        /// </summary>
        /// <param name="path"></param>
        public AccessDataBase(string path)
        {
            DataBaseAsync ??= new SQLiteAsyncConnection(path, Constants.Flags);
            DataBase ??= new SQLiteConnection(path, Constants.Flags);
        }

        public void SaveLog(Exception ex)
        {
            var log = new Model.LogsModel()
            {
                CreatedDateTime = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };

            DataBase.Insert(log);

            Console.WriteLine($"Error{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
        }

        public async Task SaveLogAsync(Exception ex)
        {
            var log = new Model.LogsModel()
            {
                CreatedDateTime = DateTime.Now,
                Message = ex.Message,
                StackTrace = ex.StackTrace is not null ? ex.StackTrace : ""
            };
            await DataBaseAsync.InsertAsync(log);

            Console.WriteLine($"Error{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
        }

        public void Dispose()
        {
            DataBase.Dispose();
            DataBaseAsync.CloseAsync();
            GC.SuppressFinalize(this);
        }
    }
}
