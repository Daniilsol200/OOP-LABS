using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // LINQ-запрос в одну строку для обработки текста
            var result = File.ReadAllText("input.txt")
                .Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => new string(word.ToLower().Where(char.IsLetter).ToArray()))
                .Where(word => word.Length >= 4)
                .GroupBy(word => word)
                .Select(g => Tuple.Create(g.Key, g.Count()))
                .OrderByDescending(t => t.Item2)
                .ThenBy(t => t.Item1)
                .ToList();

            // Вывод результата
            Console.WriteLine("Слова и их частота (отсортировано по убыванию частоты, затем по алфавиту):");
            foreach (var tuple in result)
            {
                Console.WriteLine($"Слово: {tuple.Item1}, Количество: {tuple.Item2}");
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Ошибка: Файл 'input.txt' не найден.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}