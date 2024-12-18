using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Helper;
using Server.Model;
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
        private readonly AccessDataBase _db;

        public LoginService(AccessDataBase db)
        {
            _db = db;
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
            return firstUser;
        }

        public async Task<IResult> LogOut(LoginUser user)
        {
            return Results.Ok();
        }

    }
}
