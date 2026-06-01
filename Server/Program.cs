using DataBase.Model.JsonContext;

using Microsoft.AspNetCore.Http.Features;

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

app.UseMiddleware<AutoContentLengthMiddleware>();

UserEndpoints.MapEndpoints(app);
InventoryEndpoints.MapEndpoints(app);
DriverRoutesEndpoints.MapEndpoints(app);
UpdateLogEndpoints.MapEndpoints(app);





var sampleTodos = new Todo[] {
    new(1, "Walk the dog"),
    new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
    new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
    new(4, "Clean the bathroom"),
    new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
};

var todosApi = app.MapGroup("/todos");
//todosApi.MapGet("/", () => new DataBase.Data.AccessDataBase().SaveLog(new Exception("test")));
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        //? Results.Ok(todo)
        //? throw new DataBase.Model.EntitiesServer.ErrorException("error", "message")
        ? throw new ArgumentNullException("test")
        : Results.NotFound());

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{

}
