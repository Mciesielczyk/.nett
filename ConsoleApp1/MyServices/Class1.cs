namespace ILoggerService;


public interface ILoggerService
{
    void Log(string message);
}

public class ConsoleLogger : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}