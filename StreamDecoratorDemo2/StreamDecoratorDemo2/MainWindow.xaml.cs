using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using StreamDecorator;

namespace StreamDecoratorDemo
{
    /// <summary>
    /// Логика взаимодействия для главного окна приложения
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StreamTypeComboBox.SelectedIndex = 0;
        }

        private void StartWriting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(TimeoutTextBox.Text, out int timeout) || timeout <= 0)
                {
                    MessageBox.Show("Введите корректный таймаут (положительное число секунд)");
                    return;
                }

                string streamType = (StreamTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                Stream baseStream = CreateStream(streamType);
                Stream decoratedStream = new TimeoutStreamDecorator(baseStream, timeout);

                OutputTextBox.Clear();
                WriteToStream(decoratedStream, "Начало записи в поток...\n");

                // Имитация записи с задержкой
                System.Threading.Thread.Sleep(2000);
                WriteToStream(decoratedStream, "Запись после 2 секунд...\n");

                System.Threading.Thread.Sleep(timeout * 1000);
                WriteToStream(decoratedStream, "Попытка записи после таймаута...\n");
            }
            catch (Exception ex)
            {
                OutputTextBox.AppendText($"Ошибка: {ex.Message}\n");
            }
        }

        private Stream CreateStream(string streamType)
        {
            switch (streamType)
            {
                case "FileStream":
                    string filePath = Path.Combine(Environment.CurrentDirectory, "test.txt");
                    return new FileStream(filePath, FileMode.Create, FileAccess.Write);
                case "MemoryStream":
                    return new MemoryStream();
                case "BufferedStream":
                    return new BufferedStream(new MemoryStream());
                default:
                    throw new ArgumentException("Неизвестный тип потока");
            }
        }

        private void WriteToStream(Stream stream, string text)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(text);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                OutputTextBox.AppendText(text);
            }
            catch (TimeoutException ex)
            {
                OutputTextBox.AppendText($"Ошибка: {ex.Message}\n");
            }
            catch (Exception ex)
            {
                OutputTextBox.AppendText($"Неизвестная ошибка: {ex.Message}\n");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            OutputTextBox.Clear();
        }
    }
}