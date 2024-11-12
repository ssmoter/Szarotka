
namespace DataBase.Helper
{
    public class Manifest
    {

        public static string GetManifestValue(string key)
        {
#if ANDROID
            Android.Content.Context context = Android.App.Application.Context;
            var applicationInfo = context.PackageManager.GetApplicationInfo(context.PackageName, Android.Content.PM.PackageInfoFlags.MetaData);
            var bundle = applicationInfo.MetaData;
            var value = bundle.GetString(key);
            return value;
#else
            return "AIzaSyDMfTC47bnsNBAK8S4xKk7Mhb_aiSqnCYU";
#endif
        }

    }
}
