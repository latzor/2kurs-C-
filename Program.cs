using System;

namespace Очень_крутая_5_лаба__сам_делал_100__
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите название салата: ");
            Salad salad = new Salad(Console.ReadLine());

            while (true)
            {
                int choice = PrintMenu();
                switch (choice)
                {
                    case 1:
                        salad.Add();
                        break;
                    case 2:
                        salad.GetCall();
                        break;
                    case 3:
                        salad.Sort();
                        break;
                    case 4:
                        salad.Poisk();
                        break;
                    case 5:
                        Console.WriteLine(salad.GetInfo());
                        break;
                    case 0:
                        return;
                    default:
                        break;
                }
            }
        }

        public static int PrintMenu()
        {
            Console.WriteLine("\n=== МЕНЮ ===");
            Console.WriteLine("1. Добавить ингредиент");
            Console.WriteLine("2. Подсчитать калорийность");
            Console.WriteLine("3. Сортировка по параметрам");
            Console.WriteLine("4. Поиск по калорийности");
            Console.WriteLine("5. Показать состав салата");
            Console.WriteLine("0. Выход");
            Console.Write("\nВыберите действие: ");

            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }
            return -1;
        }
    }
}
