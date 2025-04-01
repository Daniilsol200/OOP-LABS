using System;
using System.Drawing; // Для работы с Size, Point
using System.Windows.Forms; // Для работы с Form, Button, TextBox и т.д.

namespace TextEditorApp
{
    public class TextEditor : Form
    {
        // Объявление компонентов как полей класса
        private TextBox textBox; // Текстовое поле для редактирования
        private Button btnOpen;  // Кнопка "Открыть"
        private Button btnSave;  // Кнопка "Сохранить"
        private Button btnCopy;  // Кнопка "Копировать"
        private Button btnPaste; // Кнопка "Вставить"

        // Конструктор формы
        public TextEditor()
        {
            InitializeComponents(); // Вызов метода для создания интерфейса
        }

        // Метод для создания и настройки всех компонентов
        private void InitializeComponents()
        {
            // 1. Настройка самой формы
            this.Text = "Текстовый редактор"; // Заголовок окна
            this.Size = new Size(600, 400);   // Размер окна (ширина 600, высота 400)
            this.StartPosition = FormStartPosition.CenterScreen; // Центрирование окна

            // 2. Создание текстового поля
            textBox = new TextBox
            {
                Multiline = true,                // Многострочный режим
                Size = new Size(560, 300),       // Размер (ширина 560, высота 300)
                Location = new Point(10, 10),    // Положение (отступ 10px слева и сверху)
                ScrollBars = ScrollBars.Vertical,// Вертикальная прокрутка
                AcceptsTab = true                // Разрешить табуляцию
            };

            // 3. Создание кнопки "Открыть"
            btnOpen = new Button
            {
                Text = "Открыть",             // Текст на кнопке
                Location = new Point(10, 320), // Положение (10px слева, 320px сверху)
                Size = new Size(100, 30)      // Размер (ширина 100, высота 30)
            };

            // 4. Создание кнопки "Сохранить"
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(120, 320), // Сдвиг вправо на 110px от btnOpen
                Size = new Size(100, 30)
            };

            // 5. Создание кнопки "Копировать"
            btnCopy = new Button
            {
                Text = "Копировать",
                Location = new Point(230, 320), // Сдвиг вправо от btnSave
                Size = new Size(100, 30)
            };

            // 6. Создание кнопки "Вставить"
            btnPaste = new Button
            {
                Text = "Вставить",
                Location = new Point(340, 320), // Сдвиг вправо от btnCopy
                Size = new Size(100, 30)
            };

            // 7. Добавление всех компонентов на форму
            this.Controls.Add(textBox); // Добавляем текстовое поле
            this.Controls.Add(btnOpen); // Добавляем кнопку "Открыть"
            this.Controls.Add(btnSave); // Добавляем кнопку "Сохранить"
            this.Controls.Add(btnCopy); // Добавляем кнопку "Копировать"
            this.Controls.Add(btnPaste);// Добавляем кнопку "Вставить"

            // 8. Настройка событий для кнопок
            SetupEvents();
        }

        // Метод для подключения событий
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

            // Событие для кнопки "Сохранить" (делегат)
            btnSave.Click += new EventHandler(SaveFile);

            // Событие для кнопки "Копировать" (лямбда-выражение)
            btnCopy.Click += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(textBox.SelectedText))
                {
                    Clipboard.SetText(textBox.SelectedText);
                }
            };

            // Событие для кнопки "Вставить" (делегат)
            btnPaste.Click += new EventHandler(PasteText);
        }

        // Метод-обработчик для сохранения файла
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

        // Метод-обработчик для вставки текста
        private void PasteText(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textBox.Paste(Clipboard.GetText());
            }
        }
    }
}