namespace System_Zarz.Data;

public class Order
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = null!;  // np. "Nowe", "W trakcie", "Zakończone"
    
    public List<OrderMechanic> Mechanics { get; set; } = new(); // przypisani mechanicy (wiele do wielu)
    public List<OrderTask> Tasks { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<OrderPart> Parts { get; set; } = new();
}
