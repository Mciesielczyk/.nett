namespace ConsoleApp2;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Vehicle vehicle = new Motorcycle(1, "ag", "ag", 1, true);
        vehicle.DisplayInfo();
    }
}