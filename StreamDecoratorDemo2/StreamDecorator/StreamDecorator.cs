using System;
using System.IO;

namespace StreamDecorator
{
    /// <summary>
    /// Абстрактный базовый класс Stream для реализации Декоратора
    /// </summary>
    public abstract class StreamDecorator : Stream
    {
        protected Stream _stream;

        protected StreamDecorator(Stream stream)
        {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;

        public override long Position
        {
            get => _stream.Position;
            set => _stream.Position = value;
        }

        public override void Flush() => _stream.Flush();
        public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);
        public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);
        public override void SetLength(long value) => _stream.SetLength(value);
        public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);
    }
}