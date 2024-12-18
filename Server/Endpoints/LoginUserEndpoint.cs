using DataBase.Data;
using DataBase.Model.EntitiesServer;

using Server.Model;

using Server.Service;

namespace Server.Endpoints
{
    public interface ILoginUserEndpoint
    {
        Task<IResult> LogInUser(LoginUser user);
        Task<IResult> LogOutUser(string user);
    }

    public class LoginUserEndpoint : ILoginUserEndpoint
    {
        private readonly AccessDataBase _db;
        private readonly ILoginService _loginService;
        private readonly IEmailService _emailService;
        private readonly IEmailConfirmService _emailConfirmService;
        public LoginUserEndpoint(AccessDataBase db, ILoginService loginService, IEmailService emailService, IEmailConfirmService emailConfirmService)
        {
            _db = db;
            _loginService = loginService;
            _emailService = emailService;
            _emailConfirmService = emailConfirmService;
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




                return Results.Ok("token");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        public async Task<IResult> LogOutUser(string user)
        {
            return Results.Ok();
        }


    }
}
