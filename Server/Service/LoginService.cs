using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Helper;
using Server.SqlQuery;
using Server.Validation;

namespace Server.Service
{
    public interface ILoginService
    {
        Task<User> GetPublicUser(string id);
        Task<User> LogIn(LoginUser user);
        Task<IResult> LogOut(LoginUser user);
    }

    public class LoginService : ILoginService
    {
        private readonly IAccessDataBase _db;
        private readonly ITimeService _timeService;
        private readonly IUserValidation _userValidation;

        public LoginService(IAccessDataBase db, ITimeService timeService, IUserValidation userValidation)
        {
            _db = db;
            _timeService = timeService;
            _userValidation = userValidation;
        }

        public async Task<User> LogIn(LoginUser user)
        {
            user.Password = Hash.PasswordSHA256(user.Password);

            var sql = LoginQuery.In(user.Email, user.Password);

            var dbUser = await _db.DataBaseAsync.QueryAsync<User>(sql, user.Email, user.Password);

            var firstUser = dbUser.FirstOrDefault();

            _userValidation.AccountNotFound(firstUser);
            if (firstUser is null)
            {
                throw _userValidation.Validation.Throw();
            }
            firstUser.UserUpdatedId = firstUser.Id;
            if (user.RememberMe != firstUser.RememberMe)
            {
                firstUser.RememberMe = user.RememberMe;
                firstUser.Updated = _timeService.UtcNow();
                sql = LoginQuery.UpdateRememberMe(firstUser.RememberMe, firstUser.UpdatedTicks, firstUser.UserUpdatedId, firstUser.Id);
                _ = await _db.DataBaseAsync.ExecuteAsync(sql, firstUser.RememberMe, firstUser.UpdatedTicks, firstUser.UserUpdatedId, firstUser.Id);
            }

            return firstUser;
        }

        public async Task<IResult> LogOut(LoginUser user)
        {
            await Task.Delay(1);
            return Results.Ok();
        }


        public async Task<User> GetPublicUser(string id)
        {
            var sql = LoginQuery.PublicUser(id);
            string Id = id;

            var users = await _db.DataBaseAsync.QueryAsync<User>(sql, Id);
            var user = users.FirstOrDefault();

            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return user;

        }


    }
}
