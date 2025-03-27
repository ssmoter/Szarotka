using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Helper;
using Server.Model;
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
        private readonly EmailConfiguration _emailConfig = new();

        public RegisterUserService(IAccessDataBase db, IUserValidation userValidation, ITimeService time, IConfiguration configuration)
        {
            _db = db;
            _userValidation = userValidation;
            _time = time;

            var section = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
            if (section is not null)
            {
                _emailConfig = section;
            }


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

            var query = UserQuery.RegisterNewUser(registerUser.Id,
                                                  registerUser.CreatedTicks,
                                                  registerUser.UpdatedTicks,
                                                  registerUser.Name,
                                                  registerUser.Description,
                                                  registerUser.Email,
                                                  registerUser.PhoneNumber,
                                                  registerUser.UserType,
                                                  registerUser.IsDelete,
                                                  registerUser.IsEmailConfirm,
                                                  registerUser.Password);
            var task = _db.DataBaseAsync.ExecuteAsync(query,
                                                      registerUser.Id,
                                                      registerUser.CreatedTicks,
                                                      registerUser.UpdatedTicks,
                                                      registerUser.Name,
                                                      registerUser.Description,
                                                      registerUser.Email,
                                                      registerUser.PhoneNumber,
                                                      registerUser.UserType,
                                                      registerUser.IsDelete,
                                                      registerUser.IsEmailConfirm,
                                                      registerUser.Password);

            await task;

            if (task.IsCompletedSuccessfully)
            {
                return registerUser;
            }
            if (task.IsFaulted)
            {
                throw task.Exception;
            }
            throw new ArgumentException();
        }
        public async Task InsertCodeEmailAndRemoveOld(ConfirmCode user)
        {
            var now = _time.UtcNow();
            user.ExpireDate = now.AddMinutes(_emailConfig.ExpireDateMinutes).Ticks;
            var codeOldTaskSql = UserQuery.RemoveExpireCode(now.Ticks);
            var codeOldTask = _db.DataBaseAsync.ExecuteAsync(codeOldTaskSql, now.AddDays(-7).Ticks);
            var codeNewTaskSql = UserQuery.ConfirmCodeInsert(user.CreatedTicks, user.UpdatedTicks, user.UserId, user.Code, user.ExpireDate);
            var codeNewTask = _db.DataBaseAsync.ExecuteAsync(codeNewTaskSql, user.CreatedTicks, user.UpdatedTicks, user.UserId, user.Code, user.ExpireDate);

            await Task.WhenAll(codeOldTask, codeNewTask);
        }
        public async Task<User> GetUserEmailFromCodeAndRemoveOld(int code)
        {
            var sql = UserQuery.CodeConfirmCheck(code);
            var userEmails = await _db.DataBaseAsync.QueryAsync<ConfirmCode>(sql, code);
            var userEmail = userEmails.FirstOrDefault();

            _userValidation.CodeNotExist(userEmail);

            _userValidation.Validation.Throw();

            _userValidation.CodeIsExpire(userEmail!);

            _userValidation.Validation.Throw();


            var user = new User()
            {
                Id = userEmail!.UserId,
                IsEmailConfirm = true,
                Updated = _time.UtcNow(),
                UserUpdatedId = userEmail.UserId
            };
            var userSql = UserQuery.EmailIsConfirmUpdate(user.IsEmailConfirm, user.UpdatedTicks, user.UserUpdatedId, user.Id);
            var userTask = _db.DataBaseAsync.ExecuteAsync(userSql, user.IsEmailConfirm, user.UpdatedTicks, user.UserUpdatedId, user.Id);

            var TicksNow = _time.UtcNow().Ticks;

            var codeSql = UserQuery.RemoveExpireCode(TicksNow);
            var codeTask = _db.DataBaseAsync.ExecuteAsync(codeSql, TicksNow);

            var emailSql = UserQuery.GetEmailFromId(userEmail.UserId);
            var emailTask = _db.DataBaseAsync.QueryAsync<User>(emailSql, userEmail.UserId);

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

            _userValidation.Validation.Throw();

            return email!;
        }
    }
}
