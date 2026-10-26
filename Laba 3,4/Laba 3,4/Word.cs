using System.Xml.Serialization;

namespace lab3csharp
{
    public class Word
    {
        [XmlText]
        public string worD;

        [XmlIgnore]
        public int sizeWord;

        public Word() { }
        public Word(string worD)
        {
            this.worD = worD;
            this.sizeWord = worD.Length;
        }

        public override string ToString() => worD;
    }
}
