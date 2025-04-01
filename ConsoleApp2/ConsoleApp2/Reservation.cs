namespace ConsoleApp2;

public class Reservation
{
    private  int ReservationID;
    private  Vehicle ReservationVehicle;
    private string Customer;
    private DateTime Date;
    private static int id = 1;

   
    public Reservation(Vehicle ReservationVehicle, string Customer, DateTime Date)
    {
        this.ReservationID = id++;
        this.ReservationVehicle = ReservationVehicle;
        this.Customer = Customer;
        this.Date = Date;
        
    }

    public Vehicle GetVehicle()
    {
        return this.ReservationVehicle;
    }

    public int getReservationID()
    {
        return this.ReservationID;
    }
    public void displayReservation()
    {
        Console.WriteLine($"\nReservation ID: {ReservationID} \nCustomer: {Customer} \nDate: {Date}");
    }

    public void remoweReservation()
    {
    }
    public override string ToString()
    {
        return base.ToString();
    }
}