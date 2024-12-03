namespace SzarotkaBlazor.Service
{
    public static class RoutingCollectionExtensions
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(SzarotkaBlazor.Pages.Options.Main.MainOptionsV), typeof(SzarotkaBlazor.Pages.Options.Main.MainOptionsV));

            Inventory.Service.RoutingCollectionExtensionsInventory.AddRoutings();
            Shared.Service.RoutingCollectionExtensionsShared.AddRoutings();
            DriversRoutes.Service.RoutingCollectionExtensionsDriversRoutes.AddRoutings();
        }
    }
}
