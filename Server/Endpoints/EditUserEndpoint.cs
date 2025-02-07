using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;
using Server.Service;
using Server.Validation;
namespace Server.Endpoints
{
    public interface IEditUserEndpoint
    {
        Task<IResult> Update(EditUser edit);
        Task<IResult> UpdateDescription(EditUser edit);
        Task<IResult> UpdateEmail(EditUser edit);
        Task<IResult> UpdateName(EditUser edit);
        Task<IResult> UpdatePhoneNumber(EditUser edit);
        Task<IResult> UpdateUserType(EditUser edit);
    }

    public class EditUserEndpoint : IEditUserEndpoint
    {
        private readonly IAccessDataBase _db;
        private readonly IUserValidation _userValidation;
        private readonly IEditUserService _editUser;
        private readonly IAuthenticationService _authenticationService;

        public EditUserEndpoint(IAccessDataBase db,
                                IUserValidation userValidation,
                                IEditUserService editUser,
                                IAuthenticationService authenticationService)
        {
            _db = db;
            _userValidation = userValidation;
            _editUser = editUser;
            _authenticationService = authenticationService;
        }


        public async Task<IResult> Update(EditUser edit)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(edit);
                var tasks = new List<Task>
                {
                    UpdateDescriptionTask(edit),
                    UpdateEmailTask(edit),
                    UpdateNameTask(edit),
                    UpdatePhoneNumberTask(edit),
                    UpdateUserTypeTask(edit)
                };

                if (_userValidation.Validation.Count == 0)
                {
                    await Task.WhenAll(tasks);
                    var token = await _authenticationService.AuthenticateAsync(edit.New);
                    return Results.Ok(token);
                }
                else
                {
                    throw _userValidation.Validation;
                }
            }
            catch (ValidationException)
            {
                Console.WriteLine(_userValidation.Validation.GetError());
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult> UpdateDescription(EditUser edit)
        {
            try
            {

                if (_userValidation.Validation.Count == 0)
                {
                    await UpdateDescriptionTask(edit);
                    return Results.Ok();
                }
                else
                {
                    throw _userValidation.Validation;
                }
            }
            catch (ValidationException)
            {
                Console.WriteLine(_userValidation.Validation.GetError());
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IResult> UpdateName(EditUser edit)
        {
            try
            {

                if (_userValidation.Validation.Count == 0)
                {
                    await UpdateNameTask(edit);
                    return Results.Ok();
                }
                else
                {
                    throw _userValidation.Validation;
                }
            }
            catch (ValidationException)
            {
                Console.WriteLine(_userValidation.Validation.GetError());
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IResult> UpdateEmail(EditUser edit)
        {
            try
            {

                if (_userValidation.Validation.Count == 0)
                {
                    await UpdateEmailTask(edit);
                    return Results.Ok();
                }
                else
                {
                    throw _userValidation.Validation;
                }
            }
            catch (ValidationException)
            {
                Console.WriteLine(_userValidation.Validation.GetError());
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IResult> UpdatePhoneNumber(EditUser edit)
        {
            try
            {

                if (_userValidation.Validation.Count == 0)
                {
                    await UpdatePhoneNumberTask(edit);
                    return Results.Ok();
                }
                else
                {
                    throw _userValidation.Validation;
                }
            }
            catch (ValidationException)
            {
                Console.WriteLine(_userValidation.Validation.GetError());
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IResult> UpdateUserType(EditUser edit)
        {
            try
            {
                if (_userValidation.Validation.Count == 0)
                {
                    await UpdateUserTypeTask(edit);
                    return Results.Ok();
                }
                else
                {
                    throw _userValidation.Validation;
                }
            }
            catch (ValidationException)
            {
                Console.WriteLine(_userValidation.Validation.GetError());
                throw;
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        private async Task UpdateDescriptionTask(EditUser edit)
        {
            ArgumentNullException.ThrowIfNull(edit);
            if (edit.New.Description != edit.Old.Description)
            {
                await _editUser.UpdateDescription(edit.New);
            }
        }
        private async Task UpdateNameTask(EditUser edit)
        {
            ArgumentNullException.ThrowIfNull(edit);
            var result = ServerEnums.Result.Error;
            if (edit.New.Name != edit.Old.Name)
            {
                result = _userValidation.NameRequired(edit.New.Name);
            }
            if (result == ServerEnums.Result.Success)
            {
                await _editUser.UpdateName(edit.New);
            }
        }
        private async Task UpdateEmailTask(EditUser edit)
        {
            ArgumentNullException.ThrowIfNull(edit);

            var result = ServerEnums.Result.Error;

            if (edit.New.Email != edit.Old.Email)
            {
                if (_userValidation.EmailIsNull(edit.New.Email) == ServerEnums.Result.Success)
                {
                    result = _userValidation.EmailValidFormat(edit.New.Email);
                    result = await _userValidation.EmailExist(edit.New.Email);
                }
            }
            if (result == ServerEnums.Result.Success)
            {
                await _editUser.UpdateEmail(edit.New);
            }
        }
        private async Task UpdatePhoneNumberTask(EditUser edit)
        {
            ArgumentNullException.ThrowIfNull(edit);
            var result = ServerEnums.Result.Error;
            if (edit.New.PhoneNumber != edit.Old.PhoneNumber)
            {
                result = _userValidation.PhoneNumberRequired(edit.New.PhoneNumber);
                result = _userValidation.PhoneNumberRequired(edit.New.PhoneNumber);
                result = _userValidation.PhoneNumberPattern(edit.New.PhoneNumber);
            }
            if (result == ServerEnums.Result.Success)
            {
                await _editUser.UpdatePhoneNumber(edit.New);
            }
        }
        private async Task UpdateUserTypeTask(EditUser edit)
        {
            ArgumentNullException.ThrowIfNull(edit);
            if (edit.New.UserType != edit.Old.UserType)
            {
                await _editUser.UpdateUserType(edit.New);
            }
        }
    }
}
