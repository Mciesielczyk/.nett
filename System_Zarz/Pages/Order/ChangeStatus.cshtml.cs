using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using Task = System.Threading.Tasks.Task;

public class ChangeStatusModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ChangeStatusModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public int SelectedOrderId { get; set; }

    [BindProperty]
    public string NewStatus { get; set; }

    public SelectList Orders { get; set; }

    public string Message { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadOrdersAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadOrdersAsync();

        if (SelectedOrderId == 0 || string.IsNullOrEmpty(NewStatus))
        {
            ModelState.AddModelError("", "Wybierz zlecenie oraz status.");
            return Page();
        }

        var order = await _context.Orders.FindAsync(SelectedOrderId);
        if (order == null)
        {
            ModelState.AddModelError("", "Zlecenie nie istnieje.");
            return Page();
        }

        order.Status = NewStatus;
        await _context.SaveChangesAsync();

        Message = "Status został pomyślnie zmieniony.";
        return Page();
    }

    private async Task LoadOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Vehicle)
            .ToListAsync();

        Orders = new SelectList(orders.Select(o => new
        {
            o.Id,
            Label = $"#{o.Id} - {o.Vehicle?.RegistrationNumber ?? "Brak pojazdu"} - {o.Description.Substring(0, Math.Min(30, o.Description.Length))}"
        }), "Id", "Label");
    }
}