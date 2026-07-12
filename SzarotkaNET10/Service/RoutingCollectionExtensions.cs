using SzarotkaNET10.Pages.Options.Main;

namespace SzarotkaNET10.Service
{
    public static class RoutingCollectionExtensions
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(MainOptionsV), typeof(MainOptionsV));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            Inventory.Service.RoutingCollectionExtensionsInventory.AddRoutings();
            Shared.Service.RoutingCollectionExtensionsShared.AddRoutings();
            DriversRoutes.Service.RoutingCollectionExtensionsDriversRoutes.AddRoutings();
        }
    }
}
