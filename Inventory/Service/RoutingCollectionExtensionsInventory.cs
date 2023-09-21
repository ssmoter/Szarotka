namespace Inventory.Service
{
    public static class RoutingCollectionExtensionsInventory
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(Inventory.Pages.Main.MainV), typeof(Inventory.Pages.Main.MainV));
            Routing.RegisterRoute(nameof(Inventory.Pages.Products.ListProduct.AddEdit.AddEditProductV), typeof(Inventory.Pages.Products.ListProduct.AddEdit.AddEditProductV));
            Routing.RegisterRoute(nameof(Inventory.Pages.RangeDay.RangeDayV), typeof(Inventory.Pages.RangeDay.RangeDayV));

#if WINDOWS
            Routing.RegisterRoute(nameof(Inventory.Pages.SingleDay.SingleDayV), typeof(Inventory.Pages.SingleDay.SingleDayVWindows));
            Routing.RegisterRoute(nameof(Inventory.Pages.Products.ListProduct.ListProductV), typeof(Inventory.Pages.Products.ListProduct.ListProductVWindows));

#else
            Routing.RegisterRoute(nameof(Inventory.Pages.SingleDay.SingleDayV), typeof(Inventory.Pages.SingleDay.SingleDayV));
            Routing.RegisterRoute(nameof(Inventory.Pages.Products.ListProduct.ListProductV), typeof(Inventory.Pages.Products.ListProduct.ListProductV));
#endif


        }
    }
}
