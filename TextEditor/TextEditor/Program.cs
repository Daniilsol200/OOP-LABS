using System;
using System.Windows.Forms;

namespace TextEditorApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles(); // Включает современный стиль элементов управления
            Application.SetCompatibleTextRenderingDefault(false); // Настройка рендеринга текста
            Application.Run(new TextEditor()); // Запускает форму TextEditor
        }
    }
}