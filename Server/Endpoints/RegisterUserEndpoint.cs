using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.AspNetCore.Mvc;

using Server.Model;
using Server.Validation;

namespace Server.Endpoints
{
    public interface IRegisterUserEndpoint
    {
        Task<IActionResult> InsertUser([FromBody] RegisterUser registerUser);
    }

    [Route("api/v1/[controller]")]
    public class RegisterUserEndpoint : ControllerBase, IRegisterUserEndpoint
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

        [Route("user")]
        [HttpPost]
        public async Task<IActionResult> InsertUser(
            [FromBody] RegisterUser registerUser)
        {
            var validError = new ValidationException();
            if (registerUser is null)
            {
                validError.AddError($"{nameof(RegisterUser)} is required");
                throw validError;
            }

            #region Email Validation
            var emailIsCorrent = _userValidation.EmailIsCorrent(registerUser);
            if (emailIsCorrent == ServerEnums.ValidationResult.Error)
            {
                validError.AddError("Email is not a valid format");
            }
            if (emailIsCorrent == ServerEnums.ValidationResult.Success)
            {
                var emailIsExist = _userValidation.EmailIsExist(registerUser);

                if (emailIsExist == ServerEnums.ValidationResult.Error)
                {
                    validError.AddError("Email already exists");
                }
            }
            #endregion




            if (validError.Count > 0)
            {
                throw validError;
            }

            var result = await _registerService.InsertNewUser(registerUser);
            return result;
        }

    }
}
