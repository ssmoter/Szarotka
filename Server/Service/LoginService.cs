using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Helper;
using Server.SqlQuery;

namespace Server.Service
{
    public interface ILoginService
    {
        Task<User> LogIn(LoginUser user);
        Task<IResult> LogOut(LoginUser user);
    }

    public class LoginService : ILoginService
    {
        private readonly IAccessDataBase _db;
        private readonly ITimeService _timeService;

        public LoginService(IAccessDataBase db, ITimeService timeService)
        {
            _db = db;
            _timeService = timeService;
        }

        public async Task<User> LogIn(LoginUser user)
        {
            user.Password = Hash.PasswordSHA256(user.Password);

            var sql = LoginQuery.In(user);

            var dbUser = await _db.DataBaseAsync.QueryAsync<User>(sql);

            var firstUser = dbUser.FirstOrDefault();

            if (firstUser is null)
            {
                var valid = new ValidationException();
                valid.AddError("Account not found", EnumsList.Validation.AccountNotFound);
                throw valid;
            }
            firstUser.UserUpdatedId = firstUser.Id;
            if (user.RememberMe != firstUser.RememberMe)
            {
                firstUser.RememberMe = user.RememberMe;
                firstUser.Updated = _timeService.UtcNow();
                sql = LoginQuery.UpdateRememberMe(firstUser);
                _ = await _db.DataBaseAsync.ExecuteAsync(sql);
            }

            return firstUser;
        }

        public async Task<IResult> LogOut(LoginUser user)
        {
            await Task.Delay(1);
            return Results.Ok();
        }

    }
}
