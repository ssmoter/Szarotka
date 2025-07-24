using System.Net.Http.Headers;

namespace Shared.Data.ServerHttpClients
{
    public static class HttpClientExtension
    {

        public static async Task<(HttpResponseMessage HttpResponseMessage, string content)> DownloadAsync(this HttpClient client, string requestUri, Action<double> progress = null, CancellationToken cancellationToken = default)
        {
            using var memory = new MemoryStream();
            var result = await client.DownloadAsync(requestUri, memory, progress, cancellationToken);

            memory.Position = 0;
            using var sr = new StreamReader(memory);
            var content = await sr.ReadToEndAsync(cancellationToken);
            memory.Position = 0;
            return (result, content);
        }

        public static async Task<HttpResponseMessage> DownloadAsync(this HttpClient client, string requestUri, Stream destination, Action<double> progress = null, CancellationToken cancellationToken = default)
        {
            using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            response.EnsureSuccessStatusCode();
            var contentLength = response.Content.Headers.ContentLength ?? 1;

            using var download = await response.Content.ReadAsStreamAsync(cancellationToken);

            await CheckProgress(destination, progress, contentLength, download, cancellationToken);
            return response;
        }

        public static async Task<string> CheckProgress(Action<double> progress, long contentLength, Stream download, CancellationToken cancellationToken = default)
        {
            var memory = new MemoryStream();
            await CheckProgress(memory, progress, contentLength, download, cancellationToken);

            memory.Position = 0;
            using var sr = new StreamReader(memory);
            var content = await sr.ReadToEndAsync(cancellationToken);
            memory.Position = 0;
            return content;
        }
        public static async Task CheckProgress(Stream destination, Action<double> progress, long contentLength, Stream download, CancellationToken cancellationToken = default)
        {
            var buffer = new byte[81920];
            long totalRead = 0;
            int read;
            while ((read = await download.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await destination.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
                totalRead += read;
                progress?.Invoke((double)totalRead / contentLength);
            }
            progress?.Invoke(1);
            await destination.FlushAsync(cancellationToken);
            destination.Position = 0;
        }


















        public static async Task<HttpResponseMessage> PostWithProgressAsync(
            this HttpClient client,
            string requestUri,
            string contentJson,
            Action<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            using var memory = new MemoryStream();
            using var write = new StreamWriter(memory);
            await write.WriteAsync(contentJson);
            await write.FlushAsync(cancellationToken);
            memory.Position = 0;
            return await client.PostWithProgressAsync(requestUri
                , memory
                , "application/json"
                , progress, cancellationToken);
        }



        public static async Task<HttpResponseMessage> PostWithProgressAsync(
            this HttpClient client,
            string requestUri,
            Stream contentStream,
            string mediaType,
            Action<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            var totalBytes = contentStream.Length;
            using var progressStream = new ProgressStream(contentStream, totalBytes, progressDouble =>
             {
                 progress?.Invoke(progressDouble);
             });
            var content = new StreamContent(progressStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = content,
            };
            progressStream.Seek(0, SeekOrigin.Begin);
            return await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }


    }
}
