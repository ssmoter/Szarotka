
using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;

using Server.Model;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Server.Service
{
    public interface IAuthenticationService
    {
        Task<User> AuthenticateAsyncAccess(User request);
        Task<User> AuthenticateAsyncRefresh(User request);
    }

    public class AuthenticationService(JSONWebTokensSettings jSONWebTokensSettings, IAccessDataBaseAoT db, ILogger<AuthenticationService>? logger = null) : IAuthenticationService
    {
        private readonly JSONWebTokensSettings _jwtSettings = jSONWebTokensSettings;
        private readonly IAccessDataBaseAoT _db = db;
        private readonly JwtSecurityTokenHandler _handler = new();
        private readonly ILogger<AuthenticationService> _logger = logger ?? NullLogger<AuthenticationService>.Instance;

        public async Task<User> AuthenticateAsyncAccess(User request)
        {
            _logger.LogInformation("AuthenticateAsyncAccess(User) started for userId={UserId}", request.Id);
            JwtSecurityToken jwtSecurityToken = await GenerateToken(request);

            request.AccessToken = _handler.WriteToken(jwtSecurityToken);
            _logger.LogInformation("AuthenticateAsyncAccess(User) generated token for userId={UserId}", request.Id);
            return request;
        }
        public async Task<User> AuthenticateAsyncRefresh(User request)
        {
            _logger.LogInformation("AuthenticateAsyncRefresh(User) started for userId={UserId}", request.Id);

            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            request.RefreshToken = new RefreshToken
            {
                UserId = request.Id.ToString(),
                Value = Convert.ToBase64String(randomNumber),
                ExpireDate = request.RememberMe ? _db.TimeService.UtcNow().AddDays(_jwtSettings.DurationInRefreshTokenLong).Ticks : _db.TimeService.UtcNow().AddDays(_jwtSettings.DurationInRefreshTokenShort).Ticks
            };

            var deleteTime = _db.TimeService.UtcNow().AddHours(-1).Ticks;
            var deleteSql = $"DELETE FROM RefreshTokens WHERE {nameof(RefreshToken.ExpireDate)} < @Now";
            await _db.DbAsyncAoT.ExecuteAsync(deleteSql, new() { ["Now"] = deleteTime });

            var insertSql = $"INSERT INTO RefreshTokens ({nameof(RefreshToken.Value)}, {nameof(RefreshToken.ExpireDate)},{nameof(RefreshToken.UserId)}) VALUES (@{nameof(RefreshToken.Value)}, @{nameof(RefreshToken.ExpireDate)}, @{nameof(RefreshToken.UserId)})";
            await _db.DbAsyncAoT.ExecuteAsync(insertSql, new()
            {
                [nameof(RefreshToken.Value)] = request.RefreshToken.Value,
                [nameof(RefreshToken.ExpireDate)] = request.RefreshToken.ExpireDate,
                [nameof(RefreshToken.UserId)] = request.RefreshToken.UserId
            });

            _logger.LogInformation("AuthenticateAsyncRefresh(User) generated token for userId={UserId}", request.Id);
            return request;
        }


        private async Task<JwtSecurityToken> GenerateToken(User user)
        {
            _logger.LogInformation("GenerateToken started for userId={UserId}", user.Id);
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

            var expires = _db.TimeService.UtcNow().AddMinutes(_jwtSettings.DurationInAccessToken);


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
