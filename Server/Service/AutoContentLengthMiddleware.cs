namespace Server.Service
{
    public class AutoContentLengthMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
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
            }
        }
    }

}
