using System.Xml.Serialization;

public enum TokenType { Word, Punctuation }

[XmlInclude(typeof(Word))]
[XmlInclude(typeof(Punctuation))]
public abstract class Token
{
    public string Value { get; set; }
    public TokenType Type { get; set; }
}

public class Word : Token
{
    public Word(string value)
    {
        Value = value;
        Type = TokenType.Word;
    }
    public Word() { }
}

public class Punctuation : Token
{
    public Punctuation(string value)
    {
        Value = value;
        Type = TokenType.Punctuation;
    }
    public Punctuation() { }
}

public class Sentence
{
    public List<Token> Tokens { get; set; } = new List<Token>();
    
    public int WordCount => Tokens.Count(t => t.Type == TokenType.Word);
    public string Text => string.Join("", Tokens.Select(t => t.Value));
}

[XmlRoot("Text")]
public class Text
{
    public List<Sentence> Sentences { get; set; } = new List<Sentence>();
}

public class ConcordanceEntry
{
    public string Word { get; set; }
    public int Frequency { get; set; }
    public Dictionary<int, List<int>> Positions { get; set; } = new();
}