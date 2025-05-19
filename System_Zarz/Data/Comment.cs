using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace System_Zarz.Data;

public class Comment
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Zlecenie jest wymagane.")]

    public int OrderId { get; set; }
    public Order Order { get; set; }
    [Required(ErrorMessage = "Tekst komentarza jest wymagany.")]
    [StringLength(500)]

    public string Text { get; set; } 
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
