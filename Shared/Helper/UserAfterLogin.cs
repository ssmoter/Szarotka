using CommunityToolkit.Maui.Alerts;

using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Shared.Data;
using Shared.Model;

namespace Shared.Helper
{
    public partial class UserAfterLogin
    {
        public static DateTime Expires { get; private set; } = new();
        public static bool IsLogin { get; private set; } = false;
        public static User User { get; private set; } = new();

        public static event Action<User, bool> OnLogin;

        private readonly static IAccessDataBase _db;

        static UserAfterLogin()
        {
            _db = Shared.Service.AppServiceProvider.Current.GetRequiredService<IAccessDataBase>();
            // OnLogin?.Invoke(new(), true);
        }

        public static void SetLoginUser(User token)
        {
            SetLoginUser(token.Token);
        }
        public static void SetLoginUser(string token)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(token);
                var (user, expires) = DataBase.Helper.ReadToken.GetUserFromToken(token);

                User = user;
                Expires = expires;

                IsLogin = true;
                OnLogin?.Invoke(User, IsLogin);

                var helperTable = new HelperTable(nameof(UserAfterLogin.User.Token), token);
                helperTable.Set(_db);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException)
            {
                RemoveLoginUser();
            }
            catch (ArgumentException)
            {

            }
            catch (Exception ex)
            {
                Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                _db.SaveLogExtension(ex);
            }
        }

        public static void RemoveLoginUser()
        {
            Expires = new();
            IsLogin = false;
            User = new();
            var helperTable = new HelperTable(nameof(UserAfterLogin.User.Token), "");
            helperTable.Set(_db);
            OnLogin?.Invoke(User, IsLogin);
        }
    }
}
