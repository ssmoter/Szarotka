using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Helper;
using Server.SqlQuery;
using Server.Validation;

namespace Server.Service
{
    public interface IResetPasswordService
    {
        Task ChangePassword(Guid id, string password);
        Task<ConfirmCode?> GetConfirmCode(int code);
        Task<User> GetUserIdFromEmail(string email);
    }

    public class ResetPasswordService : IResetPasswordService
    {
        private readonly IAccessDataBase _db;
        private readonly IUserValidation _userValidation;
        private readonly ITimeService _time;

        public ResetPasswordService(IAccessDataBase db, IUserValidation userValidation, ITimeService time)
        {
            _db = db;
            _userValidation = userValidation;
            _time = time;
        }

        public async Task<User> GetUserIdFromEmail(string email)
        {
            string sql = UserQuery.GetIdFromEmail(email);
            var Email = email;
            var ids = await _db.DataBaseAsync.QueryAsync<User>(sql, Email);
            var id = ids.FirstOrDefault();

            _userValidation.AccountNotFound(id);

            _userValidation.Validation.Throw();

            return id!;
        }

        public async Task<ConfirmCode?> GetConfirmCode(int code)
        {
            var sql = UserQuery.CodeConfirmCheck(code);
            var result = await _db.DataBaseAsync.QueryAsync<ConfirmCode>(sql, code);
            var codeResult = result.FirstOrDefault();
            return codeResult;
        }

        public async Task ChangePassword(Guid id, string password)
        {
            var Password = Hash.PasswordSHA256(password);
            var UpdateTicks = _time.UtcNow().Ticks;
            Guid Id = id;
            Guid UserUpdatedId = id;
            var slq = UserQuery.UpdatePassword(Password, UpdateTicks, UserUpdatedId, Id);
            await _db.DataBaseAsync.ExecuteAsync(slq, Password, UpdateTicks, UserUpdatedId, Id);
        }


    }
}
