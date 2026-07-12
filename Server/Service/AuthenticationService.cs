
using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.IdentityModel.Tokens;

using Server.Model;
using Server.SqlQuery;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

    public class AuthenticationService(JSONWebTokensSettings jSONWebTokensSettings, IAccessDataBase db, ILogger<AuthenticationService>? logger = null) : IAuthenticationService
    {
        private readonly JSONWebTokensSettings _jwtSettings = jSONWebTokensSettings;
        private readonly IAccessDataBase _db = db;
        private readonly JwtSecurityTokenHandler _handler = new();
        private readonly ILogger<AuthenticationService> _logger = logger ?? NullLogger<AuthenticationService>.Instance;

        public async Task<User> AuthenticateAsync(User request)
        {
            _logger.LogInformation("AuthenticateAsync(User) started for userId={UserId}", request?.Id);
            JwtSecurityToken jwtSecurityToken = await GenerateToken(request);

            request.Token = _handler.WriteToken(jwtSecurityToken);
            _logger.LogInformation("AuthenticateAsync(User) generated token for userId={UserId}", request.Id);
            return request;
        }
        public async Task<User> AuthenticateAsync(string token)
        {
            _logger.LogInformation("AuthenticateAsync(token) started");
            var result = DataBase.Helper.ReadToken.GetUserFromToken(token);
            var id = result.user.Id.ToString();
            var sql = LoginQuery.InFromId(id);
            var users = await _db.DataBaseAsync.QueryAsync<User>(sql, new { Id = id });
            var user = users.FirstOrDefault();

            if (user is null)
            {
                _logger.LogWarning("AuthenticateAsync: user not found for id={Id}", id);
                throw new UnauthorizedAccessException();
            }

            if (user.Id == result.user.Id)
            {
                JwtSecurityToken jwtSecurityToken = await GenerateToken(result.user);
                result.user.Token = _handler.WriteToken(jwtSecurityToken);
                _logger.LogInformation("AuthenticateAsync(token) succeeded for userId={UserId}", result.user.Id);
                return result.user;
            }
            _logger.LogWarning("AuthenticateAsync: token user id mismatch for id={Id}", id);
            throw new UnauthorizedAccessException();
        }



        private async Task<JwtSecurityToken> GenerateToken(User user)
        {
            _logger.LogInformation("GenerateToken started for userId={UserId}", user?.Id);
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
                new Claim(ClaimTypes.Role,user.UserType.ToString()),
                new Claim(JwtRegisteredClaimNames.EmailVerified,user.IsEmailConfirm.ToString()),
                new Claim(nameof(User.CreatedTicks),user.CreatedTicks.ToString()),
                new Claim(nameof(User.UpdatedTicks),user.UpdatedTicks.ToString()),
                new Claim(nameof(User.UserUpdatedId),user.UserUpdatedId.ToString()),
                new Claim(nameof(User.IsDelete),user.IsDelete.ToString()),
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
