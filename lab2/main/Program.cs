using System.Globalization;

namespace lab2;

public class Program
{
    public  static void Main(string[] args)
    { 
        var text = "";
        if (args.Length > 0) // Sprawdzamy, czy podano argumenty
        {
            string path2 = args[0]; // Pobieramy ścieżkę do pliku

            if (File.Exists(path2)) // Sprawdzamy, czy plik istnieje
            {
                string[] lines = File.ReadAllLines(path2);
                foreach (string line in lines)
                {
                    text += line + Environment.NewLine;
                }
            }
            else
            {
                Console.WriteLine($"Błąd: Plik '{path2}' nie istnieje.");
                return ;
            }
        }
        else
        {
            Console.WriteLine("Co chcesz wybrac?");
            Console.WriteLine("1) Analiza tekstu od uzytkownika");
            Console.WriteLine("2) Analiza tekstu z pliku (bez ciezki)");
            Console.WriteLine("3) Analiza tekstu z pliku");
            var option = Console.ReadLine();
            //Console.WriteLine(option);
            // Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.WriteLine("Podaj tekst");
                     text = Console.ReadLine();
                  
                    break;
                case "2":
                    Console.WriteLine("Podaj sciezke");
                    string path1 =Console.ReadLine();
                    if (File.Exists(path1)) // Sprawdzamy, czy plik istnieje
                    {
                        string[] lines = File.ReadAllLines(path1);
                        foreach (string line in lines)
                        {
                            text += line + Environment.NewLine;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Błąd: Plik '{path1}' nie istnieje.");
                        return;
                    }
                    break;

                case "3":
                    string path = @"C:\Users\ciesi\Desktop\Lato\.net\lab2\.nett\lab2\main\text.txt";
                    string[] linie = File.ReadAllLines(path);
                    foreach (var line in linie)
                    {
                        Console.WriteLine(line);
                        text += line + Environment.NewLine; 
                    }

              
                    break;

                default:
                    break;
            }
            
        }
        TextAnalyzer analyzer = new TextAnalyzer();
        TextStatistics stats = analyzer.Analyze(text); 
        Console.WriteLine(stats);
        
    }
}