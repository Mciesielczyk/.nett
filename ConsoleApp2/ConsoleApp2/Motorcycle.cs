namespace ConsoleApp2;

public class Motorcycle : Vehicle,IReservable
{
    public Motorcycle(int id, string brand, string model, int yearOfProduction, bool isAvailable):  
        base(id, brand, model, yearOfProduction, isAvailable)
    {

    }

    public override void DisplayInfo()
    {
        Console.WriteLine("AAA");
    }


    public void Reserve(string customer)
    {
        throw new NotImplementedException();
    }

    public void CancelReserve()
    {
        throw new NotImplementedException();
    }

    public bool IsAvailable()
    {
        return base.IsAvailable;
    }
}