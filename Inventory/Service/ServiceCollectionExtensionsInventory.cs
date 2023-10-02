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
            services.AddSingleton<Pages.RangeDay.RangeDayVM>();

            services.AddSingleton<Pages.RangeDay.PopupSelectRangeDate.PopupSelectRangeDateV>();

            services.AddSingleton<Pages.Options.EditDriver.EditDriverVM>();
            services.AddSingleton<Pages.Options.EditDriver.EditDriverV>();


            services.AddSingleton<ISaveDayService, SaveDayService>();
            services.AddSingleton<ISelectDayService, SelectDayService>();

#if WINDOWS
            services.AddSingleton<Pages.SingleDay.SingleDayVWindows>();
            services.AddSingleton<Pages.Products.ListProduct.ListProductVWindows>();
            services.AddSingleton<Pages.RangeDay.RangeDayVWindows>();

#else
            services.AddSingleton<Pages.RangeDay.ExistingFiles.ExistingFilesV>();
            services.AddSingleton<Pages.RangeDay.ExistingFiles.ExistingFilesVM>();

            services.AddSingleton<Pages.RangeDay.RangeDayV>();
            services.AddSingleton<Pages.SingleDay.SingleDayV>();
            services.AddSingleton<Pages.Products.ListProduct.ListProductV>();
#endif


            return services;
        }
    }
}
