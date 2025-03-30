namespace ConsoleApp2;

public class Reservation
{
    private  int ReservationID;
    private  Vehicle ReservationVehicle;
    private string Customer;
    private DateTime Date;

    Reservation(int ReservationID, Vehicle ReservationVehicle, string Customer, DateTime Date)
    {
        this.ReservationID = ReservationID;
        this.ReservationVehicle = ReservationVehicle;
        this.Customer = Customer;
        this.Date = Date;
        
    }

    public override string ToString()
    {
        return base.ToString();
    }
}