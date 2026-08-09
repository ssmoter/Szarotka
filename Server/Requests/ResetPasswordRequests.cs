using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;
using Server.Service;
using Server.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


namespace Server.Requests
{
    public interface IResetPasswordRequests
    {
        Task<IResult> ResetPasswordCode(int code, CancellationToken token = default);
        Task<IResult> ResetPasswordEmail(string email, CancellationToken token = default);
        Task<IResult> ResetPasswordNew(int code, string password, CancellationToken token = default);
    }

    public class ResetPasswordRequests(IAccessDataBaseAoT db, IUserValidation userValidation, IResetPasswordService resetPasswordService, IEmailConfirmService emailConfirmService, ILogger<ResetPasswordRequests>? logger = null) : IResetPasswordRequests
    {
        private readonly IAccessDataBaseAoT _db = db;
        private readonly IUserValidation _userValidation = userValidation;
        private readonly IEmailConfirmService _emailConfirmService = emailConfirmService;
        private readonly IResetPasswordService _resetPasswordService = resetPasswordService;
        private readonly ILogger<ResetPasswordRequests> _logger = logger ?? NullLogger<ResetPasswordRequests>.Instance;

        public async Task<IResult> ResetPasswordEmail(string email, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("ResetPasswordEmail started for email={Email}", email);
                if (_userValidation.EmailIsNull(email) == ServerEnums.Result.Success)
                {
                    _userValidation.EmailValidFormat(email);
                }

                _userValidation.Validation.Throw();

                var id = await _resetPasswordService.GetUserIdFromEmail(email);

                token.ThrowIfCancellationRequested();

                await _emailConfirmService.SendResetPasswordEmailCode(id, token);
                _logger.LogInformation("ResetPasswordEmail: reset code sent for userId={UserId}", id);
                return Results.Ok();
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in ResetPasswordEmail: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in ResetPasswordEmail");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ResetPasswordEmail");
                _db.SaveLog(ex);
                throw;
            }
        }
        public async Task<IResult> ResetPasswordCode(int code, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("ResetPasswordCode started for code={Code}", code);
                token.ThrowIfCancellationRequested();

                _ = await CheckCode(code);

                _logger.LogInformation("ResetPasswordCode succeeded for code={Code}", code);
                return Results.Ok();
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed in ResetPasswordCode: {Error}", ex.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in ResetPasswordCode");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ResetPasswordCode");
                _db.SaveLog(ex);
                throw;
            }
        }
        public async Task<IResult> ResetPasswordNew(int code, string password, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("ResetPasswordNew started for code={Code}", code);
                token.ThrowIfCancellationRequested();

                var confirmCode = await CheckCode(code);

                if (_userValidation.PasswordIsNull(password) == ServerEnums.Result.Success)
                {
                    var _password = password;
                    _userValidation.PasswordLength8(_password);
                    _userValidation.PasswordNoUpper(_password);
                    _userValidation.PasswordNoLower(_password);
                    _userValidation.PasswordNoDigit(_password);
                    _userValidation.PasswordNoSpecial(_password);
                }

                _userValidation.Validation.Throw();

                await _resetPasswordService.ChangePassword(confirmCode.UserId, password);
                _logger.LogInformation("ResetPasswordNew: password changed for userId={UserId}", confirmCode.UserId);

                return Results.Ok();
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in ResetPasswordNew: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in ResetPasswordNew");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in ResetPasswordNew");
                _db.SaveLog(ex);
                throw;
            }
        }





        private async Task<ConfirmCode> CheckCode(int code)
        {
            var confirm = await _resetPasswordService.GetConfirmCode(code);

            if (_userValidation.CodeNotExist(confirm) == ServerEnums.Result.Success)
            {
                _userValidation.CodeIsExpire(confirm!);
            }
            _userValidation.Validation.Throw();
            return confirm!;
        }
    }
}
