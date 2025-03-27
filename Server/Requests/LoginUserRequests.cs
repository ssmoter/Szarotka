using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Service;
using Server.Validation;

namespace Server.Requests
{
    public interface ILoginUserRequests
    {
        Task<IResult> GetPublicUser(string id, CancellationToken token = default);
        Task<IResult> LogInUser(LoginUser user, CancellationToken token = default);
        Task<IResult> LogOutUser(string user, CancellationToken token = default);
        Task<IResult> RefreshToken(string userToken, CancellationToken token = default);
    }

    public class LoginUserRequests : ILoginUserRequests
    {
        private readonly IAccessDataBase _db;
        private readonly ILoginService _loginService;
        private readonly IEmailConfirmService _emailConfirmService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserValidation _userValidation;

        public LoginUserRequests(IAccessDataBase db,
                                 ILoginService loginService,
                                 IEmailConfirmService emailConfirmService,
                                 IAuthenticationService authenticationService,
                                 IUserValidation userValidation)
        {
            _db = db;
            _loginService = loginService;
            _emailConfirmService = emailConfirmService;
            _authenticationService = authenticationService;
            _userValidation = userValidation;
        }



        public async Task<IResult> LogInUser(LoginUser user, CancellationToken token = default)
        {
            try
            {
                _userValidation.LoginIsNull(user);

                _userValidation.Validation.Throw();

                _userValidation.EmailIsNull(user.Email);
                _userValidation.PasswordIsNull(user.Password);



                _userValidation.Validation.Throw();

                token.ThrowIfCancellationRequested();

                var dbUser = await _loginService.LogIn(user);
                _userValidation.AccountWasDelete(dbUser);

                if (_userValidation.AccountEmailIsNotConfirm(dbUser) == Model.ServerEnums.Result.Error)
                {
                    await _emailConfirmService.SendVerificationEmailCode(dbUser, token);
                }


                _userValidation.Validation.Throw();


                var userToken = await _authenticationService.AuthenticateAsync(dbUser);

                return Results.Ok(new User() { Token = userToken.Token });
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

        public async Task<IResult> RefreshToken(string userToken, CancellationToken token = default)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(userToken, nameof(userToken));
                token.ThrowIfCancellationRequested();
                var newToken = await _authenticationService.AuthenticateAsync(userToken);

                return Results.Ok(newToken);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
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

        public async Task<IResult> LogOutUser(string user, CancellationToken token = default)
        {
            try
            {
                await Task.Delay(1);
                token.ThrowIfCancellationRequested();
                return Results.Ok();
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

        public async Task<IResult> GetPublicUser(string id, CancellationToken token = default)
        {
            try
            {
                ArgumentNullException.ThrowIfNullOrWhiteSpace(id, nameof(id));
                token.ThrowIfCancellationRequested();

                User user = await _loginService.GetPublicUser(id);

                return Results.Ok(user);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
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
