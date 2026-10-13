using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Data.ServerHttpClients
{
    public partial class ProgressStream(Stream inner, long totalBytes, Action<double> reportProgress) : Stream
    {
        private readonly Stream _inner = inner;
        private readonly long _totalBytes = totalBytes;
        private readonly Action<double> _reportProgress = reportProgress;
        private long _bytesSent;

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = _inner.Read(buffer, offset, count);
            ReportProgress(bytesRead);
            return bytesRead;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int bytesRead = await _inner.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
            ReportProgress(bytesRead);
            return bytesRead;
        }

        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            int bytesRead = await _inner.ReadAsync(buffer, cancellationToken);
            ReportProgress(bytesRead);
            return bytesRead;
        }

        // Pomocnicza metoda centralizująca raportowanie postępu
        private void ReportProgress(int bytesRead)
        {
            if (bytesRead <= 0 || _totalBytes <= 0) return;

            _bytesSent += bytesRead;
            double progress = (double)_bytesSent / _totalBytes;

            // Zabezpieczenie, aby pasek nie przekroczył 100% (1.0) przy zaokrągleniach strumienia
            if (progress > 1.0) progress = 1.0;

            _reportProgress(progress);
        }

        // KLUCZOWA POPRAWKA POD REFRESH TOKEN: 
        // Gdy handler cofa strumień na początek pliku, musimy wyzerować licznik wysłanych bajtów!
        public override long Seek(long offset, SeekOrigin origin)
        {
            long position = _inner.Seek(offset, origin);
            if (origin == SeekOrigin.Begin && offset == 0)
            {
                _bytesSent = 0; // Resetujemy licznik dla drugiej próby (po odświeżeniu tokenu)
            }
            return position;
        }

        // Prawidłowe zarządzanie zwalnianiem zasobów wewnętrznych
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }
            base.Dispose(disposing);
        }

        public override async ValueTask DisposeAsync()
        {
            await _inner.DisposeAsync();
            await base.DisposeAsync();
        }

        // Delegowanie pozostałych metod i właściwości
        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => _inner.CanSeek;
        public override bool CanWrite => _inner.CanWrite;
        public override long Length => _inner.Length;
        public override long Position { get => _inner.Position; set => _inner.Position = value; }
        public override void Flush() => _inner.Flush();
        public override Task FlushAsync(CancellationToken cancellationToken) => _inner.FlushAsync(cancellationToken);
        public override void SetLength(long value) => _inner.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => _inner.WriteAsync(buffer, offset, count, cancellationToken);
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => _inner.WriteAsync(buffer, cancellationToken);
    }
}
