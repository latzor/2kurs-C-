using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Очень_крутая_5_лаба__сам_делал_100__
{
    internal class Salad
    {
        private string saladName;
        private List<Ingredient> ingredients;

        public Salad(string name)
        {
            this.saladName = name;
            this.ingredients = new List<Ingredient>();
        }

        private void PrintTableHead()
        {
            Console.WriteLine(new string('-', 75));
            Console.WriteLine($"{"Название",-15} | {"Категория",-12} | {"Калории",-14} | {"Белки",-9} | {"Углеводы",-9}");
            Console.WriteLine(new string('-', 75));
        }

        public string GetInfo()
        {
            if (ingredients == null || ingredients.Count == 0)
            {
                return "\nВ салате пока нет ingredients.\n";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append($"\nСостав салата: {saladName}\n");
            sb.Append(new string('-', 40));
            sb.Append("\n");

            foreach (var i in ingredients)
            {
                sb.Append(i.ShortInf() + "\n");
            }
            return sb.ToString();
        }

        public void Add()
        {
            Console.WriteLine("\n--- Добавление ингредиента ---");
            Console.Write("1. Добавить овощ\n2. Добавить другой ингредиент\n\nВыбор: ");

            if (!int.TryParse(Console.ReadLine(), out int choice)) return;
            switch (choice)
            {
                case 1: AddVeg(); break;
                case 2: AddOther(); break;
                default: return;
            }
        }

        public void AddVeg()
        {
            Console.Write("Название овоща: ");
            string name = Console.ReadLine();
            Console.Write("Калорийность на 100г: ");
            double calories = double.Parse(Console.ReadLine());
            Console.Write("Углеводы: ");
            int uglevodi = int.Parse(Console.ReadLine());
            Console.Write("Белки: ");
            int belki = int.Parse(Console.ReadLine());

            ingredients.Add(new Vegetable(name, calories, uglevodi, belki));
            Console.WriteLine("\nИнгредиент добавлен.");
        }

        public void AddOther()
        {
            Console.Write("Название ингредиента: ");
            string name = Console.ReadLine();
            Console.Write("Калорийность на 100г: ");
            double calories = double.Parse(Console.ReadLine());
            Console.Write("Углеводы: ");
            int uglevodi = int.Parse(Console.ReadLine());
            Console.Write("Белки: ");
            int belki = int.Parse(Console.ReadLine());
            Console.Write("Категория: ");
            string category = Console.ReadLine();

            ingredients.Add(new OtherIngredient(name, calories, uglevodi, belki, category));
            Console.WriteLine("\nИнгредиент добавлен.");
        }

        public void Sort()
        {
            if (ingredients.Count == 0)
            {
                Console.WriteLine("\nСалат пуст. Нечего сортировать.");
                return;
            }

            Console.WriteLine("\n--- Сортировка ингредиентов ---");
            Console.WriteLine("1. По калориям\n2. По углеводам\n3. По белкам");
            Console.Write("\nВыберите параметр: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Неверный ввод!");
                return;
            }

            List<Ingredient> sortedList = null;

            switch (choice)
            {
                case 1:
                    sortedList = ingredients.OrderBy(x => x.GetInfoForSort().calories).ToList();
                    break;
                case 2:
                    sortedList = ingredients.OrderBy(x => x.GetInfoForSort().uglevody).ToList();
                    break;
                case 3:
                    sortedList = ingredients.OrderBy(x => x.GetInfoForSort().belki).ToList();
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    return;
            }

            Console.WriteLine("\nРезультат сортировки:");
            PrintTableHead();
            foreach (var ingredient in sortedList)
            {
                Console.WriteLine(ingredient.GetInfo());
            }
            Console.WriteLine(new string('-', 75));
        }

        public void Poisk()
        {
            Console.WriteLine("\n--- Поиск по калорийности ---");
            Console.Write("Минимальная калорийность: ");
            int min = int.Parse(Console.ReadLine());
            Console.Write("Максимальная калорийность: ");
            int max = int.Parse(Console.ReadLine());

            Console.WriteLine($"\nИнгредиенты в диапазоне от {min} до {max} ккал:\n");

            var found = ingredients.Where(i => i.Calories >= min && i.Calories <= max).ToList();

            if (found.Count == 0)
            {
                Console.WriteLine("Ничего не найдено.");
            }
            else
            {
                PrintTableHead();
                foreach (var i in found)
                {
                    Console.WriteLine(i.GetInfo());
                }
                Console.WriteLine(new string('-', 75));
            }
        }

        public void GetCall()
        {
            double sum = ingredients.Sum(i => i.Calories);
            Console.WriteLine($"\nОбщая калорийность салата: {sum:F1} ккал/100г");
        }
    }
}
