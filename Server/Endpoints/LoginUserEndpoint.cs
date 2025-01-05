using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Service;

namespace Server.Endpoints
{
    public interface ILoginUserEndpoint
    {
        Task<IResult> LogInUser(LoginUser user);
        Task<IResult> LogOutUser(string user);
        Task<IResult> RefreshToken(string token);
    }

    public class LoginUserEndpoint : ILoginUserEndpoint
    {
        private readonly AccessDataBase _db;
        private readonly ILoginService _loginService;
        private readonly IEmailConfirmService _emailConfirmService;
        private readonly IAuthenticationService _authenticationService;

        public LoginUserEndpoint(AccessDataBase db, ILoginService loginService, IEmailConfirmService emailConfirmService, IAuthenticationService authenticationService)
        {
            _db = db;
            _loginService = loginService;
            _emailConfirmService = emailConfirmService;
            _authenticationService = authenticationService;
        }



        public async Task<IResult> LogInUser(LoginUser user)
        {
            var valid = new ValidationException();
            try
            {
                if (user is null)
                {
                    valid.AddError("Login is null", EnumsList.Validation.LoginIsNull);
                    throw valid;
                }
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    valid.AddError("Email is null", EnumsList.Validation.EmailIsNull);
                }
                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    valid.AddError("Password is null", EnumsList.Validation.PasswordIsNull);
                }

                if (valid.Count > 0)
                {
                    throw valid;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(valid.GetError());
                Console.WriteLine(ex.Message);
                throw;
            }

            try
            {
                var dbUser = await _loginService.LogIn(user);

                if (dbUser.IsDelete == true)
                {
                    valid.AddError("Account was delete", EnumsList.Validation.AccountWasDelete);
                }
                if (dbUser.IsEmailConfirm == false)
                {
                    valid.AddError("Confirm your email, new code was sent", EnumsList.Validation.AccountEmailIsNotConfirm);
                    await _emailConfirmService.SendVerificationEmailCode(dbUser);
                }

                if (valid.Count > 0)
                {
                    throw valid;
                }

                var token = await _authenticationService.AuthenticateAsync(dbUser);

                return Results.Ok(new User() { Token = token.Token });
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                Console.WriteLine(valid.GetError());
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<IResult> RefreshToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    throw new ArgumentNullException(nameof(token));
                }
                var newToken = await _authenticationService.AuthenticateAsync(token);

                return Results.Ok(newToken);
            }
            catch (Exception ex)
            {
                _db.SaveLog(ex);
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public async Task<IResult> LogOutUser(string user)
        {
            await Task.Delay(1);
            return Results.Ok();
        }


    }
}
