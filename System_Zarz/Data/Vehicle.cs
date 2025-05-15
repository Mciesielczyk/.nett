using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System_Zarz.Data;

public class Vehicle
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    [ValidateNever]
    public Customer Customer { get; set; }
    
    public string VIN { get; set; } = null!;
    public string RegistrationNumber { get; set; } = null!;
    public string? ImagePath { get; set; } // ścieżka do zdjęcia pojazdu
    
    public List<Order> Orders { get; set; } = new();
}