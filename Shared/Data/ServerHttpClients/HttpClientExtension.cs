using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Data.ServerHttpClients
{
    public static class HttpClientExtension
    {
        // 1. Wariant tekstowy - bezpiecznie zarządza pamięcią RAM i zwalnia MemoryStream
        public static async Task<(HttpResponseMessage HttpResponseMessage, string content)> DownloadAsync(
            this HttpClient client,
            string requestUri,
            Action<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            // Zastosowanie 'using' gwarantuje zwolnienie strumienia pamięci zaraz po konwersji na string
            using var memory = new MemoryStream();
            var result = await client.DownloadAsync(requestUri, memory, progress, cancellationToken);

            if (!result.IsSuccessStatusCode)
            {
                return (result, string.Empty);
            }

            memory.Position = 0;
            using var sr = new StreamReader(memory);
            var content = await sr.ReadToEndAsync(cancellationToken);
            return (result, content);
        }

        // 2. Główny wariant strumieniowy (np. pliki na dysk) - w pełni zintegrowany z handlerem autoryzacji
        public static async Task<HttpResponseMessage> DownloadAsync(
            this HttpClient client,
            string requestUri,
            Stream destination,
            Action<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return response;
            }

            var contentLength = response.Content.Headers.ContentLength ?? -1;

            using (var download = await response.Content.ReadAsStreamAsync(cancellationToken))
            {
                await CheckProgress(destination, progress, contentLength, download, cancellationToken);
            }

            return response;
        }

        public static async Task<string> CheckProgress(
            Action<double> progress,
            long contentLength,
            Stream download,
            CancellationToken cancellationToken = default)
        {
            using var memory = new MemoryStream();
            await CheckProgress(memory, progress, contentLength, download, cancellationToken);

            memory.Position = 0;
            using var sr = new StreamReader(memory);
            var content = await sr.ReadToEndAsync(cancellationToken);
            return content;
        }

        public static async Task CheckProgress(
            Stream destination,
            Action<double> progress,
            long contentLength,
            Stream download,
            CancellationToken cancellationToken = default)
        {
            var buffer = new byte[81920];
            long totalRead = 0;
            int read;

            while ((read = await download.ReadAsync(buffer, cancellationToken)) > 0)
            {
                await destination.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
                totalRead += read;

                if (contentLength > 0)
                {
                    progress?.Invoke((double)totalRead / contentLength);
                }
            }

            progress?.Invoke(1.0);
            await destination.FlushAsync(cancellationToken);
        }
        // 3. Poprawiony POST dla formatu JSON - usunięto problematyczny MemoryStream i ProgressStream
        public static async Task<HttpResponseMessage> PostWithProgressAsync(
            this HttpClient client,
            string requestUri,
            string contentJson,
            Action<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            // 1. Konwertujemy string JSON na tablicę bajtów w pamięci RAM
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(contentJson);

            // 2. Tworzymy MemoryStream na bazie tej tablicy bajtów
            var memoryStream = new MemoryStream(byteArray);

            // 3. Przekazujemy strumień pamięci bezpośrednio do Twojego mechanizmu postępu ProgressStream.
            // NIE owijamy go w instrukcję 'using', ponieważ HttpClient musi nim zarządzać w tle.
            var progressStream = new ProgressStream(memoryStream, byteArray.Length, async progressDouble =>
            {
                progress?.Invoke(progressDouble);
            });

            var content = new StreamContent(progressStream);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = content
            };

            // 4. WYJAŚNIENIE AOT: Dla dużych stringów JSON używamy ResponseHeadersRead, 
            // aby pakiety mogły płynąć strumieniowo bajt po bajcie przez ProgressStream do sieci,
            // wyzwalając poprawnie Twój callback paska postępu.
            return await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

        // 4. POST ze strumieniem (używaj TYLKO do wysyłania dużych plików z dysku!)
        public static async Task<HttpResponseMessage> PostWithProgressAsync(
            this HttpClient client,
            string requestUri,
            Stream contentStream,
            string mediaType,
            Action<double> progress = null,
            CancellationToken cancellationToken = default)
        {
            var totalBytes = contentStream.Length;
            if (contentStream.CanSeek)
            {
                contentStream.Seek(0, SeekOrigin.Begin);
            }

            var progressStream = new ProgressStream(contentStream, totalBytes, progressDouble =>
            {
                progress?.Invoke(progressDouble);
            });

            var content = new StreamContent(progressStream);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);

            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = content,
            };

            // Dla dużych plików pozostawiamy ResponseHeadersRead, ale upewnij się, 
            // ze strumień 'contentStream' nie zostanie zamknięty w kodzie wywołującym przed zakończeniem wysyłania!
            return await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        }

    }
}
