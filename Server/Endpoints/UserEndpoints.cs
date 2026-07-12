using DataBase.Model.EntitiesServer;

using Microsoft.AspNetCore.Mvc;

using Server.Requests;

namespace Server.Endpoints
{

    public static class UserEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {

            var user = app.MapGroup("/user");
            user.MapPost("/register", async ([FromBody] RegisterUser user,
                                             IRegisterUserRequests registerUserEndpoint,
                                             CancellationToken token = default) =>
            {
                return await registerUserEndpoint.InsertUser(user, token);
            });
            user.MapGet("/confirm-email/{code}", async (int code, IRegisterUserRequests registerUserEndpoint, CancellationToken token = default) =>
            {
                return await registerUserEndpoint.ConfirmEmail(code, token);
            });
            user.MapPost("login", async ([FromBody] LoginUser user, HttpContext context, ILoginUserRequests loginUserRequests, CancellationToken token = default) =>
            {
                return await loginUserRequests.LogInUser(user, token);
            });
            user.MapGet("logout", ( /*ILoginUserEndpoint loginUserEndpoint*/) =>
            {
                throw new NotImplementedException();
                //return await loginUserEndpoint.LogOutUser("user");
            }).RequireAuthorization();
            user.MapGet("refresh-token", async (HttpContext context, ILoginUserRequests loginUserRequests, CancellationToken token = default)
                =>
            {
                // Pobierz wartość nagłówka Authorization
                var authorizationHeader = context.Request.Headers.Authorization.ToString();
                if (string.IsNullOrEmpty(authorizationHeader))
                {
                    return Results.Unauthorized();
                }
                // Sprawdź, czy nagłówek zaczyna się od "Bearer "
                if (!authorizationHeader.StartsWith("Bearer "))
                {
                    return Results.Unauthorized();
                }
                // Pobierz token
                var userToken = DataBase.Helper.ReadToken.RemoveBearer(authorizationHeader);
                if (userToken is null)
                {
                    return Results.Unauthorized();
                }
                return await loginUserRequests.RefreshToken(userToken, token);
            }).RequireAuthorization();
            user.MapPost("edit", async ([FromBody] User user, IEditUserRequests editUserRequests, HttpContext context, CancellationToken token = default) =>
            {
                var authorizationHeader = context.Request.Headers.Authorization.ToString();
                var userToken = DataBase.Helper.ReadToken.RemoveBearer(authorizationHeader);
                var tokenModel = DataBase.Helper.ReadToken.GetUserFromToken(userToken);

                user.Id = new Guid(tokenModel.user.Id.ToByteArray());
                user.UserCreatedId = new Guid(tokenModel.user.Id.ToByteArray());
                user.UserUpdatedId = new Guid(tokenModel.user.Id.ToByteArray());

                tokenModel.user.UserCreatedId = new Guid(tokenModel.user.Id.ToByteArray());
                tokenModel.user.UserUpdatedId = new Guid(tokenModel.user.Id.ToByteArray());

                var editUser = new EditUser()
                {
                    New = user,
                    Old = tokenModel.user,
                };
                return await editUserRequests.Update(editUser, context, token);
            }).RequireAuthorization();
            user.MapGet("/{id}", async (string id, ILoginUserRequests loginUserRequests, CancellationToken token = default) =>
            {
                return await loginUserRequests.GetPublicUser(id, token);
            }).RequireAuthorization();

            user.MapGet("reset-password-email/{email}", async (string email, IResetPasswordRequests resetPasswordRequests, CancellationToken token = default) =>
            {
                return await resetPasswordRequests.ResetPasswordEmail(email, token);
            });
            user.MapGet("reset-password/{code}", async (int code, IResetPasswordRequests resetPasswordRequests, CancellationToken token = default) =>
            {
                return await resetPasswordRequests.ResetPasswordCode(code, token);
            });
            user.MapGet("reset-password/{code}/{password}", async (int code, string password, IResetPasswordRequests resetPasswordRequests, CancellationToken token = default) =>
            {
                return await resetPasswordRequests.ResetPasswordNew(code, password, token);
            });
        }







    }
}
