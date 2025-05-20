using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Comments;

[Authorize(Roles = "Admin,Recepcjonista")]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    [TempData]
    public string? StatusMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public List<Comment> Comments { get; set; } = new();

    [BindProperty]
    public Comment NewComment { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadCommentsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            ErrorMessage = "Nie znaleziono użytkownika.";
            await LoadCommentsAsync();
            return Page();
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => $"{e.Key}: {string.Join(", ", e.Value.Errors.Select(er => er.ErrorMessage))}");

            ErrorMessage = "Formularz zawiera błędy: " + string.Join(" | ", errors);
            await LoadCommentsAsync();
            return Page();
        }


        var orderExists = await _context.Orders.AnyAsync(o => o.Id == NewComment.OrderId);


        NewComment.UserId = user.Id;
        NewComment.CreatedAt = DateTime.Now;

        _context.Comments.Add(NewComment);
        await _context.SaveChangesAsync();

        StatusMessage = "Komentarz został dodany pomyślnie.";
        return RedirectToPage(); // reload z komunikatem
    }


    private async Task LoadCommentsAsync()
    {
        Comments = await _context.Comments
            .Include(c => c.User)
            .Include(c => c.Order)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}