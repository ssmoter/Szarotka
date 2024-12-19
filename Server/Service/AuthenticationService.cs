
using DataBase.Model.EntitiesServer;

using Microsoft.IdentityModel.Tokens;

using Server.Model;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

namespace Server.Service
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(User request);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly JSONWebTokensSettings _jwtSettings;

        public AuthenticationService(JSONWebTokensSettings jSONWebTokensSettings)
        {
            _jwtSettings = jSONWebTokensSettings;
        }


        public async Task<User> AuthenticateAsync
        (User request)
        {
            JwtSecurityToken jwtSecurityToken = await GenerateToken(request);

            request.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return request;
        }

        private async Task<JwtSecurityToken> GenerateToken(User user)
        {
            //var userClaims = await _userManager.GetClaimsAsync(user);
            //var roles = await _userManager.GetRolesAsync(user);

            //var roleClaims = new List<Claim>();

            //for (int i = 0; i < roles.Count; i++)
            //{
            //    roleClaims.Add(new Claim(ClaimTypes.Role, roles[i]));
            //}

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.UserType.ToString())
             };
            //  .Union(userClaims)
            //  .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return await Task.FromResult(jwtSecurityToken);
        }






    }
}
