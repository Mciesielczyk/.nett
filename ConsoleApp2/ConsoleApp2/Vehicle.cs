namespace ConsoleApp2;

public abstract class Vehicle
{
    RentalCompany company = RentalCompany.Instance;

    private int EngineCapacity;
    public bool isAvailable { get; set; }
    
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
    public abstract void Reserve(string name);
    
    public abstract void CancelReserve();

    public int getId()
    {
        return this.Id;
    }
   

    public string getModel()
    {
        return this.Model;
    }
    public string getBrand()
    {
        return this.Brand;
    }


    public int getYear()
    {
        return this.Year;
    }

    public bool getIsAvailable()
    {
        return this.IsAvailable;
    }
    
}