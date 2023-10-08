using DataBase.Helper;

using SQLite;

namespace DataBase.Data
{
    public class AccessDataBase : IDisposable
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

                if (!System.IO.Directory.Exists(Constants.GetPathFolder))
                {
                    System.IO.Directory.CreateDirectory(Constants.GetPathFolder);
                }

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

            Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "Ok");

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
            await Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "Ok");
        }

        public void Dispose()
        {
            DataBase.Dispose();
            DataBaseAsync.CloseAsync();
            GC.SuppressFinalize(this);
        }
    }
}
