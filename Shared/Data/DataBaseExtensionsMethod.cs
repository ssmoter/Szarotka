using DataBase.Data;

namespace Shared.Data
{
    public static class DataBaseExtensionsMethod
    {

        public static void SaveLogExtension(this IAccessDataBaseAoT db, Exception ex)
        {
            db.SaveLog(ex);
            if (IsRunningUnitTests()) return;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current?.Windows?[0]?.Page.DisplayAlertAsync("Error", ex.Message, "Ok");
            });
        }
        public static async Task SaveLogAsyncExtension(this IAccessDataBaseAoT db, Exception ex)
        {
            await db.SaveLogAsync(ex);
            if (IsRunningUnitTests()) return;
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current?.Windows?[0]?.Page.DisplayAlertAsync("Error", ex.Message, "Ok");
            });
        }



        private static bool IsRunningUnitTests()
        {
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var a in assemblies)
                {
                    var name = a.GetName().Name;
                    if (string.IsNullOrEmpty(name)) continue;
                    if (name.StartsWith("xunit", StringComparison.OrdinalIgnoreCase)
                        || name.StartsWith("nunit", StringComparison.OrdinalIgnoreCase)
                        || name.StartsWith("Microsoft.VisualStudio.TestPlatform", StringComparison.OrdinalIgnoreCase)
                        || name.StartsWith("TestHost", StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            catch
            {
                // ignore
            }
            return false;
        }

    }
}

