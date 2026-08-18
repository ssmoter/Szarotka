using CommunityToolkit.Maui.Alerts;

using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Shared.Data;
using Shared.Model;

namespace Shared.Helper
{
    public static partial class UserAfterLogin
    {
        public static DateTime Expires { get; private set; } = new();
        public static bool IsLogin { get; private set; } = false;
        public static User User { get; private set; } = new();

        public static event Action<User, bool> OnLogin;

        private readonly static IAccessDataBaseAoT _db;

        static UserAfterLogin()
        {
            var db = Shared.Service.AppServiceProvider.GetService<IAccessDataBaseAoT>();
            if (db is not null)
            {
                _db = db;
            }
            // OnLogin?.Invoke(new(), true);
        }

        public static void SetLoginUser(User token)
        {
            SetLoginUser(token.AccessToken);
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

                var helperTable = new HelperTable(nameof(UserAfterLogin.User.AccessToken), token)
                {
                    UserCreatedId = user.Id,
                    UserUpdatedId = user.Id,
                };
                helperTable.Set(_db);
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException)
            {
                RemoveLoginUser();
            }
            catch (ArgumentException ex)
            {
                Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
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
            var helperTable = new HelperTable(nameof(UserAfterLogin.User.AccessToken), "");
            helperTable.Set(_db);
            OnLogin?.Invoke(User, IsLogin);
        }



        public static void SetAuthorization(this HttpClient client)
        {
            var token = User.AccessToken;
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
