using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public static class CommandExecutor
    {
        public static string Search(string searchSequence, List<GeneticData> dataList, int count)
        {
            StringBuilder sb = new StringBuilder();
            bool recordFound = false;

            foreach (var currentItem in dataList)
            {
                if (currentItem.amino_acids.IndexOf(searchSequence, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    sb.Append("\n------------------------------------------------------------------------");
                    sb.Append($"\n{count} search  {searchSequence}");
                    sb.Append($"\norganism\t\tprotein");
                    sb.Append($"\n{currentItem.organism}\t{currentItem.protein}");
                    recordFound = true;
                }
            }

            if (!recordFound)
            {
                sb.Append("\n------------------------------------------------------------------------");
                sb.Append($"\n{count} search  {searchSequence}");
                sb.Append($"\n Not Found ");
            }

            return sb.ToString();
        }

        public static string Diff(string protein1, string protein2, List<GeneticData> dataList, int count)
        {
            StringBuilder output = new StringBuilder();
            GeneticData? firstMatch = null;
            GeneticData? secondMatch = null;

            foreach (var node in dataList)
            {
                if (node.protein == protein1)
                    firstMatch = node;
                if (node.protein == protein2)
                    secondMatch = node;
            }

            // Защита от NullReferenceException, если один из протеинов не найден в базе
            if (firstMatch == null || secondMatch == null)
            {
                output.Append("\n------------------------------------------------------------------------");
                output.Append($"\n{count} diff \t{protein1} \t{protein2}");
                output.AppendLine($"\nOne or both proteins NOT FOUND");
                return output.ToString();
            }

            string sequenceA = firstMatch.Value.amino_acids;
            string sequenceB = secondMatch.Value.amino_acids;

            int totalDifferences = 0;
            int shortestLength = sequenceA.Length < sequenceB.Length ? sequenceA.Length : sequenceB.Length;

            for (int idx = 0; idx < shortestLength; idx++)
            {
                if (sequenceA[idx] != sequenceB[idx])
                {
                    totalDifferences++;
                }
            }

            totalDifferences += Math.Abs(sequenceA.Length - sequenceB.Length);

            output.Append("\n------------------------------------------------------------------------");
            output.Append($"\n{count} diff \t{protein1} \t{protein2}");
            output.AppendLine($"\nAmino-acid occurs: {totalDifferences}");

            return output.ToString();
        }

        public static string Mode(string proteinName, List<GeneticData> dataList, int count)
        {
            StringBuilder output = new StringBuilder();
            GeneticData? matchedGene = null;

            foreach (var element in dataList)
            {
                if (element.protein.Equals(proteinName, StringComparison.OrdinalIgnoreCase))
                {
                    matchedGene = element;
                    break;
                }
            }

            if (matchedGene == null) return "NOT FOUND";

            string targetChain = matchedGene.Value.amino_acids;

            if (string.IsNullOrEmpty(targetChain))
            {
                output.Append("\n------------------------------------------------------------------------");
                output.Append($"\n{count}\tmode \t{proteinName}");
                output.AppendLine($"\nAmino-acid occurs:\nSequence is empty");
                return output.ToString();
            }

            Dictionary<char, int> freqMap = new Dictionary<char, int>();

            foreach (char letter in targetChain)
            {
                if (freqMap.TryGetValue(letter, out int currentCount))
                    freqMap[letter] = currentCount + 1;
                else
                    freqMap[letter] = 1;
            }

            int peakCount = 0;
            foreach (var pair in freqMap)
            {
                if (pair.Value > peakCount)
                    peakCount = pair.Value;
            }

            List<char> topChars = new List<char>();
            foreach (var pair in freqMap)
            {
                if (pair.Value == peakCount)
                    topChars.Add(pair.Key);
            }

            topChars.Sort();
            char primaryChar = topChars[0];

            output.Append("\n------------------------------------------------------------------------");
            output.Append($"\n{count}\tmode \t{proteinName}");
            output.AppendLine($"\nAmino-acid occurs:");
            output.Append($"{primaryChar}\t {peakCount}");

            return output.ToString();
        }
    }
}

