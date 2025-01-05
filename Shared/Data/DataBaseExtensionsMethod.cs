using DataBase.Data;

namespace Shared.Data
{
    public static class DataBaseExtensionsMethod
    {

        public static void SaveLogExtension(this AccessDataBase db, Exception ex)
        {
            db.SaveLog(ex);
            Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "Ok");
        }
        public static async Task SaveLogAsyncExtension(this AccessDataBase db, Exception ex)
        {
            await db.SaveLogAsync(ex);
            await Shell.Current.CurrentPage.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
