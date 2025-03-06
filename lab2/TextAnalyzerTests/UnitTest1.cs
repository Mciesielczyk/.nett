using NUnit.Framework;
using System;
using System.IO;
using System.Diagnostics;
using lab2;

namespace TextAnalyzerTests
{
    [TestFixture]
    public class Tests
    {
        private string testFilePath;

        [SetUp]
        public void Setup()
        {
            //  plik testowy
            testFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void Cleanup()
        {
            // Usuwanie pliku po teście
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        [Test]
        public void Test_FileExistsAndNotEmpty()
        {
            // Arrange
            File.WriteAllText(testFilePath, "To jest testowy tekst.");

            // Act
            string text = File.ReadAllText(testFilePath);

            // Assert
            Assert.IsNotEmpty(text, "Plik nie powinien być pusty.");
            Assert.AreEqual("To jest testowy tekst.", text);
        }

    

        [Test]
        public void Test_EmptyUserInput()
        {
            // Arrange
            string userInput = "";

            // Act & Assert
            Assert.IsTrue(string.IsNullOrWhiteSpace(userInput), "Wejście użytkownika powinno być traktowane jako puste.");
        }

        [Test]
        public void Test_TextAnalyzer_WordCount()
        {
            // Arrange
            var analyzer = new TextAnalyzer();
            string sampleText = "C# jest super!";
            
            // Act
            TextStatistics stats = analyzer.Analyze(sampleText);

            // Assert
            Assert.AreEqual(3, stats.WordCount, "Analizator powinien policzyć 3 słowa.");
        }
        
        [Test]
        public void CountCharacters_ShouldReturnCorrectNumber()
        {
            var text = "Hello, world!";
            var analyzer = new TextAnalyzer();
            TextStatistics stats  = analyzer.Analyze(text);
            Assert.AreEqual(13, stats.CharacterCount);
        }
        
        
        
        [Test]
        public void CountWords_ShouldReturnCorrectNumber()
        {
            var text = "Hello world!";
            var analyzer = new TextAnalyzer();
            TextStatistics stats  = analyzer.Analyze(text);
            Assert.AreEqual(2, stats.WordCount);
        }
        
        [Test]
        public void CountSentences_ShouldReturnCorrectNumber()
        {
            var text = "Hello world! How are you? I am fine.";
            var analyzer = new TextAnalyzer();
            TextStatistics stats  = analyzer.Analyze(text);
            Assert.AreEqual(3, stats.SentenceCount);
        }

        [Test]
        public void MostCommonWord_ShouldReturnCorrectWord()
        {
            var text = "apple banana apple orange apple banana";
            var analyzer = new TextAnalyzer();
            TextStatistics stats  = analyzer.Analyze(text);
            Assert.AreEqual("apple", stats.MostCommonWord);
        }

        [Test]
        public void AnalyzeText_WithEmptyString_ShouldReturnZeroes()
        {
            var text = "";
            var analyzer = new TextAnalyzer();
            TextStatistics stats  = analyzer.Analyze(text);
        
            Assert.AreEqual(0, stats.CharacterCount);
            Assert.AreEqual(0, stats.WordCount);
            Assert.AreEqual(0, stats.SentenceCount);
        }
        [Test]
        public void Test_FileDoesNotExist()
        {
            // Arrange
            string fakePath = Path.Combine(Path.GetTempPath(), "fakefile.txt");  // Ścieżka do pliku, który nie istnieje

            // Ustawienie do przechwytywania wyjścia konsoli
            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Uruchamiamy aplikację z nieistniejącym plikiem jako argumentem
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet", // Używamy dotnet do uruchomienia aplikacji
                    Arguments = $"run {fakePath}", // Przekazujemy nieistniejący plik jako argument
                    RedirectStandardOutput = true, // Przechwytujemy standardowe wyjście
                    UseShellExecute = false // Musimy to ustawić na false, aby przechwytywać wyjście
                };

                // Uruchamiamy proces
                var process = Process.Start(processStartInfo);
                process.WaitForExit(); // Czekamy na zakończenie procesu

                // Act
                string output = sw.ToString(); // Zbieramy wyjście aplikacji

                // Assert
                Assert.IsFalse(output.Contains($"Błąd: Plik '{fakePath}' nie istnieje."), "Program nie wyświetlił oczekiwanego komunikatu o błędzie.");
            }
        }
    }
}
