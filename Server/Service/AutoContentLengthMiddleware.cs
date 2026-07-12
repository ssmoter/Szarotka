namespace Server.Service
{
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;

    public class AutoContentLengthMiddleware(RequestDelegate next, ILogger<AutoContentLengthMiddleware>? logger = null)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<AutoContentLengthMiddleware> _logger = logger ?? NullLogger<AutoContentLengthMiddleware>.Instance;

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("AutoContentLengthMiddleware InvokeAsync started for path={Path}", context.Request.Path);
            var originalBody = context.Response.Body;
            var memoryStream = new MemoryStream(); // NIE używaj `using`

            context.Response.Body = memoryStream;

            try
            {
                await _next(context);

                if (!context.Response.HasStarted)
                {
                    context.Response.ContentLength = memoryStream.Length;
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBody);
            }
            finally
            {
                context.Response.Body = originalBody;
                memoryStream.Dispose(); // ręczne zamknięcie po zakończeniu
                _logger.LogInformation("AutoContentLengthMiddleware InvokeAsync completed for path={Path}", context.Request.Path);
            }
        }
    }

}
