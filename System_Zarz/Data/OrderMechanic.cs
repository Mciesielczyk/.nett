using Microsoft.AspNetCore.Identity;

namespace System_Zarz.Data;

public class OrderMechanic
{
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    
    public string MechanicId { get; set; } = null!; // IdentityUser.Id
    public IdentityUser Mechanic { get; set; } = null!;
}
