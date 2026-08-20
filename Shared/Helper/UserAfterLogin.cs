using CommunityToolkit.Maui.Alerts;

using DataBase.Data;
using DataBase.Helper;
using DataBase.Model.EntitiesServer;
using DataBase.Model.SourceGenerator;

using Shared.Data;
using Shared.Data.ServerHttpClients;

using System.Text.Json;

namespace Shared.Helper
{
    public interface IAuthService
    {
        Task SaveTokensAsync(string accessToken, RefreshToken refreshToken);
        Task<string> GetAccessTokenAsync();
        Task<RefreshToken> GetRefreshTokenAsync();
        void ClearTokens();
        void SaveTokens(string accessToken, RefreshToken refreshToken);
    }
    public class AuthService : IAuthService
    {
        public async Task SaveTokensAsync(string accessToken, RefreshToken refreshToken)
        {
            ClearTokens();
            var jsonRefreshToken = JsonSerializer.Serialize(refreshToken, SzarotkaJsonSerializerContext.Default.RefreshToken);
            await SecureStorage.Default.SetAsync("access_token", accessToken);
            await SecureStorage.Default.SetAsync("refresh_token", jsonRefreshToken);
        }

        public void SaveTokens(string accessToken, RefreshToken refreshToken)
        {
            SaveTokensAsync(accessToken, refreshToken).Forget();
        }

        public async Task<string> GetAccessTokenAsync()
             => await SecureStorage.Default.GetAsync("access_token");

        public async Task<RefreshToken> GetRefreshTokenAsync()
        {
            var json = await SecureStorage.Default.GetAsync("refresh_token");
            if (!string.IsNullOrEmpty(json))
            {
                var refreshToken = JsonSerializer.Deserialize<RefreshToken>(json, SzarotkaJsonSerializerContext.Default.RefreshToken);
                return refreshToken;
            }
            return new();
        }

        public void ClearTokens()
        {
            SecureStorage.Default.Remove("access_token");
            SecureStorage.Default.Remove("refresh_token");
        }
    }


    public static partial class UserAfterLogin
    {
        public static DateTime Expires { get; private set; } = new();
        public static bool IsLogin { get; private set; } = false;
        public static User User { get; private set; } = new();

        public static event Action<User, bool> OnLogin;

        private readonly static IAccessDataBaseAoT _db;
        private readonly static IAuthService _auth;
        private readonly static ILoginHttp _loginHttp;

        static UserAfterLogin()
        {
            var db = Shared.Service.AppServiceProvider.GetService<IAccessDataBaseAoT>();
            var auth = Shared.Service.AppServiceProvider.GetService<IAuthService>();
            var loginHttp = Shared.Service.AppServiceProvider.GetService<ILoginHttp>();
            if (db is not null)
            {
                _db = db;
            }
            if (auth is not null)
            {
                _auth = auth;
            }
            if (loginHttp is not null)
            {
                _loginHttp = loginHttp;
            }

            OnLogin?.Invoke(new(), true);
        }

        static bool _tryToLogin = true;

        public static async Task SetLoginUser(User token)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(token);

                var (user, expires) = DataBase.Helper.ReadToken.GetUserFromToken(token.AccessToken);
                User = user;
                Expires = expires;
                IsLogin = true;
                OnLogin?.Invoke(User, IsLogin);

                await _auth.SaveTokensAsync(user.AccessToken, user.RefreshToken);
                _tryToLogin = true;
            }
            catch (Microsoft.IdentityModel.Tokens.SecurityTokenException)
            {
                if (IsLogin)
                {
                    _tryToLogin = false;
                    var newToken = await _loginHttp.RefreshToken(User.RefreshToken);
                    await SetLoginUser(newToken);
                }
                else
                {
                    await RemoveLoginUser();
                }
            }
            catch (ArgumentException ex)
            {
                await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
            }
            catch (Exception ex)
            {
                await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                _db.SaveLogExtension(ex);
            }
        }

        public static async Task SetLoginUser()
        {
            var refreshToken = await _auth.GetRefreshTokenAsync();
            if (User is not null)
            {
                var user = await _loginHttp.RefreshToken(refreshToken);
                await SetLoginUser(user);
            }
        }



        public static async Task RemoveLoginUser()
        {
            var token = await _auth.GetRefreshTokenAsync();
            var result = await _loginHttp.Out(token);
            if (result.IsSuccessStatusCode)
            {
                Expires = new();
                IsLogin = false;
                User = new();
                _auth.ClearTokens();
                OnLogin?.Invoke(User, IsLogin);
            }
            else
            {
                await Toast.Make("Nie udało się wylogować", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
            }
        }
    }
}
