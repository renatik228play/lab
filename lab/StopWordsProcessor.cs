public static class StopWordsProcessor
{
    private static HashSet<string> stopWords = new HashSet<string>();
    
    public static void LoadStopWords(string filePath)
    {
        if (File.Exists(filePath))
            stopWords = new HashSet<string>(File.ReadAllLines(filePath), StringComparer.OrdinalIgnoreCase);
    }
    
    public static Text RemoveStopWords(Text text)
    {
        foreach (var sentence in text.Sentences)
        {
            sentence.Tokens.RemoveAll(t => 
                t is Word word && stopWords.Contains(word.Value));
        }
        return text;
    }
}