using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;
using Server.Service;
using Server.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Server.Requests
{
    public interface IEditUserRequests
    {
        Task<IResult> Update(EditUser edit, HttpContext context, CancellationToken token = default);
        Task<IResult> UpdateDescription(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdateEmail(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdateName(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdatePhoneNumber(EditUser edit, CancellationToken token = default);
        Task<IResult> UpdateUserType(EditUser edit, CancellationToken token = default);
    }

    public class EditUserRequests(IAccessDataBase db,
                            IUserValidation userValidation,
                            IEditUserService editUser,
                            IAuthenticationService authenticationService,
                            ILogger<EditUserRequests>? logger = null) : IEditUserRequests
    {
        private readonly IAccessDataBase _db = db;
        private readonly IUserValidation _userValidation = userValidation;
        private readonly IEditUserService _editUser = editUser;
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly ILogger<EditUserRequests> _logger = logger ?? NullLogger<EditUserRequests>.Instance;

        public async Task<IResult> Update(EditUser edit, HttpContext context, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("Update started for user edit request");
                ArgumentNullException.ThrowIfNull(edit);
                var tasks = new List<Task>
                {
                    UpdateDescriptionTask(edit),
                    UpdateEmailTask(edit),
                    UpdateNameTask(edit),
                    UpdatePhoneNumberTask(edit),
                    UpdateUserTypeTask(edit)
                };

                token.ThrowIfCancellationRequested();
                await Task.WhenAll(tasks);

                _userValidation.Validation.Throw();

                var userToken = await CreatedNewToken(edit.New.Id.ToString());
                return Results.Ok(userToken);
            }
            catch (ValidationException)
            {
                var userToken = await CreatedNewToken(edit.New.Id.ToString());
                _logger.LogWarning("Validation failed in Update: {Error}", _userValidation.Validation.GetError());
                _userValidation.AddToken(userToken.Token);
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in Update");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in Update");
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult> UpdateDescription(EditUser edit, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("UpdateDescription started for userId={UserId}", edit?.New?.Id);
                token.ThrowIfCancellationRequested();
                _userValidation.Validation.Throw();
                await UpdateDescriptionTask(edit);
                var userToken = await CreatedNewToken(edit.New.Id.ToString());
                return Results.Ok(userToken);
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in UpdateDescription: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in UpdateDescription");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UpdateDescription");
                throw;
            }
        }
        public async Task<IResult> UpdateName(EditUser edit, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("UpdateName started for userId={UserId}", edit?.New?.Id);
                token.ThrowIfCancellationRequested();
                await UpdateNameTask(edit);
                _userValidation.Validation.Throw();
                var userToken = await CreatedNewToken(edit.New.Id.ToString());
                return Results.Ok(userToken);
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in UpdateName: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in UpdateName");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UpdateName");
                throw;
            }
        }
        public async Task<IResult> UpdateEmail(EditUser edit, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("UpdateEmail started for userId={UserId}", edit?.New?.Id);
                token.ThrowIfCancellationRequested();
                await UpdateEmailTask(edit);
                _userValidation.Validation.Throw();
                var userToken = await CreatedNewToken(edit.New.Id.ToString());
                return Results.Ok(userToken);
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in UpdateEmail: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in UpdateEmail");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UpdateEmail");
                throw;
            }
        }
        public async Task<IResult> UpdatePhoneNumber(EditUser edit, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("UpdatePhoneNumber started for userId={UserId}", edit?.New?.Id);
                token.ThrowIfCancellationRequested();
                await UpdatePhoneNumberTask(edit);
                _userValidation.Validation.Throw();
                var userToken = await CreatedNewToken(edit.New.Id.ToString());
                return Results.Ok(userToken);
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in UpdatePhoneNumber: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in UpdatePhoneNumber");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UpdatePhoneNumber");
                throw;
            }
        }
        public async Task<IResult> UpdateUserType(EditUser edit, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("UpdateUserType started for userId={UserId}", edit?.New?.Id);
                token.ThrowIfCancellationRequested();
                await UpdateUserTypeTask(edit);
                _userValidation.Validation.Throw();
                var userToken = await CreatedNewToken(edit.New.Id.ToString());
                return Results.Ok(userToken);
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in UpdateUserType: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in UpdateUserType");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in UpdateUserType");
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
            var users = await _db.DataBaseAsync.QueryAsync<User>(sql, id);

            User updateUser = users.FirstOrDefault() ?? throw new UnauthorizedAccessException();

            var userToken = await _authenticationService.AuthenticateAsync(updateUser);
            return userToken;
        }
    }
}
