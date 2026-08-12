using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Service;
using Server.Validation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Server.Requests
{
    public interface ILoginUserRequests
    {
        Task<IResult> GetPublicUser(string id, CancellationToken token = default);
        Task<IResult> LogInUser(LoginUser user, CancellationToken token = default);
        Task<IResult> LogOutUser(string user, CancellationToken token = default);
        Task<IResult> RefreshToken(string userToken, CancellationToken token = default);
    }

    public class LoginUserRequests(IAccessDataBaseAoT db,
                             ILoginService loginService,
                             IEmailConfirmService emailConfirmService,
                             IAuthenticationService authenticationService,
                             IUserValidation userValidation,
                             ILogger<LoginUserRequests>? logger = null) : ILoginUserRequests
    {
        private readonly IAccessDataBaseAoT _db = db;
        private readonly ILoginService _loginService = loginService;
        private readonly IEmailConfirmService _emailConfirmService = emailConfirmService;
        private readonly IAuthenticationService _authenticationService = authenticationService;
        private readonly IUserValidation _userValidation = userValidation;
        private readonly ILogger<LoginUserRequests> _logger = logger ?? NullLogger<LoginUserRequests>.Instance;

        public async Task<IResult> LogInUser(LoginUser user, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("LogInUser started for email={Email}", user?.Email);
                _userValidation.LoginIsNull(user);

                _userValidation.Validation.Throw();

                _userValidation.EmailIsNull(user!.Email);
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

                _logger.LogInformation("LogInUser completed for email={Email}", user?.Email);
                return Results.Ok(new User() { Token = userToken.Token });
            }
            catch (ValidationException)
            {
                _logger.LogWarning("Validation failed in LogInUser: {Error}", _userValidation.Validation.GetError());
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in LogInUser");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in LogInUser");
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult> RefreshToken(string userToken, CancellationToken token = default)
        {
            try
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(userToken, nameof(userToken));
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
                _logger.LogInformation("LogOutUser started for user={User}", user);
                await Task.Delay(1);
                token.ThrowIfCancellationRequested();
                _logger.LogInformation("LogOutUser completed for user={User}", user);
                return Results.Ok();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in LogOutUser");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in LogOutUser");
                _db.SaveLog(ex);
                throw;
            }

        }

        public async Task<IResult> GetPublicUser(string id, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("GetPublicUser started for id={Id}", id);
                ArgumentException.ThrowIfNullOrWhiteSpace(id, nameof(id));
                token.ThrowIfCancellationRequested();

                User user = await _loginService.GetPublicUser(id);

                _logger.LogInformation("GetPublicUser succeeded for id={Id}", id);
                return Results.Ok(user);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Unauthorized();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "Operation cancelled in GetPublicUser");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetPublicUser");
                _db.SaveLog(ex);
                throw;
            }
        }
    }
}
