using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceRecords = "sequences.txt";
            string sourceCommands = "commands.txt";
            string destinationPath = "genedata.txt";

            List<GeneticData> mainDatabase = GetData1(sourceRecords);

            if (mainDatabase == null) return;

            try
            {
                using (StreamReader fileReader = new StreamReader(sourceCommands))
                {
                    int lineCounter = 1;
                    string currentLine;
                    StringBuilder logBuilder = new StringBuilder();

                    while ((currentLine = fileReader.ReadLine()) != null)
                    {
                        string[] tokens = currentLine.Split('\t');
                        if (tokens.Length > 0 && !string.IsNullOrWhiteSpace(tokens[0]))
                        {
                            switch (tokens[0].Trim())
                            {
                                case "search":
                                    {
                                        if (tokens.Length > 1)
                                        {
                                            string querySequence = tokens[1].Trim();
                                            string searchOutput = CommandExecutor.Search(querySequence, mainDatabase, lineCounter);
                                            logBuilder.AppendLine(searchOutput);
                                        }
                                        break;
                                    }
                                case "diff":
                                    {
                                        if (tokens.Length > 2)
                                        {
                                            string diffOutput = CommandExecutor.Diff(tokens[1].Trim(), tokens[2].Trim(), mainDatabase, lineCounter);
                                            logBuilder.AppendLine(diffOutput);
                                        }
                                        break;
                                    }
                                case "mode":
                                    {
                                        if (tokens.Length > 1)
                                        {
                                            string modeOutput = CommandExecutor.Mode(tokens[1].Trim(), mainDatabase, lineCounter);
                                            logBuilder.AppendLine(modeOutput);
                                        }
                                        break;
                                    }
                                default:
                                    Console.WriteLine("Ошибка команды");
                                    break;
                            }
                        }
                        lineCounter++;
                    }

                    logBuilder.Append("\n------------------------------------------------------------------------");
                    File.WriteAllText(destinationPath, logBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static List<GeneticData> GetData1(string filePath)
        {
            List<GeneticData> recordsList = new List<GeneticData>();

            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Файл не найден: {filePath}");
                    return null;
                }

                using (StreamReader fileReader = new StreamReader(filePath))
                {
                    string currentLine;
                    while ((currentLine = fileReader.ReadLine()) != null)
                    {
                        string[] lineSegments = currentLine.Split('\t');

                        if (lineSegments.Length >= 3)
                        {
                            string decodedChain = DecodeRLE(lineSegments[2].Trim());
                            recordsList.Add(new GeneticData(lineSegments[0].Trim(), lineSegments[1].Trim(), decodedChain));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении данных: {ex.Message}");
                return null;
            }

            return recordsList;
        }

        static string DecodeRLE(string rle)
        {
            if (string.IsNullOrEmpty(rle)) return "";

            string textBuffer = "";
            int index = 0;

            while (index < rle.Length)
            {
                if (char.IsDigit(rle[index]))
                {
                    string digitsChain = "";
                    while (index < rle.Length && char.IsDigit(rle[index]))
                    {
                        digitsChain += rle[index];
                        index++;
                    }

                    int repeatCount = int.Parse(digitsChain);

                    if (index < rle.Length)
                    {
                        char targetChar = rle[index];
                        textBuffer += new string(targetChar, repeatCount);
                        index++;
                    }
                }
                else
                {
                    textBuffer += rle[index];
                    index++;
                }
            }

            return textBuffer;
        }
    }
}
