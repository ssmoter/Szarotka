using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Helper;
using Server.Model;
using Server.SqlQuery;
using Server.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
        private readonly ILogger<RegisterUserService> _logger;

        public RegisterUserService(IAccessDataBase db, IUserValidation userValidation, ITimeService time, IConfiguration configuration, ILogger<RegisterUserService>? logger = null)
        {
            _db = db;
            _userValidation = userValidation;
            _time = time;
            _logger = logger ?? NullLogger<RegisterUserService>.Instance;

            var section = configuration.GetSection(nameof(EmailConfiguration)).Get<EmailConfiguration>();
            if (section is not null)
            {
                _emailConfig = section;
            }
        }

        public async Task<RegisterUser> InsertNewUser(RegisterUser registerUser)
        {
            _logger.LogInformation("InsertNewUser started for email={Email}", registerUser?.Email);
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
                _logger.LogInformation("InsertNewUser succeeded for email={Email} userId={UserId}", registerUser.Email, registerUser.Id);
                return registerUser;
            }
            if (task.IsFaulted)
            {
                _logger.LogError(task.Exception, "InsertNewUser failed for email={Email}", registerUser.Email);
                throw task.Exception!;
            }
            _logger.LogError("InsertNewUser unexpected failure for email={Email}", registerUser.Email);
            throw new ArgumentException("Nie uało się dodać użytkownika");
        }
        public async Task InsertCodeEmailAndRemoveOld(ConfirmCode user)
        {
            _logger.LogInformation("InsertCodeEmailAndRemoveOld started for userId={UserId}", user?.UserId);
            var now = _time.UtcNow();
            user.ExpireDate = now.AddMinutes(_emailConfig.ExpireDateMinutes).Ticks;
            var codeOldTaskSql = UserQuery.RemoveExpireCode(now.Ticks);
            var codeOldTask = _db.DataBaseAsync.ExecuteAsync(codeOldTaskSql, now.AddDays(-7).Ticks);
            var codeNewTaskSql = UserQuery.ConfirmCodeInsert(user.CreatedTicks, user.UpdatedTicks, user.UserId, user.Code, user.ExpireDate);
            var codeNewTask = _db.DataBaseAsync.ExecuteAsync(codeNewTaskSql, user.CreatedTicks, user.UpdatedTicks, user.UserId, user.Code, user.ExpireDate);

            try
            {
                await Task.WhenAll(codeOldTask, codeNewTask);
                _logger.LogInformation("InsertCodeEmailAndRemoveOld succeeded for userId={UserId}", user.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InsertCodeEmailAndRemoveOld failed for userId={UserId}", user.UserId);
                throw;
            }
        }
        public async Task<User> GetUserEmailFromCodeAndRemoveOld(int code)
        {
            _logger.LogInformation("GetUserEmailFromCodeAndRemoveOld started for code={Code}", code);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserEmailFromCodeAndRemoveOld: database operations failed for code={Code}", code);
                throw;
            }

            var email = emailTask.Result.FirstOrDefault();

            _userValidation.AccountNotFound(email);

            _userValidation.Validation.Throw();

            _logger.LogInformation("GetUserEmailFromCodeAndRemoveOld succeeded for code={Code} userId={UserId}", code, email?.Id);
            return email!;
        }
    }
}
