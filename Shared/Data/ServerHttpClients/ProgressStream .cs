namespace Shared.Data.ServerHttpClients
{
    public partial class ProgressStream : Stream
    {
        private readonly Stream _inner;
        private readonly long _totalBytes;
        private readonly Action<double> _reportProgress;
        private long _bytesSent;

        public ProgressStream(Stream inner, long totalBytes, Action<double> reportProgress)
        {
            _inner = inner;
            _totalBytes = totalBytes;
            _reportProgress = reportProgress;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = _inner.Read(buffer, offset, count);
            _bytesSent += bytesRead;
            double progress = (double)_bytesSent / _totalBytes;
            _reportProgress(progress);
            return bytesRead;
        }
        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int bytesRead = await _inner.ReadAsync(buffer.AsMemory(offset, count), cancellationToken);
            _bytesSent += bytesRead;
            double progress = (double)_bytesSent / _totalBytes;
            _reportProgress(progress);
            return bytesRead;
        }
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            int bytesRead = await _inner.ReadAsync(buffer, cancellationToken);
            _bytesSent += bytesRead;
            double progress = (double)_bytesSent / _totalBytes;
            _reportProgress(progress);
            return bytesRead;
        }

        // Delegowanie pozostałych metod
        public override bool CanRead => _inner.CanRead;
        public override bool CanSeek => _inner.CanSeek;
        public override bool CanWrite => _inner.CanWrite;
        public override long Length => _inner.Length;
        public override long Position { get => _inner.Position; set => _inner.Position = value; }
        public override void Flush() => _inner.Flush();
        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);
        public override void SetLength(long value) => _inner.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _inner.Write(buffer, offset, count);
    }
}
