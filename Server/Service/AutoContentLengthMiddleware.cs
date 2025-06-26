namespace Server.Service
{
    public class AutoContentLengthMiddleware
    {
        private readonly RequestDelegate _next;

        public AutoContentLengthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBody = context.Response.Body;

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            // Tylko jeśli odpowiedź nie została już zakończona
            if (!context.Response.HasStarted)
            {
                context.Response.ContentLength = memoryStream.Length;
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBody);
            context.Response.Body = originalBody;
        }
    }

}
