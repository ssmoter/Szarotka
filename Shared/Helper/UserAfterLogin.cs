using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Shared.Data;
using Shared.Model;

using System.IdentityModel.Tokens.Jwt;

namespace Shared.Helper
{
    public partial class UserAfterLogin
    {
        public static DateTime Expires { get; private set; } = new();
        public static bool IsLogin { get; private set; } = true;
        public static User User { get; private set; } = new();
        public static event Action<User, bool> OnLogin;

        private static readonly JwtSecurityTokenHandler _handler = new();
        private readonly static IAccessDataBase _db;

        static UserAfterLogin()
        {
            _db = Shared.Service.AppServiceProvider.Current.GetRequiredService<IAccessDataBase>();
            OnLogin?.Invoke(new(), true);
        }

        public static void SetLoginUser(User token)
        {
            SetLoginUser(token.Token);
        }
        public static void SetLoginUser(string token)
        {
            try
            {
                var result = DataBase.Helper.ReadToken.GetUserFromToken(token);

                User = result.user;
                Expires = result.expires;

                IsLogin = true;
                OnLogin?.Invoke(User, IsLogin);

                var helperTable = new HelperTable(nameof(UserAfterLogin.User.Token), token);
                helperTable.Set(_db);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException)
            {
                RemoveLoginUser();
            }
            catch (Exception ex)
            {
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
