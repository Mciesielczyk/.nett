using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System_Zarz.Data;
using System_Zarz.Models;

namespace System_Zarz.Pages.Customers;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Customer Customer { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        _context.Customers.Add(Customer);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Index"); // przekieruj po zapisaniu
    }
}