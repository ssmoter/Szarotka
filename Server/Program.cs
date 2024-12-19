using DataBase.Model.EntitiesServer;

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

using Server.Endpoints;
using Server.Service;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(UserJsonSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(RegisterUserJsonSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Add(LoginUserJsonSerializerContext.Default);
});

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

var app = builder.Build();

app.UseExceptionHandler();

app.UseRouting();
app.UseAuthorization();


var user = app.MapGroup("/user");
user.MapPost("/register", async ([FromBody] RegisterUser user, IRegisterUserEndpoint registerUserEndpoint)
    =>
    {
        return await registerUserEndpoint.InsertUser(user);
    });
user.MapGet("/confirm_email/{code}", async (int code, IRegisterUserEndpoint registerUserEndpoint)
    =>
    {
        return await registerUserEndpoint.ConfirmEmail(code);
    });
user.MapPost("login", async ([FromBody] LoginUser user, ILoginUserEndpoint loginUserEndpoint)
    =>
    {
        return await loginUserEndpoint.LogInUser(user);
    });
user.MapGet("logout", async (string user, ILoginUserEndpoint loginUserEndpoint)
    =>
{
    return await loginUserEndpoint.LogOutUser(user);
});














var sampleTodos = new Todo[] {
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
todosApi.RequireAuthorization();
todosApi.MapGet("/", () => sampleTodos);
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
