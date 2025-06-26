using Shared.Pages.ConfirmEmail;
using Shared.Pages.ExistingFiles;
using Shared.Pages.Log;
using Shared.Pages.Log.LogData;
using Shared.Pages.LogIn;
using Shared.Pages.LogIn.ForgetPassword;
using Shared.Pages.Register;
using Shared.Pages.UpdateDataBase;
using Shared.Pages.UpdateDifference;
using Shared.Pages.UserDisplay;
using Shared.Pages.UserDisplay.UserEdit;

namespace Shared.Service
{
    public static class RoutingCollectionExtensionsShared
    {
        public static void AddRoutings()
        {
            Routing.RegisterRoute(nameof(LogDataV), typeof(LogDataV));
#if WINDOWS
            Routing.RegisterRoute(nameof(LogV), typeof(LogVWindows));
#else
            Routing.RegisterRoute(nameof(LogV), typeof(LogV));
#endif
            Routing.RegisterRoute(nameof(ExistingFilesV), typeof(ExistingFilesV));
            Routing.RegisterRoute(nameof(UpdateDataBaseV), typeof(UpdateDataBaseV));
            Routing.RegisterRoute(nameof(RegisterV), typeof(RegisterV));
            Routing.RegisterRoute(nameof(ConfirmEmailV), typeof(ConfirmEmailV));
            Routing.RegisterRoute(nameof(LogInV), typeof(LogInV));
            Routing.RegisterRoute(nameof(UserDisplayV), typeof(UserDisplayV));
            Routing.RegisterRoute(nameof(UserEditV), typeof(UserEditV));
            Routing.RegisterRoute(nameof(ForgetPasswordV), typeof(ForgetPasswordV));
            Routing.RegisterRoute(nameof(UpdateDifferenceV), typeof(UpdateDifferenceV));


        }
    }
}
