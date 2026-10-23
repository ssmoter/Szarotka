using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.Extensions.Logging.Abstractions;

using Server.Helper;
using Server.SqlQuery;
using Server.Validation;

namespace Server.Service
{
    public interface ILoginService
    {
        Task<User> GetPublicUser(string id);
        Task<User> LogIn(LoginUser user);
        Task<IResult> LogOut(string refreshToken);
    }

    public class LoginService(IAccessDataBaseAoT db, ITimeService timeService, IUserValidation userValidation, ILogger<LoginService>? logger = null) : ILoginService
    {
        private readonly IAccessDataBaseAoT _db = db;
        private readonly ITimeService _timeService = timeService;
        private readonly IUserValidation _userValidation = userValidation;
        private readonly ILogger<LoginService> _logger = logger ?? NullLogger<LoginService>.Instance;

        public async Task<User> LogIn(LoginUser user)
        {
            _logger.LogInformation("LogIn started for email={Email}", user?.Email);
            user!.Password = Hash.PasswordSHA256(user.Password);

            var sql = LoginQuery.In(user.Email, user.Password);

            var dbUser = await _db.DbAsyncAoT.QueryAsync<User>(sql, param: new()
            {
                [nameof(user.Email)] = user.Email,
                [nameof(user.Password)] = user.Password
            });

            var firstUser = dbUser.FirstOrDefault();

            _userValidation.AccountNotFound(firstUser);

            _userValidation.Validation.Throw();

            firstUser!.UserUpdatedId = firstUser.Id;

            _logger.LogInformation("LogIn succeeded for userId={UserId}", firstUser.Id);
            return firstUser;
        }

        public async Task<IResult> LogOut(string refreshToken)
        {
            _logger.LogInformation("LogOut started for token={Token}", refreshToken);

            var deleteSql = $"DELETE FROM {nameof(RefreshToken)} WHERE {nameof(RefreshToken.Value)} = @Value";
            await _db.DbAsyncAoT.ExecuteAsync(deleteSql, param: new() { ["Value"] = refreshToken });

            _logger.LogInformation("LogOut completed for token={Token}", refreshToken);
            return Results.Ok();
        }


        public async Task<User> GetPublicUser(string id)
        {
            _logger.LogInformation("GetPublicUser started for id={Id}", id);
            var sql = LoginQuery.PublicUser(id);
            IEnumerable<User> users = await _db.DbAsyncAoT.QueryAsync<User>(sql, param: new()
            {
                [nameof(id)] = id
            });
            var user = users.FirstOrDefault();

            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return user;

        }


    }
}
