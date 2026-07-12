using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Model;
using Server.Service;
using Server.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Server.Requests
{
    public interface IRegisterUserRequests
    {
        Task<IResult> ConfirmEmail(int code, CancellationToken token = default);
        Task<IResult> InsertUser(RegisterUser registerUser, CancellationToken token = default);
    }


    public class RegisterUserRequests(IAccessDataBase db
                                    , IRegisterUserService register
                                    , IUserValidation userValidation
                                    , IEmailService emailService
                                    , IEmailConfirmService emailConfirmService
                                    , ITimeService time
                                    , ILogger<RegisterUserRequests>? logger = null) : IRegisterUserRequests
    {
        private readonly IAccessDataBase _db = db;
        private readonly IRegisterUserService _registerService = register;
        private readonly IUserValidation _userValidation = userValidation;
        private readonly IEmailService _emailService = emailService;
        private readonly IEmailConfirmService _emailConfirmService = emailConfirmService;
        private readonly ITimeService _time = time;
        private readonly ILogger<RegisterUserRequests> _logger = logger ?? NullLogger<RegisterUserRequests>.Instance;

        public async Task<IResult> InsertUser(RegisterUser registerUser, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("InsertUser started for email={Email}", registerUser?.Email);

                if (_userValidation.RegisterUserNull(registerUser) == ServerEnums.Result.Error)
                {
                    _userValidation.Validation.Throw();
                }
                if (_userValidation.EmailIsNull(registerUser!.Email) == ServerEnums.Result.Success)
                {
                    _userValidation.EmailValidFormat(registerUser.Email);
                    await _userValidation.EmailExist(registerUser.Email);
                }
                if (_userValidation.PasswordIsNull(registerUser.Password) == ServerEnums.Result.Error)
                {
                    var password = registerUser.Password;
                    _userValidation.PasswordLength8(password);
                    _userValidation.PasswordNoUpper(password);
                    _userValidation.PasswordNoLower(password);
                    _userValidation.PasswordNoDigit(password);
                    _userValidation.PasswordNoSpecial(password);
                    _userValidation.PasswordContainEmail(password, registerUser.Email);
                }
                _userValidation.NameRequired(registerUser.Name);

                _userValidation.Validation.Throw();

                token.ThrowIfCancellationRequested();
                RegisterUser result = await _registerService.InsertNewUser(registerUser);

                await _emailConfirmService.SendVerificationEmailCode(result, token);

                _logger.LogInformation("InsertUser succeeded for email={Email} userId={UserId}", registerUser.Email, result.Id);
                return Results.Ok();
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in InsertUser: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in InsertUser");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in InsertUser");
                throw;
            }

        }
        public async Task<IResult> ConfirmEmail(int code, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("ConfirmEmail started for code={Code}", code);
                token.ThrowIfCancellationRequested();
                var user = await _registerService.GetUserEmailFromCodeAndRemoveOld(code);
                _logger.LogInformation("ConfirmEmail succeeded for code={Code} userId={UserId}", code, user?.Id);
                return Results.Ok(user);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed in ConfirmEmail: {Error}", ex.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in ConfirmEmail");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ConfirmEmail");
                throw;
            }
        }


    }
}
