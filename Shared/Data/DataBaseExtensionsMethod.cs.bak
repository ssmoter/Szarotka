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
                Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "Ok");
            });
        }
        public static async Task SaveLogAsyncExtension(this IAccessDataBase db, Exception ex)
        {
            await db.SaveLogAsync(ex);
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "Ok");
            });
        }

        public static string GetServerUrl(this IAccessDataBase db)
        {
#if DEBUG
            return Constants.ServerUrl;
#else
            var result = Shared.Model.HelperTableExtension.GetHelperTable(nameof(Constants.ServerUrl), db);
            if (result is null)
            {
                return "";
            }
            return result.Value;
#endif

        }
        public static async Task<string> GetServerUrlAsync(this IAccessDataBase db)
        {

#if DEBUG
            return await Task.FromResult(Constants.ServerUrl);
#else
            var result = await Shared.Model.HelperTableExtension.GetHelperTableAsync(nameof(Constants.ServerUrl), db);
            if (result is null)
            {
                return "";
            }
            return result.Value;
#endif
        }

    }
}
