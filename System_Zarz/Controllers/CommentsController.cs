using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize] // tylko dla zalogowanych
public class CommentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CommentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/comments?orderId=123  - pobierz komentarze z konkretnego zlecenia
    [HttpGet]
    public async Task<IActionResult> GetComments(int? orderId)
    {
        var query = _context.Comments.Include(c => c.User).AsQueryable();

        if (orderId.HasValue)
            query = query.Where(c => c.OrderId == orderId.Value);

        var comments = await query
            .OrderBy(c => c.CreatedAt)
            .Select(c => new 
            {
                c.Id,
                c.Text,
                Author = c.User.UserName,
                c.CreatedAt,
                c.OrderId
            }).ToListAsync();

        return Ok(comments);
    }

    // POST: api/comments  - dodaj komentarz
    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] CommentCreateModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var comment = new Comment
        {
            OrderId = model.OrderId,
            Text = model.Text,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComments), new { orderId = comment.OrderId }, comment);
    }

    public class CommentCreateModel
    {
        public int OrderId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
