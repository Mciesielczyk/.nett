using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace lab12
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string[] urls = {
                "https://www.gutenberg.org/files/84/84-0.txt",     // Frankenstein
                "https://www.gutenberg.org/files/11/11-0.txt",     // Alice
                "https://www.gutenberg.org/files/1661/1661-0.txt", // Sherlock
                "https://www.gutenberg.org/files/2701/2701-0.txt"  // Moby Dick
            };

            var stopwatchPobieranie = Stopwatch.StartNew();

            Task<string>[] zadaniaPobrania = urls.Select(Pobierz).ToArray();
            string[] teksty = await Task.WhenAll(zadaniaPobrania);

            stopwatchPobieranie.Stop();
            Console.WriteLine($"Czas pobierania: {stopwatchPobieranie.ElapsedMilliseconds} ms\n");

            var stopwatchPrzetwarzanie = Stopwatch.StartNew();

            var wspolnySlownik = new ConcurrentDictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            Parallel.ForEach(teksty, tekst =>
            {
                string[] slowa = tekst.Split(new[] { ' ', '\n', '\r', '\t', '.', ',', ';', ':', '!', '?', '-', '"', '(', ')', '[', ']', '*', '_', '\'' },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var slowo in slowa)
                {
                    string s = slowo.ToLowerInvariant();
                    wspolnySlownik.AddOrUpdate(s, 1, (_, stare) => stare + 1);
                }
            });

            stopwatchPrzetwarzanie.Stop();
            Console.WriteLine($" Czas przetwarzania: {stopwatchPrzetwarzanie.ElapsedMilliseconds} ms\n");

            Console.WriteLine(" 10 najczęstszych słów:");
            foreach (var para in wspolnySlownik.OrderByDescending(p => p.Value).Take(10))
            {
                Console.WriteLine($"{para.Key} : {para.Value}");
            }
        }

        static async Task<string> Pobierz(string url)
        {
            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
