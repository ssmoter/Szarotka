using DataBase.Helper;

using SQLite;

using System.Diagnostics;

namespace DataBase.Data
{
    public class AccessDataBase : IDisposable
    {
        public SQLiteAsyncConnection DataBaseAsync { get; private set; }
        public SQLiteConnection DataBase { get; private set; }

        public AccessDataBase()
        {
            DataBaseAsync ??= new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            DataBase ??= new SQLiteConnection(Constants.DatabasePath, Constants.Flags);
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
#if DEBUG
            Debug.WriteLine($"Error{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
#endif
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
