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

        public override string ToString()
        {
            return string.Join(" ", Sentences);
        }
    }
}

