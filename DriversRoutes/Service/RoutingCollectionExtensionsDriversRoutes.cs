namespace DriversRoutes.Service
{
    public static class RoutingCollectionExtensionsDriversRoutes
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(Pages.Maps.MapsV), typeof(Pages.Maps.MapsV));
            Routing.RegisterRoute(nameof(Pages.Main.MainVDriversRoutesV), typeof(Pages.Main.MainVDriversRoutesV));

        }
    }
}
