using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.IdentityModel.Tokens;

using Shared.Data;
using Shared.Model;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shared.Helper
{
    public partial class UserAfterLogin
    {
        public static DateTime Expires { get; private set; } = new();
        public static bool IsLogin { get; private set; } = false;
        public static User User { get; private set; } = new();
        public static event Action<User, bool> OnLogin;

        private static readonly JwtSecurityTokenHandler _handler = new();
        private readonly static AccessDataBase _db;

        static UserAfterLogin()
        {
            _db = Shared.Service.AppServiceProvider.Current.GetRequiredService<AccessDataBase>();
        }

        public static void SetLoginUser(User token)
        {
            SetLoginUser(token.Token);
        }
        public static void SetLoginUser(string token)
        {
            //JwtSecurityToken jwtToken = _handler.ReadJwtToken(token);
            try
            {
                _handler.ValidateToken(token
                    , new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("81234CFB77034ECCDDD547F5SADFAASADSFGAFGDFAEWFCVZXVB")),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                foreach (var item in jwtToken.Claims)
                {
                    if (item.Type == JwtRegisteredClaimNames.Sub)
                    {
                        User.Name = item.Value;
                    }
                    if (item.Type == JwtRegisteredClaimNames.Email)
                    {
                        User.Email = item.Value;
                    }
                    if (item.Type == JwtRegisteredClaimNames.PhoneNumber)
                    {
                        User.PhoneNumber = item.Value;
                    }
                    if (item.Type == ClaimTypes.Role)
                    {
                        User.UserType = Enum.Parse<UserType>(item.Value);
                    }
                    if (item.Type == JwtRegisteredClaimNames.NameId)
                    {
                        User.Id = Guid.Parse(item.Value);
                    }
                }

                Expires = jwtToken.ValidTo;
                IsLogin = true;
                User.Token = token;
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
