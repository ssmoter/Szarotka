using DataBase.Data;
using DataBase.Helper;

namespace Shared.Data
{
    public static class DataBaseExtensionsMethod
    {

        public static void SaveLogExtension(this IAccessDataBase db, Exception ex)
        {
            db.SaveLog(ex);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "Ok");
            });
        }
        public static async Task SaveLogAsyncExtension(this IAccessDataBase db, Exception ex)
        {
            await db.SaveLogAsync(ex);
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.CurrentPage.DisplayAlertAsync("Error", ex.Message, "Ok");
            });
        }

    }
}

