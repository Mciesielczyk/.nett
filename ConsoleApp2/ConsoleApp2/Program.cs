namespace ConsoleApp2;

class Program
{
    static void Main(string[] args)
    {
        
     
        
        Console.WriteLine("Hello, World!");
        Vehicle vehicle1 = new Motorcycle(1, "ag", "ag", 1, true,7);
        Vehicle vehicle2 = new Motorcycle(2, "sdsa", "asda", 22, false,88);
        Vehicle vehicle3 = new Motorcycle(3, "vvvsdsa", "vvasda", 22, true,88);
        Vehicle vehicle4 = new Motorcycle(4, "dddsdsa", "aasdasda", 22, true,88);

        vehicle1.DisplayInfo();
        RentalCompany company = RentalCompany.Instance;
        company.OnNewReservation += (message) =>
        {
            Console.WriteLine(message);  
        };
        company.AddVehicle(vehicle1);
        company.AddVehicle(vehicle1);
        company.AddVehicle(vehicle2);
        company.AddVehicle(vehicle3);
        company.AddVehicle(vehicle4);

        //company.ReserveVehicle(1,"dupa");
        
        List<Vehicle> vehiclesA = new List<Vehicle>();
        vehiclesA = company.vehicles;
        vehiclesA = vehiclesA.GetAvailableVehicles();
        
        foreach (Vehicle vehicle in vehiclesA)
        {
            vehicle.DisplayInfo();
        }
        
        //vehicle1.Reserve("adam");
        
        company.ListAvailableVehicles();
        vehicle1.Reserve("a");
        vehicle1.Reserve("a");
        vehicle1.Reserve("a");
        vehicle1.Reserve("b");
        vehicle2.Reserve("b");
        vehicle3.Reserve("b");
        vehicle4.Reserve("b");

        company.displayReservations();
        vehicle4.CancelReserve();
        Console.WriteLine("-----------------------");
        company.displayReservations();


    }
}