namespace DataBase.Service
{
    public static class RoutingCollectionExtensionsDataBase
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(Pages.Log.LogData.LogDataV), typeof(Pages.Log.LogData.LogDataV));
#if WINDOWS
            Routing.RegisterRoute(nameof(Pages.Log.LogV), typeof(Pages.Log.LogVWindows));
#else
            Routing.RegisterRoute(nameof(Pages.Log.LogV), typeof(Pages.Log.LogV));
#endif
            Routing.RegisterRoute(nameof(Pages.ExistingFiles.ExistingFilesV), typeof(Pages.ExistingFiles.ExistingFilesV));
            Routing.RegisterRoute(nameof(Pages.UpdateDataBase.UpdateDataBaseV), typeof(Pages.UpdateDataBase.UpdateDataBaseV));

        }
    }
}
