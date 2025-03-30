namespace ConsoleApp2;

public abstract class Vehicle
{
    private int Id;
    private string Model;
    private string Brand;
    private int Year;
    public bool IsAvailable;
    public Vehicle(int id, string brand, string model, int year, bool isAvailable)
    {
        Id = id;
        Brand = brand;
        Model = model;
        Year = year;
        IsAvailable = isAvailable;
    }

    protected Vehicle()
    {
        throw new NotImplementedException();
    }

    public abstract void DisplayInfo();

}