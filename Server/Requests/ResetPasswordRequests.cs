using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;
using Server.Service;
using Server.Validation;


namespace Server.Requests
{
    public interface IResetPasswordRequests
    {
        Task<IResult> ResetPasswordCode(int code, CancellationToken token = default);
        Task<IResult> ResetPasswordEmail(string email, CancellationToken token = default);
        Task<IResult> ResetPasswordNew(int code, string password, CancellationToken token = default);
    }

    public class ResetPasswordRequests : IResetPasswordRequests
    {
        private readonly IAccessDataBase _db;
        private readonly IUserValidation _userValidation;
        private readonly IEmailConfirmService _emailConfirmService;
        private readonly IResetPasswordService _resetPasswordService;

        public ResetPasswordRequests(IAccessDataBase db, IUserValidation userValidation, IResetPasswordService resetPasswordService, IEmailConfirmService emailConfirmService)
        {
            _db = db;
            _userValidation = userValidation;
            _resetPasswordService = resetPasswordService;
            _emailConfirmService = emailConfirmService;
        }

        public async Task<IResult> ResetPasswordEmail(string email, CancellationToken token = default)
        {
            try
            {
                if (_userValidation.EmailIsNull(email) == ServerEnums.Result.Success)
                {
                    _userValidation.EmailValidFormat(email);
                }

                _userValidation.Validation.Throw();

                var id = await _resetPasswordService.GetUserIdFromEmail(email);

                token.ThrowIfCancellationRequested();

                await _emailConfirmService.SendResetPasswordEmailCode(id, token);

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
        public async Task<IResult> ResetPasswordCode(int code, CancellationToken token = default)
        {
            try
            {
                token.ThrowIfCancellationRequested();

                _ = await CheckCode(code);

                return Results.Ok();
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
        public async Task<IResult> ResetPasswordNew(int code, string password, CancellationToken token = default)
        {
            try
            {
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
