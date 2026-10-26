using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace lab3csharp
{
    public class Sentence
    {
        [XmlElement("word")]
        public List<Word> Words = new List<Word>();

        [XmlIgnore]
        public List<object> tokens = new List<object>();

        [XmlIgnore]
        public int size;

        public Sentence() { }

        public Sentence(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return;
            size = str.Length;
            string pattern = @"([\p{L}]+)|([.,!?;:—])";
            MatchCollection matches = Regex.Matches(str, pattern);

            foreach (Match match in matches)
            {
                if (match.Groups[1].Success)
                {
                    var w = new Word(match.Value);
                    tokens.Add(w);
                    Words.Add(w);
                }
                else if (match.Groups[2].Success)
                {
                    tokens.Add(new Punctuation(match.Value));
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tokens.Count; i++)
            {
                string tokenStr = tokens[i].ToString();

                if (i > 0 && tokens[i] is Word)
                {
                    sb.Append(" ");
                }
                sb.Append(tokenStr);
            }
            return sb.ToString();
        }
    }
}
