namespace ConsoleApp2;

public static class VehicleExtensions
{
    public static List<Vehicle> GetAvailableVehicles(this List<Vehicle> vehicles)
    {
    return vehicles.Where(v => v.IsAvailable).ToList();      
    }
}