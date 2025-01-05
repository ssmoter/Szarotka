
using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.IdentityModel.Tokens;

using Server.Model;
using Server.SqlQuery;

using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;

using System.Text;

namespace Server.Service
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsync(User request);
        Task<User> AuthenticateAsync(string token);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly JSONWebTokensSettings _jwtSettings;
        private readonly AccessDataBase _db;
        private readonly JwtSecurityTokenHandler _handler;
        public AuthenticationService(JSONWebTokensSettings jSONWebTokensSettings, AccessDataBase db)
        {
            _jwtSettings = jSONWebTokensSettings;
            _db = db;
            _handler = new JwtSecurityTokenHandler();
        }


        public async Task<User> AuthenticateAsync(User request)
        {
            JwtSecurityToken jwtSecurityToken = await GenerateToken(request);

            request.Token = _handler.WriteToken(jwtSecurityToken);

            return request;
        }

        public async Task<User> AuthenticateAsync(string token)
        {
            var jwtToken = _handler.ReadJwtToken(token);

            var claimId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId);
            string id = claimId?.Value ?? throw new ArgumentNullException(nameof(claimId), "Claim not found");
           
            var users = await _db.DataBaseAsync.QueryAsync<User>(LoginQuery.InFromId(id));
            var user = users.FirstOrDefault();

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            JwtSecurityToken jwtSecurityToken = await GenerateToken(user);
            user.Token = _handler.WriteToken(jwtSecurityToken);
            return user;
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
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.UserType.ToString())
             };
            //  .Union(userClaims)
            //  .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            DateTime expires = user.RememberMe ? DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays) : DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: signingCredentials);

            return await Task.FromResult(jwtSecurityToken);
        }



    }
}
