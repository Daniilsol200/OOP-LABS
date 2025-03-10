using System;
using System.IO;

namespace StreamDecorator
{
    /// <summary>
    /// Декоратор, запрещающий запись после заданного таймаута
    /// </summary>
    public class TimeoutStreamDecorator : StreamDecorator
    {
        private readonly DateTime _expirationTime;

        public TimeoutStreamDecorator(Stream stream, int timeoutSeconds) : base(stream)
        {
            if (timeoutSeconds <= 0)
                throw new ArgumentException("Таймаут должен быть положительным числом", nameof(timeoutSeconds));
            _expirationTime = DateTime.Now.AddSeconds(timeoutSeconds);
        }

        public override bool CanWrite => base.CanWrite && DateTime.Now < _expirationTime;

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite)
                throw new TimeoutException("Время для записи в поток истекло");
            base.Write(buffer, offset, count);
        }
    }
}