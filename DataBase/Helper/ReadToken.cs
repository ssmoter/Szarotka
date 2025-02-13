using DataBase.Model.EntitiesServer;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DataBase.Helper;

public static class ReadToken
{
    private static readonly JwtSecurityTokenHandler _handler = new();
    public static (User user, DateTime expires) GetUserFromToken(string token)
    {
        var user = new User();
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
                    user.Name = item.Value;
                }
                if (item.Type == JwtRegisteredClaimNames.Email)
                {
                    user.Email = item.Value;
                }
                if (item.Type == JwtRegisteredClaimNames.PhoneNumber)
                {
                    user.PhoneNumber = item.Value;
                }
                if (item.Type == ClaimTypes.Role)
                {
                    user.UserType = Enum.Parse<UserType>(item.Value);
                }
                if (item.Type == JwtRegisteredClaimNames.NameId)
                {
                    user.Id = Guid.Parse(item.Value);
                }
            }
            user.Token = token;
            return (user, jwtToken.ValidTo);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public static string RemoveBearer(string token)
    {
        return token["Bearer ".Length..].Trim();
    }
}
