namespace ConsoleApp2;

public class Motorcycle : Vehicle,IReservable
{
    RentalCompany company = RentalCompany.Instance;

    private int EngineCapacity;
    public bool isAvailable { get; set; }

    public Motorcycle(int id, string brand, string model, int yearOfProduction, bool isAvailable, int engineCapacity):  
        base(id, brand, model, yearOfProduction, isAvailable)
    {
        EngineCapacity=engineCapacity;
    }

    public override void DisplayInfo()
    {
        string Model = getModel();
        string Brand = getBrand();
        int YearOfProduction = getYear();

        Console.WriteLine($"Vehicle Info: {Brand} {Model} ({YearOfProduction}), Available: {getIsAvailable()}");


    }


    public override void Reserve(string customer)
    {
        RentalCompany company = RentalCompany.Instance;
        company.ReserveVehicle(this.getId(),customer);
    }

    public  override void CancelReserve()
    {
        ChangeAvailability();
        company.CancelReservation(this.getId());
    }

    public bool IsAvailable()
    {
        return this.isAvailable;
    }


    public void ChangeAvailability()
    {
        isAvailable = !isAvailable;

    }
}