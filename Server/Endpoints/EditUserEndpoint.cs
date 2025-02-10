using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;
using Server.Service;
using Server.Validation;
namespace Server.Endpoints
{
    public interface IEditUserEndpoint
    {
        Task<IResult> Update(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdateDescription(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdateEmail(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdateName(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdatePhoneNumber(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdateUserType(EditUser edit, CancellationToken token = default);
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


        public async Task<IResult> Update(EditUser edit, CancellationToken token = default)
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

                if (_userValidation.Validation.ValidationErrors.Count == 0)
                {
                    token.ThrowIfCancellationRequested();
                    await Task.WhenAll(tasks);
                    var userToken = await CreatedNewToken(edit.New.Id.ToString());
                    return Results.Ok(userToken);
                }
                else
                {
                    throw _userValidation.Validation.Throw();
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

        public async Task<IResult> UpdateDescription(EditUser edit, CancellationToken token = default)
        {
            try
            {
                if (_userValidation.Validation.ValidationErrors.Count == 0)
                {
                    token.ThrowIfCancellationRequested();
                    await UpdateDescriptionTask(edit);
                    var userToken = await CreatedNewToken(edit.New.Id.ToString());
                    return Results.Ok(userToken);
                }
                else
                {
                    throw _userValidation.Validation.Throw();
                }
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
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IResult> UpdateName(EditUser edit, CancellationToken token = default)
        {
            try
            {
                if (_userValidation.Validation.ValidationErrors.Count == 0)
                {
                    token.ThrowIfCancellationRequested();
                    await UpdateNameTask(edit);
                    var userToken = await CreatedNewToken(edit.New.Id.ToString());
                    return Results.Ok(userToken);
                }
                else
                {
                    throw _userValidation.Validation.Throw();
                }
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
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IResult> UpdateEmail(EditUser edit, CancellationToken token = default)
        {
            try
            {

                if (_userValidation.Validation.ValidationErrors.Count == 0)
                {
                    token.ThrowIfCancellationRequested();
                    await UpdateEmailTask(edit);
                    var userToken = await CreatedNewToken(edit.New.Id.ToString());
                    return Results.Ok(userToken);
                }
                else
                {
                    throw _userValidation.Validation.Throw();
                }
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
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IResult> UpdatePhoneNumber(EditUser edit, CancellationToken token = default)
        {
            try
            {

                if (_userValidation.Validation.ValidationErrors.Count == 0)
                {
                    token.ThrowIfCancellationRequested();
                    await UpdatePhoneNumberTask(edit);
                    var userToken = await CreatedNewToken(edit.New.Id.ToString());
                    return Results.Ok(userToken);
                }
                else
                {
                    throw _userValidation.Validation.Throw();
                }
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
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public async Task<IResult> UpdateUserType(EditUser edit, CancellationToken token = default)
        {
            try
            {
                if (_userValidation.Validation.ValidationErrors.Count == 0)
                {
                    token.ThrowIfCancellationRequested();
                    await UpdateUserTypeTask(edit);
                    var userToken = await CreatedNewToken(edit.New.Id.ToString());
                    return Results.Ok(userToken);
                }
                else
                {
                    throw _userValidation.Validation.Throw();
                }
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



        private async Task<User> CreatedNewToken(string id)
        {
            var sql = SqlQuery.LoginQuery.InFromId(id);
            var users = await _db.DataBaseAsync.QueryAsync<User>(sql);

            User updateUser = users.FirstOrDefault() ?? throw new UnauthorizedAccessException();

            var userToken = await _authenticationService.AuthenticateAsync(updateUser);
            return userToken;
        }
    }
}
