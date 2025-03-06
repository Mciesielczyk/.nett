public struct TextStatistics
{
    public int CharacterCount { get; set; } 
    public int CharacterCountWithoutSpaces { get; set; }
    public int LetterCount { get; set; }
    public int DigitCount { get; set; }
    public int PunctuationCount { get; set; }
    public int WordCount { get; set; }
    public int SentenceCount { get; set; }
    public string MostCommonWord { get; set; }
    public double AverageWordLength { get; set; }
    public string ShortestWord { get; set; }
    public string LongestWord { get; set; }
    public double AverageWords { get; set; }
    public string LongestSentence { get; set; }
    public int UniqueWords { get; set; }

    public override string ToString()
    {
        return $"Statystyki tekstu:\n" +
               $"Liczba znaków: {CharacterCount}\n" +
               $"Liczba znaków bez spacji: {CharacterCountWithoutSpaces}\n" +
               $"Liczba liter: {LetterCount}\n" +
               $"Liczba cyfr: {DigitCount}\n" +
               $"Liczba znaków interpunkcyjnych: {PunctuationCount}\n" +
               $"Liczba słów: {WordCount}\n" +
               $"Liczba zdań: {SentenceCount}\n" +
               $"Najczęściej występujące słowo: {MostCommonWord}\n" +
               $"Średnia długość słowa: {AverageWordLength:F2}\n" + 
               $"Najkrótsze słowo: {ShortestWord}\n" +
               $"Najdłuższe słowo: {LongestWord}\n" +
               $"Średnia liczba słów w zdaniu: {AverageWords:F2}\n" + 
               $"Najdłuższe zdanie: {LongestSentence}\n" +
               $"Liczba unikalnych słów: {UniqueWords}";
    }
}