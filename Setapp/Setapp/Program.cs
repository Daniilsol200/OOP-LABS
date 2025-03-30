// CustomSetConsole/Program.cs
using CustomSetLibrary;
using System;

class Program
{
    static void Main(string[] args)
    {
        CustomSet<char> set = new CustomSet<char>();
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("Custom Set Demo");
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
                    Console.Write("Enter a single character to add: ");
                    string inputAdd = Console.ReadLine();
                    if (!string.IsNullOrEmpty(inputAdd) && inputAdd.Length == 1)
                    {
                        char valueToAdd = inputAdd[0];
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
                        Console.WriteLine("Please enter exactly one character");
                    }
                    break;

                case "2":
                    Console.Write("Enter a single character to remove: ");
                    string inputRemove = Console.ReadLine();
                    if (!string.IsNullOrEmpty(inputRemove) && inputRemove.Length == 1)
                    {
                        char valueToRemove = inputRemove[0];
                        if (set.Remove(valueToRemove))
                            Console.WriteLine($"Removed '{valueToRemove}'");
                        else
                            Console.WriteLine($"Value '{valueToRemove}' not found");
                    }
                    else
                    {
                        Console.WriteLine("Please enter exactly one character");
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
                        foreach (char item in set)
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