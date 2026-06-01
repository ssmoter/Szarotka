namespace Inventory.Service
{
    public static class RoutingCollectionExtensionsInventory
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(Inventory.Pages.Main.MainV), typeof(Inventory.Pages.Main.MainV));
            Routing.RegisterRoute(nameof(Inventory.Pages.Products.ListProduct.AddEdit.AddEditProductV), typeof(Inventory.Pages.Products.ListProduct.AddEdit.AddEditProductV));
            Routing.RegisterRoute(nameof(Inventory.Pages.RangeDay.Graph.GraphV), typeof(Inventory.Pages.RangeDay.Graph.GraphV));
            Routing.RegisterRoute(nameof(Inventory.Pages.SingleDay.SingleDayV), typeof(Inventory.Pages.SingleDay.SingleDayV));

            Routing.RegisterRoute(nameof(Inventory.Pages.Products.ListProduct.ListProductV), typeof(Inventory.Pages.Products.ListProduct.ListProductV));

            Routing.RegisterRoute(nameof(Inventory.Pages.RangeDay.RangeDayV), typeof(Inventory.Pages.RangeDay.RangeDayV));
            Routing.RegisterRoute(nameof(Inventory.Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageV), typeof(Inventory.Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageV));


        }
    }
}
