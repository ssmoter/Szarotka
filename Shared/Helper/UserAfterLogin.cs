using DataBase.Model.EntitiesServer;

using Microsoft.IdentityModel.Tokens;

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
        public static void SetLoginUser(User token)
        {

            //JwtSecurityToken jwtToken = _handler.ReadJwtToken(token);
            _handler.ValidateToken(token.Token
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
            User.Token = token.Token;
            OnLogin?.Invoke(User, IsLogin);
        }
        public static void RemoveLoginUser()
        {
            Expires = new();
            IsLogin = false;
            User = new();
            OnLogin?.Invoke(User, IsLogin);
        }
    }
}
