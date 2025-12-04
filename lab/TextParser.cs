using System.Text.RegularExpressions;

public static class TextParser
{
    public static Text ParseText(string input)
    {
        var text = new Text();
        var sentences = Regex.Split(input, @"(?<=[.!?])\s+");
        
        foreach (var sentence in sentences)
        {
            if (string.IsNullOrWhiteSpace(sentence)) continue;
            
            var tokens = new List<Token>();
            var words = Regex.Split(sentence, @"(\W+)");
            
            foreach (var item in words)
            {
                if (string.IsNullOrWhiteSpace(item)) continue;
                
                if (Regex.IsMatch(item, @"\w+"))
                    tokens.Add(new Word(item));
                else
                    tokens.Add(new Punctuation(item));
            }
            
            text.Sentences.Add(new Sentence { Tokens = tokens });
        }
        
        return text;
    }
}