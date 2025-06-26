namespace Shared.Data.ServerHttpClients
{
    public static class HttpClientExtension
    {

        public static async Task<(HttpResponseMessage HttpResponseMessage, string content)> DownloadAsync(this HttpClient client, string requestUri, ProgressBar progress = null, CancellationToken cancellationToken = default)
        {
            using var memory = new MemoryStream();
            var result = await client.DownloadAsync(requestUri, memory, progress, cancellationToken);

            memory.Position = 0;
            using var sr = new StreamReader(memory);
            var content = sr.ReadToEnd();
            memory.Position = 0;
            return (result, content);
        }

        public static async Task<HttpResponseMessage> DownloadAsync(this HttpClient client, string requestUri, Stream destination, ProgressBar progress = null, CancellationToken cancellationToken = default)
        {
            using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();
            var contentLength = response.Content.Headers.ContentLength ?? 1;

            using var download = await response.Content.ReadAsStreamAsync(cancellationToken);

            var buffer = new byte[81920];
            long totalRead = 0;
            int read;
            while ((read = await download.ReadAsync(buffer, cancellationToken)) > 0)
            {
#if DEBUG
                await Task.Delay(TimeSpan.FromSeconds(1));
#endif
                await destination.WriteAsync(buffer, 0, read, cancellationToken);
                totalRead += read;
                UpdateProgress(progress, (double)totalRead / contentLength);
            }
            UpdateProgress(progress, 1);
            return response;
        }

        private static void UpdateProgress(ProgressBar bar, double progress)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                if (bar is not null)
                {
                    //bar.Progress = progress;                    
                    await bar.ProgressTo(progress, 500, Easing.Linear);
                }
            });
        }

    }
}
