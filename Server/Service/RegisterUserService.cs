using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Helper;
using Server.SqlQuery;
using Server.Validation;

namespace Server.Service
{
    public interface IRegisterUserService
    {
        Task<User> GetUserEmailFromCodeAndRemoveOld(int code);
        Task InsertCodeEmailAndRemoveOld(ConfirmCode user);
        Task<RegisterUser> InsertNewUser(RegisterUser registerUser);
    }

    public class RegisterUserService : IRegisterUserService
    {
        private readonly IAccessDataBase _db;
        private readonly IUserValidation _userValidation;
        private readonly ITimeService _time;
        public RegisterUserService(IAccessDataBase db, IUserValidation userValidation, ITimeService time)
        {
            _db = db;
            _userValidation = userValidation;
            _time = time;
        }

        public async Task<RegisterUser> InsertNewUser(RegisterUser registerUser)
        {
            registerUser.IsDelete = false;
            registerUser.IsEmailConfirm = false;
            var time = _time.UtcNow();
            if (registerUser.Created == DateTime.MinValue)
            {
                registerUser.Created = time;
            }
            registerUser.Updated = time;
            if (registerUser.Id == Guid.Empty)
            {
                registerUser.Id = Guid.CreateVersion7();
            }
            registerUser.Password = Hash.PasswordSHA256(registerUser.Password);

            var query = UserQuery.RegisterNewUser(registerUser);
            var task = _db.DataBaseAsync.ExecuteAsync(query);

            await task;

            if (task.IsCompletedSuccessfully)
            {
                return registerUser;
            }

            throw new ArgumentException();
        }
        public async Task InsertCodeEmailAndRemoveOld(ConfirmCode user)
        {
            var now = _time.UtcNow();
            user.ExpireDate = now.AddMinutes(10).Ticks;
            var codeOldTask = _db.DataBaseAsync.ExecuteAsync(UserQuery.RemoveExpireCode(now.Ticks));
            var codeNewTask = _db.DataBaseAsync.ExecuteAsync(UserQuery.EmailConfirmInsert(user));

            await Task.WhenAll(codeOldTask, codeNewTask);
        }
        public async Task<User> GetUserEmailFromCodeAndRemoveOld(int code)
        {
            var sql = UserQuery.EmailConfirmCheck(code);
            var userEmails = await _db.DataBaseAsync.QueryAsync<ConfirmCode>(sql);
            var userEmail = userEmails.FirstOrDefault();

            _userValidation.CodeNotExist(userEmail);
            if (userEmail is null)
            {
                throw _userValidation.Validation.Throw();
            }
            _userValidation.CodeIsExpire(userEmail);

            if (_userValidation.Validation.ValidationErrors.Count > 0)
            {
                throw _userValidation.Validation.Throw();
            }

            var user = new User()
            {
                Id = userEmail.UserId,
                IsEmailConfirm = true,
                Updated = _time.UtcNow(),
                UserUpdatedId = userEmail.UserId
            };
            var userSql = UserQuery.EmailIsConfirmUpdate(user);
            var userTask = _db.DataBaseAsync.ExecuteAsync(userSql);

            var codeSql = UserQuery.RemoveExpireCode(_time.UtcNow().Ticks);
            var codeTask = _db.DataBaseAsync.ExecuteAsync(codeSql);

            var emailSql = UserQuery.GetEmailFromId(userEmail.UserId);
            var emailTask = _db.DataBaseAsync.QueryAsync<User>(emailSql);

            try
            {
                await Task.WhenAll(userTask, codeTask, emailTask);
            }
            catch (Exception)
            {
                throw;
            }

            var email = emailTask.Result.FirstOrDefault();

            _userValidation.AccountNotFound(email);

            if (email is null)
            {
                throw _userValidation.Validation.Throw();
            }

            return email;
        }
    }
}
