using DataBase.Service;

using MudBlazor.Services;

using Shared.Data;
using Shared.Data.ServerHttpClients;
using Shared.Pages.ConfirmEmail;
using Shared.Pages.LogIn;
using Shared.Pages.LogIn.ForgetPassword;
using Shared.Pages.UpdateDifference;
using Shared.Pages.UserDisplay;
using Shared.Pages.UserDisplay.UserEdit;

namespace Shared.Service
{
    public static class ServiceCollectionExtensionsShared
    {
        public static IServiceCollection AddMyServiceShared(this IServiceCollection services)
        {
            services.AddMudServices();
            services.AddHttpClient();

            services.AddScoped<ICreatedDataBase, CreatedDataBase>();

            services.AddScoped<Pages.Log.LogData.LogDataVM>();
            services.AddScoped<Pages.Log.LogData.LogDataV>();

            services.AddScoped<Pages.Log.LogVM>();
            services.AddScoped<Pages.ExistingFiles.ExistingFilesVM>();
            services.AddScoped<Pages.ExistingFiles.ExistingFilesV>();

            services.AddScoped<Pages.UpdateDataBase.UpdateDataBaseVM>();
            services.AddScoped<Pages.UpdateDataBase.UpdateDataBaseV>();

            services.AddScoped<Pages.Register.RegisterVM>();
            services.AddScoped<Pages.Register.RegisterV>();

#if WINDOWS
            services.AddScoped<Pages.Log.LogVWindows>();
#else
            services.AddScoped<Pages.Log.LogV>();
#endif

            services.AddScoped<IRegisterHttp, RegisterHttp>();
            services.AddScoped<ILoginHttp, LoginHttp>();
            services.AddScoped<IEditUserHttp, EditUserHttp>();

            services.AddScoped<ConfirmEmailVM>();
            services.AddScoped<ConfirmEmailV>();
            services.AddScoped<LogInVM>();
            services.AddScoped<LogInV>();

            services.AddScoped<UserDisplayVM>();
            services.AddScoped<UserDisplayV>();

            services.AddScoped<UserEditVM>();
            services.AddScoped<UserEditV>();

            services.AddScoped<ForgetPasswordV>();
            services.AddScoped<ForgetPasswordVM>();

            services.AddSingleton<UpdateDifferenceV>();
            services.AddSingleton<UpdateDifferenceVM>();

            services.AddScoped<IResetPasswordHttp, ResetPasswordHttp>();
            services.AddScoped<IUpdateLogsHttp, UpdateLogsHttp>();

            return services;
        }

    }
}
