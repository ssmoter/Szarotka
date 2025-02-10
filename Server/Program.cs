using DataBase.Model.EntitiesServer;
using DataBase.Model.JsonContext;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using Server.Endpoints;
using Server.Service;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions((Action<Microsoft.AspNetCore.Http.Json.JsonOptions>)(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(SzarotkaJsonSerializerContext.Default);
}));

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
        $"{context.HttpContext.Request.Method} {context.HttpContext.TraceIdentifier}";
        context.ProblemDetails.Extensions.TryAdd("requestid", context.HttpContext.TraceIdentifier);
        var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

builder.Services.AddExceptionHandler<Server.Handler.ProblemExceptionHandler>();
builder.Services.AddMyServiceServer(builder.Configuration);
builder.Services.AddSecurityServicesServer(builder.Configuration);

builder.Services.AddAuthorization();

#if DEBUG
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
#endif

var app = builder.Build();

#if DEBUG
app.UseCors("AllowAll");
#endif

app.UseExceptionHandler();

app.UseRouting();
app.UseAuthorization();



var user = app.MapGroup("/user");
user.MapPost("/register", async ([FromBody] RegisterUser user, IRegisterUserEndpoint registerUserEndpoint, CancellationToken token = default)
    =>
    {
        return await registerUserEndpoint.InsertUser(user, token);
    });
user.MapGet("/confirm_email/{code}", async (int code, IRegisterUserEndpoint registerUserEndpoint, CancellationToken token = default)
    =>
    {
        return await registerUserEndpoint.ConfirmEmail(code, token);
    });
user.MapPost("login", async ([FromBody] LoginUser user, HttpContext context, ILoginUserEndpoint loginUserEndpoint, CancellationToken token = default)
    =>
    {
        return await loginUserEndpoint.LogInUser(user, token);
    });
user.MapGet("logout", ( /*ILoginUserEndpoint loginUserEndpoint*/)
    =>
{
    throw new NotImplementedException();
    //return await loginUserEndpoint.LogOutUser("user");
}).RequireAuthorization();
user.MapGet("refresh_token", async (HttpContext context, ILoginUserEndpoint loginUserEndpoint, CancellationToken token = default)
    =>
{
    // Pobierz wartość nagłówka Authorization
    var authorizationHeader = context.Request.Headers["Authorization"].ToString();
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
    return await loginUserEndpoint.RefreshToken(userToken, token);
}).RequireAuthorization();
user.MapPost("edit", async ([FromBody] User user, IEditUserEndpoint editUserEndpoint, HttpContext context, CancellationToken token = default)
    =>
{
    var authorizationHeader = context.Request.Headers.Authorization.ToString();
    var userToken = DataBase.Helper.ReadToken.RemoveBearer(authorizationHeader);
    var tokenModel = DataBase.Helper.ReadToken.GetUserFromToken(userToken);

    user.UserUpdatedId = new Guid(tokenModel.user.Id.ToByteArray());
    user.Id = new Guid(tokenModel.user.Id.ToByteArray());
    var editUser = new EditUser()
    {
        New = user,
        Old = tokenModel.user,
    };

    return await editUserEndpoint.Update(editUser, token);
}).RequireAuthorization();













var sampleTodos = new Todo[] {
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
todosApi.RequireAuthorization();
todosApi.MapGet("/", () => new DataBase.Data.AccessDataBase().SaveLog(new Exception("test")));
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        //? throw new Server.Model.ErrorException("error", "message")
        : Results.NotFound());

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{

}
