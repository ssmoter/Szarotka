using Shared.Helper;

namespace Inventory.Helper
{
    public static class SelectedDriver
    {
        public static string Id => UserAfterLogin.User.Id.ToString();
        public static string Name => UserAfterLogin.User.Name;
        public static string Description => UserAfterLogin.User.Description;
    }
}
