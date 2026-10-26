using System;
using System.Collections.Generic;
using System.IO;

namespace lab3csharp
{
    class Program
    {
        public static void Main(string[] args)
        {
            string pathIn = "samTEXT.txt";

            if (!File.Exists(pathIn))
            {
                Console.WriteLine("Такого файла нет");
                return;
            }

            string rawText = File.ReadAllText(pathIn);
            Text text = new Text(rawText);

            Console.WriteLine("--- Исходный текст: ---");
            Console.WriteLine(text.ToString());
            Console.WriteLine("\n\n------------------------------------\n\n");

            Console.WriteLine("--- Предложения по возрастанию числа слов: ---");
            Console.WriteLine(text.SortPoWord());
            Console.WriteLine("\n\n------------------------------------\n\n");

            Console.WriteLine("--- Предложения по возрастанию длины строки: ---");
            Console.WriteLine(text.SortPoSize());
            Console.WriteLine("\n\n------------------------------------\n\n");

            Console.Write("Задайте длину слова для поиска в вопросительных предложениях: ");
            if (int.TryParse(Console.ReadLine(), out int dlina))
            {
                Console.WriteLine();
                List<string> resultWords = text.GetWordsByLength(dlina);
                Console.WriteLine($"Слова длиной {dlina} буквы из вопросительных предложений:");
                foreach (var w in resultWords)
                {
                    Console.WriteLine(w);
                }
            }
            Console.WriteLine("\n\n------------------------------------\n\n");

            Console.Write("Введите длину слов для удаления (начинающихся с согласной): ");
            if (int.TryParse(Console.ReadLine(), out int lengthToRemove))
            {
                text.Task4(lengthToRemove);
                Console.WriteLine("\nТекст после удаления слов:");
                Console.WriteLine(text.ToString());
            }

            Console.WriteLine("\n\n------------------------------------\n\n");

            Console.Write("Введите номер предложения для замены (1, 2, 3...): ");
            if (int.TryParse(Console.ReadLine(), out int sentenceNum))
            {
                Console.Write("Введите длину заменяемых слов: ");
                if (int.TryParse(Console.ReadLine(), out int targetLen))
                {
                    Console.Write("Введите подстроку для замены: ");
                    string replacement = Console.ReadLine();

                    text.ReplaceWordsInSentence(sentenceNum - 1, targetLen, replacement);

                    Console.WriteLine("\nТекст после замены:");
                    Console.WriteLine(text.ToString());
                }
            }

            Console.WriteLine("\n\n------------------------------------\n\n");
            Console.WriteLine("\nТекст после удаления стоп-слов:");
            text.RemoveStopWords(text.RU);
            text.RemoveStopWords(text.EN);
            Console.WriteLine(text.ToString());
            Console.WriteLine("\n\n------------------------------------\n\n");

            string xmlPath = "result_text.xml";
            text.ExportToXml(xmlPath);
            Console.WriteLine($"Объект успешно экспортирован в XML-файл: {xmlPath}");

            Console.WriteLine("\n\n------------------------------------\n\n");
            Console.WriteLine("--- Сгенерированный Конкорданс текста: ---");
            Console.WriteLine(text.BuildConcordance());
        }
    }
}
