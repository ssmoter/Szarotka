using Inventory.Pages.Options.CreateTable;

namespace Inventory.Service
{
    public static class ServiceCollectionExtensionsInventory
    {
        public static IServiceCollection AddMyServiceInventory(this IServiceCollection services)
        {

            services.AddSingleton<CreateTableVM>();
            services.AddSingleton<CreateTableV>();
            services.AddSingleton<Pages.Main.MainVM>();
            services.AddSingleton<Pages.Main.MainV>();
            services.AddSingleton<Pages.SingleDay.SingleDayVM>();
            services.AddSingleton<Pages.Products.ListProduct.ListProductVM>();
            services.AddSingleton<Pages.Products.ListProduct.AddEdit.AddEditProductVM>();
            services.AddSingleton<Pages.Products.ListProduct.AddEdit.AddEditProductV>();
            services.AddSingleton<Pages.RangeDay.RangeDayV>();
            services.AddSingleton<Pages.RangeDay.RangeDayVM>();


#if WINDOWS
            services.AddSingleton<Pages.SingleDay.SingleDayVWindows>();
            services.AddSingleton<Pages.Products.ListProduct.ListProductVWindows>();

#else

            services.AddSingleton<Pages.SingleDay.SingleDayV>();
            services.AddSingleton<Pages.Products.ListProduct.ListProductV>();
#endif


            return services;
        }
    }
}
