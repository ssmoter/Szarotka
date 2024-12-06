using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;
using Server.Validation;

namespace Server.Endpoints
{
    public interface IRegisterUserEndpoint
    {
        Task<IResult> InsertUser(RegisterUser registerUser);
    }


    public class RegisterUserEndpoint : IRegisterUserEndpoint
    {
        private readonly AccessDataBase _db;
        private readonly Service.IRegisterUserService _registerService;
        private readonly IUserValidation _userValidation;

        public RegisterUserEndpoint(AccessDataBase db
                                    , Service.IRegisterUserService register
                                    , IUserValidation userValidation)
        {
            _db = db;
            _registerService = register;
            _userValidation = userValidation;
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
                if (emailIsCorrent == ServerEnums.ValidationResult.Error)
                {
                    validError.AddError("Email is not a valid format", EnumsList.Validation.EmailValidFormat);
                }
                if (emailIsCorrent == ServerEnums.ValidationResult.Success)
                {
                    var emailIsExist = _userValidation.EmailIsExist(registerUser);

                    if (emailIsExist == ServerEnums.ValidationResult.Error)
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

                var result = await _registerService.InsertNewUser(registerUser);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                throw new ErrorException(ex.Source, ex.Message);
            }
        }

    }
}
