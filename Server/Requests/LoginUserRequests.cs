using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Microsoft.Extensions.Logging.Abstractions;

using Server.Service;
using Server.Validation;

namespace Server.Requests
{
    public interface ILoginUserRequests
    {
        Task<IResult> GetPublicUser(string id, CancellationToken token = default);
        Task<IResult> LogInUser(LoginUser user, CancellationToken token = default);
        Task<IResult> LogOutUser(string refreshToken, CancellationToken token = default);
        Task<IResult> NewAccessToken(string userToken, CancellationToken token = default);
        Task<IResult> NewRefreshToken(string userToken, CancellationToken token = default);
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

                dbUser = await _authenticationService.AuthenticateAsyncAccess(dbUser);
                dbUser = await _authenticationService.AuthenticateAsyncRefresh(dbUser);

                _logger.LogInformation("LogInUser completed for email={Email}", user?.Email);
                return Results.Ok(dbUser);
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

        public async Task<IResult> NewAccessToken(string userToken, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("NewAccessToken invoked.");

                ArgumentException.ThrowIfNullOrWhiteSpace(userToken, nameof(userToken));
                token.ThrowIfCancellationRequested();

                var maskedToken = userToken.Length > 8 ? $"{userToken[..4]}...{userToken[^4..]}" : userToken;
                _logger.LogDebug("Looking up refresh token for token={Token}", maskedToken);

                string userIdFromToken = $"SELECT * FROM {nameof(RefreshToken)} WHERE {nameof(RefreshToken.Value)} = @Value";
                var userIds = await _db.DbAsyncAoT.QueryAsync<RefreshToken>(userIdFromToken, new() { ["Value"] = userToken });
                _logger.LogDebug("Refresh token query returned {Count} rows for token={Token}", userIds?.Count() ?? 0, maskedToken);

                var userId = userIds!.FirstOrDefault();
                if (userId is null)
                {
                    _logger.LogWarning("NewAccessToken failed: Refresh token not found for token={Token}", maskedToken);
                    return Results.Unauthorized();
                }
                if (userId.ExpireDate < _db.TimeService.UtcNow().Ticks)
                {
                    _logger.LogWarning("NewAccessToken failed: Refresh token expired for token={Token}", maskedToken);
                    return Results.Unauthorized();
                }

                _logger.LogDebug("Refresh token valid for UserId={UserId}", userId.UserId);
                var user = await _loginService.GetPublicUser(userId.UserId);

                var newToken = await _authenticationService.AuthenticateAsyncAccess(user);

                _logger.LogInformation("New access token issued for UserId={UserId}", userId.UserId);
                return Results.Ok(newToken.AccessToken);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "NewAccessToken unauthorized access.");
                return Results.Unauthorized();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogInformation(ex, "NewAccessToken operation canceled.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NewAccessToken failed with exception.");
                _db.SaveLog(ex);
                throw;
            }
        }

        public async Task<IResult> NewRefreshToken(string userToken, CancellationToken token = default)
        {
            try
            {
                var tokenPreview = string.IsNullOrWhiteSpace(userToken) ? "<null>" : (userToken.Length > 8 ? userToken[..8] + "..." : userToken);
                _logger.LogDebug("NewRefreshToken called. TokenPreview={TokenPreview}", tokenPreview);

                ArgumentException.ThrowIfNullOrWhiteSpace(userToken, nameof(userToken));
                token.ThrowIfCancellationRequested();

                string userIdFromToken = $"SELECT * FROM {nameof(RefreshToken)} WHERE {nameof(RefreshToken.Value)} = @Value";
                var userIds = await _db.DbAsyncAoT.QueryAsync<RefreshToken>(userIdFromToken, new() { ["Value"] = userToken });
                _logger.LogDebug("Query for refresh token returned {Count} items", userIds?.Count() ?? 0);

                var userId = userIds!.FirstOrDefault();
                if (userId is null)
                {
                    _logger.LogWarning("NewRefreshToken failed: Refresh token not found for tokenPreview={TokenPreview}", tokenPreview);
                    return Results.Unauthorized();
                }
                if (userId.ExpireDate < _db.TimeService.UtcNow().Ticks)
                {
                    _logger.LogWarning("NewRefreshToken failed: Refresh token expired for userId={UserId}", userId.UserId);
                    return Results.Unauthorized();
                }

                _logger.LogInformation("Invalidating existing refresh token for userId={UserId}", userId.UserId);
                await _loginService.LogOut(userToken);

                var user = await _loginService.GetPublicUser(userId.UserId);
                if (user is null)
                {
                    _logger.LogWarning("NewRefreshToken failed: Public user not found for userId={UserId}", userId.UserId);
                    return Results.Unauthorized();
                }

                var newToken = await _authenticationService.AuthenticateAsyncRefresh(user);
                _logger.LogInformation("New refresh token issued for userId={UserId}", userId.UserId);

                return Results.Ok(newToken.RefreshToken);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "NewRefreshToken unauthorized access");
                return Results.Unauthorized();
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogInformation(ex, "Operation canceled during NewRefreshToken");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in NewRefreshToken");
                _db.SaveLog(ex);
                throw;
            }
        }


        public async Task<IResult> LogOutUser(string refreshToken, CancellationToken token = default)
        {
            try
            {
                _logger.LogInformation("LogOutUser started for token={Token}", refreshToken);
                token.ThrowIfCancellationRequested();

                await _loginService.LogOut(refreshToken);

                _logger.LogInformation("LogOutUser completed for token={Token}", refreshToken);
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
