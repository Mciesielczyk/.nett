using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace System_Zarz.Data;

public class Order
{
    public int Id { get; set; }
   // [Required(ErrorMessage = "Pojazd jest wymagany.")]
    public int VehicleId { get; set; }
    [ValidateNever] 
    public Vehicle Vehicle { get; set; } 

    public DateTime CreatedDate { get; set; }

    public string Status { get; set; } = "Nowe";  // np. "Nowe", "W trakcie", "Zakończone"

    [Required(ErrorMessage = "Opis problemu jest wymagany.")]
    [StringLength(1000, ErrorMessage = "Opis nie może przekraczać 1000 znaków.")]
    public string Description { get; set; } = string.Empty;

    public List<OrderMechanic> Mechanics { get; set; } = new();
    public List<OrderTask> Tasks { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<OrderPart> Parts { get; set; } = new();
    
    public List<OrderTask> OrderTasks { get; set; } = new();
    public List<OrderPart> OrderParts { get; set; } = new();
}