using CommunityToolkit.Maui.Alerts;

using DataBase.Data;
using DataBase.Helper;
using DataBase.Model.EntitiesServer;
using DataBase.Model.SourceGenerator;

using Shared.Data;
using Shared.Data.ServerHttpClients;

using System.Net;
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
            if (!string.IsNullOrWhiteSpace(jsonRefreshToken))
            {
                await SecureStorage.Default.SetAsync("refresh_token", jsonRefreshToken);
            }
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                await SecureStorage.Default.SetAsync("access_token", accessToken);
            }
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

        public static async Task SetLoginUser(User token, bool validateLifetime = true)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(token);
                await _auth.SaveTokensAsync(token.AccessToken, token.RefreshToken);

                var (user, expires) = DataBase.Helper.ReadToken.GetUserFromToken(token.AccessToken, validateLifetime);
                User = user;
                Expires = expires;
                IsLogin = true;

                OnLogin?.Invoke(User, IsLogin);
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
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
            }
            catch (ArgumentException ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                });
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                });
                _db.SaveLogExtension(ex);
            }
        }

        public static async Task SetLoginUser()
        {
            try
            {
                var refreshToken = await _auth.GetRefreshTokenAsync();
                var accessToken = await _auth.GetAccessTokenAsync();
                User user = new();
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    (user, _) = DataBase.Helper.ReadToken.GetUserFromToken(accessToken, false);
                }
                if (!string.IsNullOrWhiteSpace(refreshToken.Value))
                {
                    if (refreshToken.ExpireDate > _db.TimeService.UtcNow().Ticks)
                    {
                        user.RefreshToken = refreshToken;
                        await SetLoginUser(user, false);
                    }
                    else
                    {
                        user = await _loginHttp.RefreshToken(refreshToken);
                        await SetLoginUser(user);
                    }

                }
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Toast.Make(ex.Message, CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                });
                _db.SaveLogExtension(ex);
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
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Toast.Make("Nie udało się wylogować", CommunityToolkit.Maui.Core.ToastDuration.Long).Show();
                });
            }
        }
    }
}
