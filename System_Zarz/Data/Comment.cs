using Microsoft.AspNetCore.Identity;

namespace System_Zarz.Data;

public class Comment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    
    public string Text { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
