namespace ConsoleApp2;

public class RentalCompany
{
    List<Vehicle> vehicles = new List<Vehicle>();
    List<Reservation> reservations = new List<Reservation>();
    private event Action<string> OnNewReservation;

    RentalCompany(List<Vehicle> vehicles, List<Reservation> reservations)
    {
        this.vehicles = vehicles;
        this.reservations = reservations;
    }

    public void AddVehicle(Vehicle vehicle)
    {
        this.vehicles.Add(vehicle);
    }

    public void ReserveVehicle(int vehicleId, string customer)
    {
        vehicles.
    }
}