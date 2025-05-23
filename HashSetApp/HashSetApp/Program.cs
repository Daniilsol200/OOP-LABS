﻿using HashSetLib;
using System;

namespace HashSetApp
{
    /// <summary>
    /// Основной класс консольного приложения для взаимодействия с пользователем.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Точка входа в приложение. Запрашивает числа у пользователя и выводит уникальные значения.
        /// </summary>
        /// <param name="args">Аргументы командной строки (не используются).</param>
        static void Main(string[] args)
        {
            CustomHashSet<int> set = new CustomHashSet<int>();

            Console.WriteLine("Введите числа (для завершения введите 'stop'):");

            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToLower() == "stop")
                    break;

                if (int.TryParse(input, out int number))
                {
                    set.Add(number);
                    Console.WriteLine($"Добавлено: {number}. Текущий размер: {set.Count}");
                }
                else
                {
                    Console.WriteLine("Пожалуйста, введите корректное число или 'stop'");
                }
            }

            Console.WriteLine("\nУникальные элементы в коллекции:");
            var enumerator = set.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.Write(enumerator.Current + " ");
            }
            Console.WriteLine($"\nОбщее количество уникальных элементов: {set.Count}");
        }
    }
}