using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace lab3csharp
{
    [XmlRoot("text")]
    public class Text
    {
        [XmlElement("sentence")]
        public List<Sentence> Sentences = new List<Sentence>();

        [XmlIgnore]
        public HashSet<string> RU = new HashSet<string>();

        [XmlIgnore]
        public HashSet<string> EN = new HashSet<string>();

        public Text() { }

        public Text(string text)
        {
            string pathEN = "stopwords_en.txt";
            string pathRU = "stopwords_ru.txt";

            RU = LoadStopWords(pathRU);
            EN = LoadStopWords(pathEN);

            string[] rawSentences = Regex.Split(text, @"(?<=[.!?])\s+");

            foreach (string sentence in rawSentences)
            {
                if (!string.IsNullOrWhiteSpace(sentence))
                {
                    Sentences.Add(new Sentence(sentence));
                }
            }
        }

        public override string ToString()
        {
            return string.Join(" ", Sentences);
        }

        public string SortPoWord()
        {
            var sb = new StringBuilder();
            Sentences = Sentences.OrderBy(sentence => sentence.tokens.Count(token => token is Word)).ToList();
            int i = 0;
            foreach (Sentence sentence in Sentences)
            {
                i++;
                sb.Append(i.ToString() + " ");
                sb.AppendLine(sentence.ToString());
            }
            return sb.ToString();
        }

        public string SortPoSize()
        {
            var sb = new StringBuilder();
            int i = 0;
            Sentences = Sentences.OrderBy(sentence => sentence.size).ToList();
            foreach (Sentence sentence in Sentences)
            {
                i++;
                sb.Append(i.ToString() + " ");
                sb.AppendLine(sentence.ToString());
            }
            return sb.ToString();
        }

        public List<string> GetWordsByLength(int targetLength)
        {
            List<string> foundWords = new List<string>();
            foreach (var sentence in Sentences)
            {
                if (sentence.tokens.Count > 0 && sentence.tokens[sentence.tokens.Count - 1] is Punctuation lastPunct && lastPunct.punctuation == "?")
                {
                    foreach (var token in sentence.tokens)
                    {
                        if (token is Word word && word.worD.Length == targetLength)
                        {
                            string currentWord = word.worD;
                            if (!foundWords.Contains(currentWord, StringComparer.OrdinalIgnoreCase))
                            {
                                foundWords.Add(currentWord);
                            }
                        }
                    }
                }
            }
            return foundWords;
        }

        private bool IsConsonant(string word)
        {
            if (string.IsNullOrEmpty(word)) return false;
            char firstChar = char.ToLowerInvariant(word[0]);
            string vowels = "аеёиоуыэюяaeiouy";
            return !vowels.Contains(firstChar);
        }

        public void Task4(int targetLength)
        {
            foreach (var sentence in Sentences)
            {
                sentence.tokens.RemoveAll(token =>
                {
                    if (token is Word word)
                    {
                        if (word.worD.Length == targetLength && IsConsonant(word.worD))
                        {
                            return true;
                        }
                    }
                    return false;
                });

                sentence.Words.Clear();
                foreach (var t in sentence.tokens)
                {
                    if (t is Word w) sentence.Words.Add(w);
                }
            }
        }

        public void ReplaceWordsInSentence(int sentenceIndex, int targetLength, string replacement)
        {
            if (sentenceIndex < 0 || sentenceIndex >= Sentences.Count)
            {
                Console.WriteLine("Ошибка: предложения с таким номером нет");
                return;
            }

            Sentence targetSentence = Sentences[sentenceIndex];
            for (int i = 0; i < targetSentence.tokens.Count; i++)
            {
                if (targetSentence.tokens[i] is Word word && word.worD.Length == targetLength)
                {
                    targetSentence.tokens[i] = new Word(replacement);
                }
            }

            targetSentence.Words.Clear();
            foreach (var t in targetSentence.tokens)
            {
                if (t is Word w) targetSentence.Words.Add(w);
            }
        }

        public HashSet<string> LoadStopWords(string filePath)
        {
            HashSet<string> stopWords = new HashSet<string>();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string word = line.Trim().ToLower();
                    if (!string.IsNullOrEmpty(word))
                    {
                        stopWords.Add(word);
                    }
                }
            }
            return stopWords;
        }

        public void RemoveStopWords(HashSet<string> stopWords)
        {
            if (stopWords == null || stopWords.Count == 0) return;
            foreach (var sentence in Sentences)
            {
                sentence.tokens.RemoveAll(token =>
                {
                    if (token is Word word)
                    {
                        string cleanWord = word.worD.ToLower();
                        return stopWords.Contains(cleanWord);
                    }
                    return false;
                });

                sentence.Words.Clear();
                foreach (var t in sentence.tokens)
                {
                    if (t is Word w) sentence.Words.Add(w);
                }
            }
        }

        private class ConcordanceInfo
        {
            public string DisplayWord;
            public int Count;
            public SortedSet<int> Lines = new SortedSet<int>();
            public ConcordanceInfo(string displayWord) { DisplayWord = displayWord; }
        }

        public string BuildConcordance()
        {
            SortedDictionary<string, ConcordanceInfo> concordance =
                new SortedDictionary<string, ConcordanceInfo>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < Sentences.Count; i++)
            {
                int lineNum = i + 1;
                foreach (var token in Sentences[i].tokens)
                {
                    if (token is Word wordObj)
                    {
                        string word = wordObj.worD;
                        string key = word.ToLower();

                        if (!concordance.TryGetValue(key, out var info))
                        {
                            info = new ConcordanceInfo(word);
                            concordance[key] = info;
                        }

                        info.Count++;
                        info.Lines.Add(lineNum);
                    }
                }
            }

            var sb = new StringBuilder();
            foreach (var kvp in concordance)
            {
                var info = kvp.Value;
                string dots = new string('.', Math.Max(1, 35 - info.DisplayWord.Length));
                sb.Append($"{info.DisplayWord}{dots}{info.Count}:");
                sb.AppendLine(" " + string.Join(" ", info.Lines));
            }
            return sb.ToString();
        }

        public void ExportToXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Text));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, this);
            }
        }
    }
}
