using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System.Security.Claims;
using System_Zarz.Mappers;
using System_Zarz.DTOs;

[Route("api/[controller]")]
[ApiController]
[Authorize] // tylko dla zalogowanych
public class CommentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly CommentMapper _mapper;

    public CommentsController(ApplicationDbContext context, CommentMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
            .ToListAsync();
        var commentDtos = comments.Select(c => _mapper.ToDto(c));
        return Ok(commentDtos);
    }

    // POST: api/comments  - dodaj komentarz
    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] CommentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        var comment = new Comment
        {
            OrderId = dto.OrderId,
            Text = dto.Text,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetComments), new { orderId = comment.OrderId }, comment);
    }
}
