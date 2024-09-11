namespace Inventory.Service
{
    public static class RoutingCollectionExtensionsInventory
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(Inventory.Pages.Main.MainV), typeof(Inventory.Pages.Main.MainV));
            Routing.RegisterRoute(nameof(Inventory.Pages.Products.ListProduct.AddEdit.AddEditProductV), typeof(Inventory.Pages.Products.ListProduct.AddEdit.AddEditProductV));
            Routing.RegisterRoute(nameof(Inventory.Pages.Options.EditDriver.EditDriverV), typeof(Inventory.Pages.Options.EditDriver.EditDriverV));
            Routing.RegisterRoute(nameof(Inventory.Pages.RangeDay.Graph.GraphV), typeof(Inventory.Pages.RangeDay.Graph.GraphV));
            Routing.RegisterRoute(nameof(Inventory.Pages.SingleDay.SingleDayV), typeof(Inventory.Pages.SingleDay.SingleDayV));

#if WINDOWS
            Routing.RegisterRoute(nameof(Inventory.Pages.Products.ListProduct.ListProductV), typeof(Inventory.Pages.Products.ListProduct.ListProductVWindows));
            Routing.RegisterRoute(nameof(Inventory.Pages.RangeDay.RangeDayV), typeof(Inventory.Pages.RangeDay.RangeDayVWindows));

#else
            Routing.RegisterRoute(nameof(Inventory.Pages.RangeDay.RangeDayV), typeof(Inventory.Pages.RangeDay.RangeDayV));
            Routing.RegisterRoute(nameof(Inventory.Pages.Products.ListProduct.ListProductV), typeof(Inventory.Pages.Products.ListProduct.ListProductV));
#endif


        }
    }
}
