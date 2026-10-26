using System.Xml.Serialization;

namespace lab3csharp
{
    public class Punctuation
    {
        [XmlAttribute]
        public string punctuation { get; set; }

        public Punctuation() { }

        public Punctuation(string punctuation)
        {
            this.punctuation = punctuation;
        }

        public override string ToString() => punctuation;
    }
}
