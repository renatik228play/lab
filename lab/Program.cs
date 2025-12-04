class Program
{
    static void Main()
    {
        StopWordsProcessor.LoadStopWords("stopwords_ru.txt");
        
        string input = "Привет, как дела? Я учу C#. Это интересно!";
        Text text = TextParser.ParseText(input);
        
        Console.WriteLine("КОНКОРДАНС ТЕКСТА:");
        TextProcessor.PrintConcordance(text);
        
        Console.WriteLine("\n1. Сортировка по количеству слов:");
        foreach (var s in TextProcessor.SortByWordCount(text))
            Console.WriteLine($"  {s.Text} ({s.WordCount} слов)");
        
        Console.WriteLine("\n3. Слова длиной 3 в вопросительных предложениях:");
        foreach (var word in TextProcessor.FindWordsInQuestions(text, 3))
            Console.WriteLine($"  {word}");
        
        StopWordsProcessor.RemoveStopWords(text);
        
        TextProcessor.ExportToXml(text, "output.xml");
        Console.WriteLine("\n7. Текст экспортирован в output.xml");
    }
}