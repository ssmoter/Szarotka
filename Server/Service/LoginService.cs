using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Helper;
using Server.SqlQuery;
using Server.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Server.Service
{
    public interface ILoginService
    {
        Task<User> GetPublicUser(string id);
        Task<User> LogIn(LoginUser user);
        Task<IResult> LogOut(LoginUser user);
    }

    public class LoginService(IAccessDataBase db, ITimeService timeService, IUserValidation userValidation, ILogger<LoginService>? logger = null) : ILoginService
    {
        private readonly IAccessDataBase _db = db;
        private readonly ITimeService _timeService = timeService;
        private readonly IUserValidation _userValidation = userValidation;
        private readonly ILogger<LoginService> _logger = logger ?? NullLogger<LoginService>.Instance;

        public async Task<User> LogIn(LoginUser user)
        {
            _logger.LogInformation("LogIn started for email={Email}", user?.Email);
            user.Password = Hash.PasswordSHA256(user.Password);

            var sql = LoginQuery.In(user.Email, user.Password);

            var dbUser = await _db.DataBaseAsync.QueryAsync<User>(sql, user.Email, user.Password);

            var firstUser = dbUser.FirstOrDefault();

            _userValidation.AccountNotFound(firstUser);

            _userValidation.Validation.Throw();

            firstUser!.UserUpdatedId = firstUser.Id;
            if (user.RememberMe != firstUser.RememberMe)
            {
                firstUser.RememberMe = user.RememberMe;
                firstUser.Updated = _timeService.UtcNow();
                sql = LoginQuery.UpdateRememberMe(firstUser.RememberMe, firstUser.UpdatedTicks, firstUser.UserUpdatedId, firstUser.Id);
                _ = await _db.DataBaseAsync.ExecuteAsync(sql, firstUser.RememberMe, firstUser.UpdatedTicks, firstUser.UserUpdatedId, firstUser.Id);
                _logger.LogInformation("LogIn: updated RememberMe for userId={UserId} to {RememberMe}", firstUser.Id, firstUser.RememberMe);
            }

            _logger.LogInformation("LogIn succeeded for userId={UserId}", firstUser.Id);
            return firstUser;
        }

        public async Task<IResult> LogOut(LoginUser user)
        {
            _logger.LogInformation("LogOut started for email={Email}", user?.Email);
            await Task.Delay(1);
            _logger.LogInformation("LogOut completed for email={Email}", user?.Email);
            return Results.Ok();
        }


        public async Task<User> GetPublicUser(string id)
        {
            _logger.LogInformation("GetPublicUser started for id={Id}", id);
            var sql = LoginQuery.PublicUser(id);
            string Id = id;

            var users = await _db.DataBaseAsync.QueryAsync<User>(sql, Id);
            var user = users.FirstOrDefault();

            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return user;

        }


    }
}
