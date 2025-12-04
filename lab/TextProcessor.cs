using System.Collections.Generic;
using System.Linq;

public static class TextProcessor
{
    public static List<Sentence> SortByWordCount(Text text) =>
        text.Sentences.OrderBy(s => s.WordCount).ToList();
    
    public static List<Sentence> SortByLength(Text text) =>
        text.Sentences.OrderBy(s => s.Text.Length).ToList();
    
    public static List<string> FindWordsInQuestions(Text text, int length)
    {
        var words = new HashSet<string>();
        foreach (var sentence in text.Sentences.Where(s => s.Text.EndsWith("?")))
        {
            words.UnionWith(sentence.Tokens
                .Where(t => t is Word w && w.Value.Length == length)
                .Select(t => t.Value));
        }
        return words.ToList();
    }
    
    public static void RemoveWordsStartingWithConsonant(Text text, int length)
    {
        var consonants = "бвгджзйклмнпрстфхцчшщBCDFGHJKLMNPQRSTVWXYZ";
        foreach (var sentence in text.Sentences)
        {
            sentence.Tokens.RemoveAll(t => t is Word w && 
                w.Value.Length == length && 
                consonants.Contains(char.ToUpper(w.Value[0])));
        }
    }
    
    public static void ReplaceWordsInSentence(Text text, int sentenceIndex, 
        int wordLength, string replacement)
    {
        if (sentenceIndex >= 0 && sentenceIndex < text.Sentences.Count)
        {
            var sentence = text.Sentences[sentenceIndex];
            for (int i = 0; i < sentence.Tokens.Count; i++)
            {
                if (sentence.Tokens[i] is Word w && w.Value.Length == wordLength)
                {
                    sentence.Tokens[i] = new Word(replacement);
                }
            }
        }
    }
    
    public static void ExportToXml(Text text, string filePath)
    {
        using var writer = new System.IO.StreamWriter(filePath);
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Text));
        serializer.Serialize(writer, text);
    }
    
    public static Dictionary<string, ConcordanceEntry> GenerateConcordance(Text text)
    {
        var concordance = new Dictionary<string, ConcordanceEntry>();
        
        for (int i = 0; i < text.Sentences.Count; i++)
        {
            var words = text.Sentences[i].Tokens
                .Where(t => t is Word)
                .Select(t => t.Value.ToLower());
            
            foreach (var word in words)
            {
                if (!concordance.ContainsKey(word))
                    concordance[word] = new ConcordanceEntry { Word = word };
                
                concordance[word].Frequency++;
                
                if (!concordance[word].Positions.ContainsKey(i + 1))
                    concordance[word].Positions[i + 1] = new List<int>();
                
                concordance[word].Positions[i + 1].Add(0);
            }
        }
        
        return concordance;
    }
    
    public static void PrintConcordance(Text text)
    {
        var concordance = GenerateConcordance(text);
        
        foreach (var entry in concordance.OrderBy(e => e.Key))
        {
            Console.WriteLine($"{entry.Key} [Частота: {entry.Value.Frequency}]");
            Console.Write("  Предложения: ");
            Console.WriteLine(string.Join(", ", entry.Value.Positions.Keys));
        }
    }
}