namespace ConsoleApp2;

public class RentalCompany
{
    public List<Vehicle> vehicles = new List<Vehicle>();
    public List<Reservation> reservations = new List<Reservation>();
    public event Action<string> OnNewReservation;

    private static RentalCompany? _instance;
    private RentalCompany() { }

    public static RentalCompany Instance
    {
        get
        {
            if (_instance == null)
                {
                    _instance = new RentalCompany();
                }
                return _instance;
            
        }
    }
    public void AddVehicle(Vehicle vehicle)
    {
        this.vehicles.Add(vehicle);
    }

    public void ReserveVehicle(int vehicleId, string customer)
    {

        foreach (var vehicle in vehicles)
        {
            if(vehicle.getId()==vehicleId && vehicle.IsAvailable)
            {
                Reservation reservation = new Reservation( vehicle, customer,DateTime.Now);
                reservations.Add(reservation);
                vehicle.IsAvailable = false;
                
                OnNewReservation?.Invoke($"Rezerwacja dokonana przez {customer}.");

            }
        }
    }

    public void displayReservations()
    {
        foreach (var va in reservations)
        {
            va.displayReservation();
        }
    }

    public int getResID(Vehicle vehicle)
    {
        foreach (var res in reservations)
        {
            if  (res.GetVehicle().getId()==vehicle.getId())
            {
                return res.getReservationID();
            }
        }
        return -1;
    }
    
    public void CancelReservation(int vecId)
    {
        foreach (var vec in vehicles)
        {
            if (vec.getId()==vecId)
            {
                int a = getResID(vec);
                
                Reservation reservation = reservations.Find(res => res.getReservationID() == a);
                reservations.Remove(reservation);
            }
        }
    }
    public List<Vehicle> GetAvailableVehicles()
    {
        return vehicles.Where(v => v.IsAvailable).ToList(); 
    }
    public void ListAvailableVehicles()
    {
        var availableVehicles = GetAvailableVehicles(); 
        if (availableVehicles.Any()) 
        {
            foreach (var vehicle in availableVehicles)
            {
                vehicle.DisplayInfo();  
            }
        }
        else
        {
            Console.WriteLine("Brak dostępnych pojazdów.");
        }
    }
}