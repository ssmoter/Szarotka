using DataBase.Data;
using DataBase.Model.EntitiesServer;
using DataBase.Service;

using Server.Model;
using Server.Service;
using Server.Validation;

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
                                    , ITimeService time) : IRegisterUserRequests
    {
        private readonly IAccessDataBase _db = db;
        private readonly IRegisterUserService _registerService = register;
        private readonly IUserValidation _userValidation = userValidation;
        private readonly IEmailService _emailService = emailService;
        private readonly IEmailConfirmService _emailConfirmService = emailConfirmService;
        private readonly ITimeService _time = time;

        public async Task<IResult> InsertUser(RegisterUser registerUser, CancellationToken token = default)
        {
            try
            {
                #region Validation
                if (_userValidation.RegisterUserNull(registerUser) == ServerEnums.Result.Error)
                {
                    _userValidation.Validation.Throw();
                }
                if (_userValidation.EmailIsNull(registerUser.Email) == ServerEnums.Result.Success)
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
                #endregion

                _userValidation.Validation.Throw();

                token.ThrowIfCancellationRequested();
                RegisterUser result = await _registerService.InsertNewUser(registerUser);

                await _emailConfirmService.SendVerificationEmailCode(result, token);

                return Results.Ok();
            }
            catch (ValidationException)
            {
                Console.WriteLine(_userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }

        }
        public async Task<IResult> ConfirmEmail(int code, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();
                var user = await _registerService.GetUserEmailFromCodeAndRemoveOld(code);
                return Results.Ok(user);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine(ex.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                throw;
            }
        }


    }
}
