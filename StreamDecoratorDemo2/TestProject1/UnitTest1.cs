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
            // ������� ��������� ���� ��� FileStream
            _testFilePath = Path.Combine(Path.GetTempPath(), "test_stream.txt");
            _fileStream = new FileStream(_testFilePath, FileMode.Create, FileAccess.ReadWrite);
            _memoryStream = new MemoryStream();
            _bufferedStream = new BufferedStream(new MemoryStream());
        }

        [TearDown]
        public void Teardown()
        {
            // ������� �������
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
            string input = "�������� ������";
            byte[] writeBuffer = Encoding.UTF8.GetBytes(input);

            // Act
            decorator.Write(writeBuffer, 0, writeBuffer.Length);
            decorator.Flush();

            _memoryStream.Position = 0;
            byte[] readBuffer = new byte[writeBuffer.Length];
            int bytesRead = decorator.Read(readBuffer, 0, readBuffer.Length);

            // Assert
            string result = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
            Assert.AreEqual(input, result, "���������� � ����������� ������ �� ���������");
            Assert.IsTrue(decorator.CanWrite, "CanWrite ������ ���� true");
            Assert.IsTrue(decorator.CanRead, "CanRead ������ ���� true");
        }

        [TestCase("FileStream")]
        [TestCase("MemoryStream")]
        [TestCase("BufferedStream")]
        public void TimeoutStreamDecorator_WriteBeforeTimeout_WritesCorrectly(string streamType)
        {
            // Arrange
            Stream baseStream = GetStreamByType(streamType);
            using var decorator = new TimeoutStreamDecorator(baseStream, 5); // 5 ������ ��������
            string input = "������ �� ��������";
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
                Assert.AreEqual(input, result, $"������ � {streamType} �� ���������� ���������");
            }
            Assert.IsTrue(decorator.CanWrite, $"CanWrite ������ ���� true ��� {streamType} �� ��������");
        }

        [TestCase("FileStream")]
        [TestCase("MemoryStream")]
        [TestCase("BufferedStream")]
        public void TimeoutStreamDecorator_WriteAfterTimeout_ThrowsTimeoutException(string streamType)
        {
            // Arrange
            Stream baseStream = GetStreamByType(streamType);
            using var decorator = new TimeoutStreamDecorator(baseStream, 1); // 1 ������� ��������
            byte[] buffer = Encoding.UTF8.GetBytes("������ ����� ��������");

            // Act
            Thread.Sleep(1500); // ���� 1.5 �������, ����� ������� �����

            // Assert
            var exception = Assert.Throws<TimeoutException>(() =>
                decorator.Write(buffer, 0, buffer.Length),
                $"��������� ���������� TimeoutException ��� {streamType}");
            Assert.AreEqual("����� ��� ������ � ����� �������", exception.Message,
                $"��������� ���������� ����������� ��� {streamType}");
            Assert.IsFalse(decorator.CanWrite, $"CanWrite ������ ���� false ����� �������� ��� {streamType}");
        }

        [Test]
        public void TimeoutStreamDecorator_NullStream_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new TimeoutStreamDecorator(null, 5),
                "��������� ���������� ArgumentNullException ��� �������� null");
            Assert.AreEqual("stream", exception.ParamName, "�������� ��� ��������� � ����������");
        }

        [Test]
        public void TimeoutStreamDecorator_InvalidTimeout_ThrowsArgumentException()
        {
            // Arrange
            using var memoryStream = new MemoryStream();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new TimeoutStreamDecorator(memoryStream, 0),
                "��������� ���������� ArgumentException ��� ������������ ��������");
            Assert.IsTrue(exception.Message.Contains("������� ������ ���� ������������� ������"),
                "��������� ���������� �� �������� ���������� ������");
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
                    throw new ArgumentException($"����������� ��� ������: {streamType}");
            }
        }
    }

    // ��������������� ����� ��� ������������ ������������ StreamDecorator
    internal class TestStreamDecorator : StreamDecorator
    {
        public TestStreamDecorator(Stream stream) : base(stream) { }
    }
}