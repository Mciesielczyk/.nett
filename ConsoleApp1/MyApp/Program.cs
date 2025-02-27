using MyLibrary;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using ILoggerService;
namespace ConsoleApp1;

class Program
{
    static void Main(string[] args)
    {
        Calculator calc2 = new();
        //double sum = calc2.Add(1, 2);
        double extraction = calc2.Subtract(1, 2);
        //System.Console.WriteLine(sum);
        System.Console.WriteLine(extraction);
        
        
        double sum2 = Calculator.Add(5, 3);
        var result = new{ Operation = "Add", A=5,B=3,Result = sum2};
        string jsonResult = JsonConvert.SerializeObject(result,Formatting.Indented);
        Console.WriteLine(jsonResult);
        
        var serviceProvider = new ServiceCollection()
            .AddSingleton<ILoggerService.ILoggerService, ConsoleLogger>()
            .BuildServiceProvider();
        
        
        // Uzyskanie instancji loggera
        var logger = serviceProvider.GetService<ILoggerService.ILoggerService>();
        logger.Log("Aplikacja uruchomiona.");
        
        
        double sum = Calculator.Add(10, 15);
        logger.Log($"Wynik dodawania: {sum}");
    }
}