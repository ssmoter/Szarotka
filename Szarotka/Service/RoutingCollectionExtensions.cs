namespace Szarotka.Service
{
    public static class RoutingCollectionExtensions
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(Szarotka.Pages.Options.Main.MainOptionsV), typeof(Szarotka.Pages.Options.Main.MainOptionsV));

            Inventory.Service.RoutingCollectionExtensionsInventory.AddRoutings();
            DataBase.Service.RoutingCollectionExtensionsDataBase.AddRoutings();
        }
    }
}
