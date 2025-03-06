namespace lab2;

public class TextAnalyzer
{
    public TextStatistics Analyze(string text)
    {
        TextStatistics stat = new TextStatistics();

        if (string.IsNullOrWhiteSpace(text))
        {
            stat.CharacterCount = 0;
            stat.CharacterCountWithoutSpaces = 0;
            stat.LetterCount = 0;
            stat.DigitCount = 0;
            stat.PunctuationCount = 0;
            stat.WordCount = 0;
            stat.UniqueWords = 0;
            stat.MostCommonWord = "";
            stat.AverageWordLength = 0;
            stat.ShortestWord = "";
            stat.LongestWord = "";
            stat.SentenceCount = 0;
            stat.AverageWords = 0;
            stat.LongestSentence = "";

            return stat;
        }
        
        stat.CharacterCount = text.Length;
        stat.CharacterCountWithoutSpaces = text.Replace(" ","").Length;
        stat.LetterCount = text.Count(char.IsLetter);
        stat.DigitCount = text.Count(char.IsDigit);
        stat.PunctuationCount = text.Count(char.IsPunctuation);
        stat.WordCount = text.Split(' ').Length;
        
        string[] words = text.Split(new char[] { ' ', '\n', '\t', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        HashSet<string> uniqueWordsSet  = new HashSet<string>(words.Select(w => w.ToLower()));
        int uniqueWordsCount = uniqueWordsSet.Count;
        stat.UniqueWords = uniqueWordsCount;

        stat.MostCommonWord = CountWords(words);
        stat.AverageWordLength = Avg(words);

        stat.ShortestWord = FindShoertest(words);
        stat.LongestWord = FindLongestWord(words);

        char[] sentence = { '.', '!', '?' };
        stat.SentenceCount = text.Count(znak => sentence.Contains(znak));
        if (stat.SentenceCount == 0) stat.SentenceCount = 1;
        stat.AverageWords = AvgW(text)/ stat.SentenceCount;
        
        stat.LongestSentence = FindLongestSentence(text);
        //wypisz(words);
        return stat;
        
    }

    void wypisz(string [] words)
    {
        foreach (var word in words)
        {
            Console.WriteLine(word);
        }
    }
    string FindLongestSentence(string text)
    {
        int Words = 0;
        int max = 0;
        string zdanie = null;
        string zdanieMax= " ";

        foreach (char lit in text)
        {
            zdanie=zdanie+lit.ToString();
            if (lit == ' ') Words++;
            if (lit == '.' || lit == '!' || lit == '?')
            {
                Words++;
                if (Words > max)
                {
                    max = Words;
                    zdanieMax=zdanie;
                }

                zdanie = null;
                Words = 0;
            }
        }
        return zdanieMax;
    }
    int AvgW(string text)
    {
        int Words = 0;
        int sum = 0;
        foreach (char lit in text)
        {
            if (lit == ' ') Words++;
            if (lit == '.' || lit == '!' || lit == '?')
            {
                Words++;
                sum = sum + Words;
                Words = 0;
            }
        }
        return sum;
    }
    string FindShoertest(string[] words)
    {
        string shoertest = words[0];
        foreach (var wor in words)
        {
            if (shoertest.Length > wor.Length)
            {
                shoertest = wor;
            }
        }
        return shoertest;
    }
    
    string FindLongestWord(string[] words)
    {
        string longest = words[0];
        foreach (var wor in words)
        {
            if (longest.Length < wor.Length)
            {
                longest = wor;
            }
        }
        return longest;
    }
    double Avg(string[] words)
    {
        double average = 0;
        foreach (var woord in words)
        {
            average = woord.Length + average;
        }
        average /= words.Length;
        return average;
    }
     public string  CountWords(string[] words)
    {
        Dictionary<string, int> wordCount = new Dictionary<string, int>();
        foreach (var word in words)
        {
            string wordLower = word.ToLower();
            if (wordCount.ContainsKey(wordLower))
            {
                wordCount[wordLower]++;
            }
            else
            {
                wordCount[wordLower] = 1;
            }
        }

        string najczestszeSlowo = null;
        int maxLiczbaWystapien = 0;
        
        foreach (KeyValuePair<string,int> para in wordCount)
        {
            if (para.Value > maxLiczbaWystapien)
            {
                maxLiczbaWystapien = para.Value;
                najczestszeSlowo = para.Key;
            }
        }
        return najczestszeSlowo;
    }
    
}