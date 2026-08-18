using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Microsoft.Extensions.Logging.Abstractions;

using Server.Helper;
using Server.Model;
using Server.SqlQuery;
using Server.Validation;

namespace Server.Service
{
    public interface IRegisterUserService
    {
        Task<User> InsertUserAfterConfirmEmail(int code);
        ConfirmCode CreatedConfirmCode(ConfirmCode user);
        Task<RegisterUser> CheckUserBeforInsert(RegisterUser registerUser);
    }

    public class RegisterUserService : IRegisterUserService
    {
        private readonly IAccessDataBaseAoT _db;
        private readonly IUserValidation _userValidation;
        private readonly ITimeService _time;
        private readonly EmailConfiguration _emailConfig = new();
        private readonly ILogger<RegisterUserService> _logger;

        public RegisterUserService(IAccessDataBaseAoT db, IUserValidation userValidation, ITimeService time, IConfiguration configuration, ILogger<RegisterUserService>? logger = null)
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

        public async Task<RegisterUser> CheckUserBeforInsert(RegisterUser registerUser)
        {
            _logger.LogInformation("CheckUserBeforInsert started for email={Email}", registerUser.Email);
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
            registerUser.UserCreatedId = registerUser.Id;
            registerUser.UserUpdatedId = registerUser.Id;

            registerUser.Password = Hash.PasswordSHA256(registerUser.Password);
            _logger.LogInformation("CheckUserBeforInsert succeeded for email={Email} userId={UserId}", registerUser.Email, registerUser.Id);
            return registerUser;
        }
        public ConfirmCode CreatedConfirmCode(ConfirmCode user)
        {
            _logger.LogInformation("CreatedConfirmCode started for userId={UserId}", user.UserId);
            var now = _time.UtcNow();
            user.ExpireDate = now.AddMinutes(_emailConfig.ExpireDateMinutes).Ticks;
            return user;
        }
        public async Task<User> InsertUserAfterConfirmEmail(int code)
        {
            _logger.LogInformation("InsertUserAfterConfirmEmail started for code={Code}", code);
            DictionaryList.RegisterUser.TryRemove(code, out var storedUser);

            _userValidation.CodeNotExist(storedUser.code);

            _userValidation.Validation.Throw();

            _userValidation.CodeIsExpire(storedUser.code);

            _userValidation.Validation.Throw();

            storedUser.user.IsEmailConfirm = true;
            storedUser.user.IsDelete = false;

            var query = UserQuery.RegisterNewUser(storedUser.user.Id,
                                                  storedUser.user.CreatedTicks,
                                                  storedUser.user.UpdatedTicks,
                                                  storedUser.user.Name,
                                                  storedUser.user.Description,
                                                  storedUser.user.Email,
                                                  storedUser.user.PhoneNumber,
                                                  storedUser.user.UserType,
                                                  storedUser.user.IsDelete,
                                                  storedUser.user.IsEmailConfirm,
                                                  storedUser.user.Password);
            await _db.DbAsyncAoT.ExecuteAsync(query,
                                                      new()
                                                      {
                                                          [nameof(storedUser.user.Id)] = storedUser.user.Id,
                                                          [nameof(storedUser.user.CreatedTicks)] = storedUser.user.CreatedTicks,
                                                          [nameof(storedUser.user.UpdatedTicks)] = storedUser.user.UpdatedTicks,
                                                          [nameof(storedUser.user.Name)] = storedUser.user.Name,
                                                          [nameof(storedUser.user.Description)] = storedUser.user.Description,
                                                          [nameof(storedUser.user.Email)] = storedUser.user.Email,
                                                          [nameof(storedUser.user.PhoneNumber)] = storedUser.user.PhoneNumber,
                                                          [nameof(storedUser.user.UserType)] = storedUser.user.UserType,
                                                          [nameof(storedUser.user.IsDelete)] = storedUser.user.IsDelete,
                                                          [nameof(storedUser.user.IsEmailConfirm)] = storedUser.user.IsEmailConfirm,
                                                          [nameof(storedUser.user.Password)] = storedUser.user.Password
                                                      });

            var email = storedUser.user;

            _userValidation.AccountNotFound(email);

            _userValidation.Validation.Throw();

            _logger.LogInformation("InsertUserAfterConfirmEmail succeeded for code={Code} userId={UserId}", code, email.Id);

            email.Password = ""; // Clear the password before returning the user object

            return email;
        }
    }
}
