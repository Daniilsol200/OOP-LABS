using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using StreamDecorator;

namespace StreamDecorator.Tests
{
    [TestFixture]
    public class StreamDecoratorTests
    {
        private string _testFilePath;
        private Stream _fileStream;
        private Stream _memoryStream;
        private Stream _bufferedStream;

        [SetUp]
        public void Setup()
        {
            // Создаем временный файл для FileStream
            _testFilePath = Path.Combine(Path.GetTempPath(), "test_stream.txt");
            _fileStream = new FileStream(_testFilePath, FileMode.Create, FileAccess.ReadWrite);
            _memoryStream = new MemoryStream();
            _bufferedStream = new BufferedStream(new MemoryStream());
        }

        [TearDown]
        public void Teardown()
        {
            // Очищаем ресурсы
            _fileStream.Dispose();
            _memoryStream.Dispose();
            _bufferedStream.Dispose();
            if (File.Exists(_testFilePath))
                File.Delete(_testFilePath);
        }

        [Test]
        public void StreamDecorator_BasicWriteAndRead_SetsValuesCorrectly()
        {
            // Arrange
            var decorator = new TestStreamDecorator(_memoryStream);
            string input = "Тестовая запись";
            byte[] writeBuffer = Encoding.UTF8.GetBytes(input);

            // Act
            decorator.Write(writeBuffer, 0, writeBuffer.Length);
            decorator.Flush();

            _memoryStream.Position = 0;
            byte[] readBuffer = new byte[writeBuffer.Length];
            int bytesRead = decorator.Read(readBuffer, 0, readBuffer.Length);

            // Assert
            string result = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
            Assert.AreEqual(input, result, "Записанные и прочитанные данные не совпадают");
            Assert.IsTrue(decorator.CanWrite, "CanWrite должен быть true");
            Assert.IsTrue(decorator.CanRead, "CanRead должен быть true");
        }

        [TestCase("FileStream")]
        [TestCase("MemoryStream")]
        [TestCase("BufferedStream")]
        public void TimeoutStreamDecorator_WriteBeforeTimeout_WritesCorrectly(string streamType)
        {
            // Arrange
            Stream baseStream = GetStreamByType(streamType);
            using var decorator = new TimeoutStreamDecorator(baseStream, 5); // 5 секунд таймаута
            string input = "Запись до таймаута";
            byte[] buffer = Encoding.UTF8.GetBytes(input);

            // Act
            decorator.Write(buffer, 0, buffer.Length);
            decorator.Flush();

            // Assert
            if (baseStream.CanSeek)
            {
                baseStream.Position = 0;
                byte[] readBuffer = new byte[buffer.Length];
                int bytesRead = baseStream.Read(readBuffer, 0, readBuffer.Length);
                string result = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
                Assert.AreEqual(input, result, $"Данные в {streamType} не записались корректно");
            }
            Assert.IsTrue(decorator.CanWrite, $"CanWrite должен быть true для {streamType} до таймаута");
        }

        [TestCase("FileStream")]
        [TestCase("MemoryStream")]
        [TestCase("BufferedStream")]
        public void TimeoutStreamDecorator_WriteAfterTimeout_ThrowsTimeoutException(string streamType)
        {
            // Arrange
            Stream baseStream = GetStreamByType(streamType);
            using var decorator = new TimeoutStreamDecorator(baseStream, 1); // 1 секунда таймаута
            byte[] buffer = Encoding.UTF8.GetBytes("Запись после таймаута");

            // Act
            Thread.Sleep(1500); // Ждем 1.5 секунды, чтобы таймаут истек

            // Assert
            var exception = Assert.Throws<TimeoutException>(() =>
                decorator.Write(buffer, 0, buffer.Length),
                $"Ожидалось исключение TimeoutException для {streamType}");
            Assert.AreEqual("Время для записи в поток истекло", exception.Message,
                $"Сообщение исключения некорректно для {streamType}");
            Assert.IsFalse(decorator.CanWrite, $"CanWrite должен быть false после таймаута для {streamType}");
        }

        [Test]
        public void TimeoutStreamDecorator_NullStream_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new TimeoutStreamDecorator(null, 5),
                "Ожидалось исключение ArgumentNullException при передаче null");
            Assert.AreEqual("stream", exception.ParamName, "Неверное имя параметра в исключении");
        }

        [Test]
        public void TimeoutStreamDecorator_InvalidTimeout_ThrowsArgumentException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new TimeoutStreamDecorator(memoryStream, 0),
                "Ожидалось исключение ArgumentException при некорректном таймауте");
            Assert.IsTrue(exception.Message.Contains("Таймаут должен быть положительным числом"),
                "Сообщение исключения не содержит ожидаемого текста");
        }

        private Stream GetStreamByType(string streamType)
        {
            switch (streamType)
            {
                case "FileStream":
                    return _fileStream;
                case "MemoryStream":
                    return _memoryStream;
                case "BufferedStream":
                    return _bufferedStream;
                default:
                    throw new ArgumentException($"Неизвестный тип потока: {streamType}");
            }
        }
    }

    // Вспомогательный класс для тестирования абстрактного StreamDecorator
    internal class TestStreamDecorator : StreamDecorator
    {
        public TestStreamDecorator(Stream stream) : base(stream) { }
    }
}