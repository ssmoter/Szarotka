using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;
using Server.Service;
using Server.Validation;

namespace Server.Endpoints
{
    public interface IRegisterUserEndpoint
    {
        Task<IResult> ConfirmEmail(int code);
        Task<IResult> InsertUser(RegisterUser registerUser);
    }


    public class RegisterUserEndpoint : IRegisterUserEndpoint
    {
        private readonly AccessDataBase _db;
        private readonly IRegisterUserService _registerService;
        private readonly IUserValidation _userValidation;
        private readonly IEmailService _emailService;
        private readonly IEmailConfirmService _emailConfirmService;

        public RegisterUserEndpoint(AccessDataBase db
                                    , IRegisterUserService register
                                    , IUserValidation userValidation
                                    , IEmailService emailService,
IEmailConfirmService emailConfirmService)
        {
            _db = db;
            _registerService = register;
            _userValidation = userValidation;
            _emailService = emailService;
            _emailConfirmService = emailConfirmService;
        }

        public async Task<IResult> InsertUser(RegisterUser registerUser)
        {
            try
            {
                #region Validation
                var validError = new ValidationException();
                if (registerUser is null)
                {
                    validError.AddError($"{nameof(RegisterUser)} is required", EnumsList.Validation.RegisterUserNull);
                    throw validError;
                }

                var emailIsCorrent = _userValidation.EmailIsCorrent(registerUser);
                if (emailIsCorrent == ServerEnums.Result.Error)
                {
                    validError.AddError("Email is not a valid format", EnumsList.Validation.EmailValidFormat);
                }
                if (emailIsCorrent == ServerEnums.Result.Success)
                {
                    var emailIsExist = await _userValidation.EmailIsExist(registerUser);

                    if (emailIsExist == ServerEnums.Result.Error)
                    {
                        validError.AddError("Email already exists", EnumsList.Validation.EmailExist);
                    }
                }

                _userValidation.PasswordValid(registerUser, ref validError);

                #endregion


                if (validError.Count > 0)
                {
                    throw validError;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            try
            {
                RegisterUser result = await _registerService.InsertNewUser(registerUser);

                await _emailConfirmService.SendVerificationEmailCode(result);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new ErrorException(ex.StackTrace is not null ? ex.StackTrace : "", ex.Message);
            }
        }
        public async Task<IResult> ConfirmEmail(int code)
        {
            try
            {
                if (CountDigits(code) < 5)
                {
                    throw new ArgumentNullException(nameof(code));
                }
                var user = await _registerService.GetUserEmailFromCodeAndRemoveOld(code);

                if (user is null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
                return Results.Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            static int CountDigits(int number)
            {
                if (number == 0) return 1;
                return (int)Math.Log10(Math.Abs(number)) + 1;
            }

        }


    }
}
