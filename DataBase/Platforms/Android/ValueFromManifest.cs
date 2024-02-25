using Android.Content.PM;

namespace DataBase.Platforms.Android
{
    public class ValueFromManifest
    {
        public static string GetValue(string packageName, string key)
        {
            try
            {
                return "AIzaSyDMfTC47bnsNBAK8S4xKk7Mhb_aiSqnCYU";
                //var appInfo = PackageManager.GetApplicationInfo(packageName, PackageInfoFlags.MetaData);
                //var metaData = appInfo.MetaData;
                //var myKey = metaData.GetString(key);

                //return myKey;
            }
            catch (PackageManager.NameNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
