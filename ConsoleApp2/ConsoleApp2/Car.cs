namespace ConsoleApp2;

public class Car : Vehicle,IReservable
{
    RentalCompany company = RentalCompany.Instance;
    public bool isAvailable { get; set; }
    private string BodyType { get; set; }
    public Car(int id, string brand, string model, int yearOfProduction, bool isAvailable, string bodyType):  
        base(id, brand, model, yearOfProduction, isAvailable)
    {
        BodyType=bodyType;
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