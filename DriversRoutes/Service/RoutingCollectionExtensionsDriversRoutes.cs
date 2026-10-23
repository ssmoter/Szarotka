namespace DriversRoutes.Service
{
    public static class RoutingCollectionExtensionsDriversRoutes
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(Pages.Maps.MapAndPoints.MapsV), typeof(Pages.Maps.MapAndPoints.MapsV));
            Routing.RegisterRoute(nameof(Pages.Main.MainVDriversRoutesV), typeof(Pages.Main.MainVDriversRoutesV));
            Routing.RegisterRoute(nameof(Pages.ListOfPoints.ListOfPointsV), typeof(Pages.ListOfPoints.ListOfPointsV));
            Routing.RegisterRoute(nameof(Pages.Customer.AddCustomer.AddCustomerV), typeof(Pages.Customer.AddCustomer.AddCustomerV));
            Routing.RegisterRoute(nameof(Pages.Customer.DisplayCustomer.DisplayCustomerV), typeof(Pages.Customer.DisplayCustomer.DisplayCustomerV));

        }
    }
}
