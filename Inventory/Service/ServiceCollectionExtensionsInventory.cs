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

            services.AddTransient<Pages.Options.EditDriver.EditDriverVM>();
            services.AddTransient<Pages.Options.EditDriver.EditDriverV>();

            services.AddScoped<Pages.RangeDay.Graph.GraphV>();
            services.AddScoped<Pages.RangeDay.Graph.GraphVM>();

            services.AddScoped<ISaveDayService, SaveDayService>();
            services.AddScoped<ISelectDayService, SelectDayService>();

            services.AddSingleton<Pages.SingleDay.SingleDayV>();
#if WINDOWS
            services.AddScoped<Pages.Products.ListProduct.ListProductVWindows>();
            services.AddScoped<Pages.RangeDay.RangeDayVWindows>();
#else
            services.AddScoped<Pages.RangeDay.RangeDayV>();
            services.AddScoped<Pages.Products.ListProduct.ListProductV>();
#endif

            return services;
        }
    }
}
