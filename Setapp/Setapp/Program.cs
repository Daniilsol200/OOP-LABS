// CustomSetConsole/Program.cs
using CustomSetLibrary;
using System;

class Program
{
    static void Main(string[] args)
    {
        CustomSet<string> set = new CustomSet<string>(); // Используем string вместо char
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Custom Set Demo (Strings)");
            Console.WriteLine("-------------------");
            Console.WriteLine("1. Add item");
            Console.WriteLine("2. Remove item");
            Console.WriteLine("3. Clear set");
            Console.WriteLine("4. Show contents");
            Console.WriteLine("5. Exit");
            Console.WriteLine("-------------------");
            Console.Write("Select an option (1-5): ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter a value to add (e.g., '11', 'abc'): ");
                    string valueToAdd = Console.ReadLine();
                    if (!string.IsNullOrEmpty(valueToAdd)) // Проверяем, что ввод не пустой
                    {
                        if (set.Contains(valueToAdd))
                            Console.WriteLine($"Value '{valueToAdd}' already exists");
                        else
                        {
                            set.Add(valueToAdd);
                            Console.WriteLine($"Added '{valueToAdd}'");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Input cannot be empty");
                    }
                    break;

                case "2":
                    Console.Write("Enter a value to remove (e.g., '11', 'abc'): ");
                    string valueToRemove = Console.ReadLine();
                    if (!string.IsNullOrEmpty(valueToRemove))
                    {
                        if (set.Remove(valueToRemove))
                            Console.WriteLine($"Removed '{valueToRemove}'");
                        else
                            Console.WriteLine($"Value '{valueToRemove}' not found");
                    }
                    else
                    {
                        Console.WriteLine("Input cannot be empty");
                    }
                    break;

                case "3":
                    set.Clear();
                    Console.WriteLine("Set cleared");
                    break;

                case "4":
                    Console.WriteLine("Current set contents:");
                    if (set.Count == 0)
                        Console.WriteLine("Set is empty");
                    else
                    {
                        foreach (string item in set)
                        {
                            Console.Write($"{item} ");
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine($"Count: {set.Count}");
                    break;

                case "5":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid option. Please select 1-5.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}