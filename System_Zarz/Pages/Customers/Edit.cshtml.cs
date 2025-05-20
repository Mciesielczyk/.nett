using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System_Zarz.Models;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Customers;

[Authorize(Roles = "Admin,Recepcjonista")]

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Customer Customer { get; set; } = new();

    public List<Customer> AllCustomers { get; set; } = new();
    public string? Message { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SearchId { get; set; }

    public async Task OnGetAsync()
    {
        AllCustomers = await _context.Customers.ToListAsync();

        if (SearchId.HasValue)
        {
            var customer = await _context.Customers.FindAsync(SearchId.Value);
            if (customer != null)
            {
                Customer = customer;
                Message = null;
            }
            else
            {
                Message = $"❌ Nie znaleziono klienta o ID {SearchId}";
            }
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            AllCustomers = await _context.Customers.ToListAsync();
            return Page();
        }

        var customerToUpdate = await _context.Customers.FindAsync(Customer.Id);
        if (customerToUpdate == null)
        {
            Message = "❌ Nie znaleziono klienta do edycji.";
            AllCustomers = await _context.Customers.ToListAsync();
            return Page();
        }

        customerToUpdate.FullName = Customer.FullName;
        customerToUpdate.Email = Customer.Email;
        customerToUpdate.Phone = Customer.Phone;

        await _context.SaveChangesAsync();
        Message = "✅ Dane klienta zostały zaktualizowane.";

        AllCustomers = await _context.Customers.ToListAsync();
        return Page();
    }
}
