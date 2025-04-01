using System;
using System.Drawing;
using System.Windows.Forms;

namespace TextEditorApp
{
    public class TextEditor : Form
    {
        // Явно объявленный делегат для обработки текста
        // Он определяет методы, которые принимают строку (string) и ничего не возвращают (void)
        public delegate void TextHandler(string text);

        // Поля класса
        private TextBox textBox;        // Текстовое поле
        private Button btnOpen;         // Кнопка "Открыть"
        private Button btnSave;         // Кнопка "Сохранить"
        private Button btnCopy;         // Кнопка "Копировать"
        private Button btnPaste;        // Кнопка "Вставить"
        private TextHandler textHandler; // Поле для хранения делегата

        public TextEditor()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Настройка формы
            this.Text = "Текстовый редактор";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создание текстового поля
            textBox = new TextBox
            {
                Multiline = true,
                Size = new Size(560, 300),
                Location = new Point(10, 10),
                ScrollBars = ScrollBars.Vertical,
                AcceptsTab = true
            };

            // Создание кнопок
            btnOpen = new Button
            {
                Text = "Открыть",
                Location = new Point(10, 320),
                Size = new Size(100, 30)
            };

            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(120, 320),
                Size = new Size(100, 30)
            };

            btnCopy = new Button
            {
                Text = "Копировать",
                Location = new Point(230, 320),
                Size = new Size(100, 30)
            };

            btnPaste = new Button
            {
                Text = "Вставить",
                Location = new Point(340, 320),
                Size = new Size(100, 30)
            };

            // Добавление компонентов на форму
            this.Controls.Add(textBox);
            this.Controls.Add(btnOpen);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCopy);
            this.Controls.Add(btnPaste);

            // Настройка событий
            SetupEvents();
        }

        private void SetupEvents()
        {
            // Событие для кнопки "Открыть" (лямбда-выражение)
            btnOpen.Click += (sender, e) =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        textBox.Text = System.IO.File.ReadAllText(openFileDialog.FileName);
                    }
                }
            };

            // Событие для кнопки "Сохранить" (встроенный делегат EventHandler)
            btnSave.Click += new EventHandler(SaveFile);

            // Инициализация нашего делегата для копирования текста
            textHandler = CopyText; // Привязываем метод CopyText к делегату

            // Событие для кнопки "Копировать" с использованием нашего делегата
            btnCopy.Click += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(textBox.SelectedText))
                {
                    textHandler(textBox.SelectedText); // Вызываем делегат
                }
            };

            // Событие для кнопки "Вставить" (встроенный делегат EventHandler)
            btnPaste.Click += new EventHandler(PasteText);
        }

        // Метод для сохранения файла
        private void SaveFile(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, textBox.Text);
                }
            }
        }

        // Метод для вставки текста
        private void PasteText(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textBox.Paste(Clipboard.GetText());
            }
        }

        // Метод, соответствующий сигнатуре делегата TextHandler
        private void CopyText(string text)
        {
            Clipboard.SetText(text); // Копируем текст в буфер обмена
            MessageBox.Show($"Скопировано: {text}"); // Показываем уведомление
        }
    }
}