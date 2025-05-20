using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System_Zarz.Data;
using System_Zarz.Models;

namespace System_Zarz.Pages.Customers;

[Authorize(Roles = "Admin,Recepcjonista")]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Customer Customer { get; set; } = new();

    public bool SuccessMessage { get; set; } = false;
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ErrorMessage = "❌ Wystąpił błąd. Upewnij się, że wszystkie pola są poprawnie wypełnione.";
            return Page();
        }

        try
        {
            _context.Customers.Add(Customer);
            await _context.SaveChangesAsync();

            SuccessMessage = true;
            ModelState.Clear();
            Customer = new();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"❌ Wystąpił błąd przy zapisie: {ex.Message}";
        }

        return Page();
    }
}