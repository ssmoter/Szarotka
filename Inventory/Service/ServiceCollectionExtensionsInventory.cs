using Inventory.Data;
using Inventory.Pages.Options.CreateTable;

namespace Inventory.Service
{
    public static class ServiceCollectionExtensionsInventory
    {
        public static IServiceCollection AddMyServiceInventory(this IServiceCollection services)
        {

            services.AddScoped<CreateTableVM>();
            services.AddScoped<CreateTableV>();
            services.AddSingleton<Pages.Main.MainVM>();
            services.AddSingleton<Pages.Main.MainV>();

            services.AddSingleton<Pages.SingleDay.SingleDayVM>();

            services.AddScoped<Pages.Products.ListProduct.ListProductVM>();
            services.AddTransient<Pages.Products.ListProduct.AddEdit.AddEditProductVM>();
            services.AddTransient<Pages.Products.ListProduct.AddEdit.AddEditProductV>();
            services.AddScoped<Pages.RangeDay.RangeDayVM>();

            services.AddTransient<Pages.RangeDay.PopupSelectRangeDate.PopupSelectRangeDateV>();

            services.AddScoped<Pages.RangeDay.Graph.GraphV>();
            //services.AddScoped<Pages.RangeDay.Graph.GraphVM>();

            services.AddSingleton<Pages.SingleDay.SingleDayV>();

            services.AddScoped<Pages.Products.ListProduct.ListProductV>();
            services.AddScoped<Pages.RangeDay.RangeDayV>();
            services.AddScoped<Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageVM>();
            services.AddScoped<Pages.SingleDayPreview.SingleDayPreviewPage.SingleDayPreviewPageV>();
            services.AddScoped<Data.InventoryApi.IGetProductHttp, Data.InventoryApi.GetProductHttp>();
            services.AddScoped<Data.InventoryApi.ISendProductHttp, Data.InventoryApi.SendProductHttp>();
            services.AddScoped<Data.InventoryApi.ISendDayHttp, Data.InventoryApi.SendDayHttp>();
            services.AddScoped<Data.InventoryApi.IGetDayHttp, Data.InventoryApi.GetDayHttp>();

            return services;
        }
    }
}
