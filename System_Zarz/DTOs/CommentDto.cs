using Microsoft.AspNetCore.Identity;

namespace System_Zarz.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public IdentityUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    }
}