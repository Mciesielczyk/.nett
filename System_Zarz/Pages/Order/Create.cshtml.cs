using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using Task = System.Threading.Tasks.Task;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    [BindProperty]
    public List<string> selectedMechanics { get; set; }

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Order Order { get; set; }

    public SelectList Vehicles { get; set; }
    public MultiSelectList Mechanics { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        Vehicles = new SelectList(await _context.Vehicles.Include(v => v.Customer).ToListAsync(),
            "Id", "RegistrationNumber"); // tu pokazujemy numer rejestracji

        var mechanicRoleId = (await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Mechanik"))?.Id;
        var mechanics = await _context.Users
            .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == mechanicRoleId))
            .ToListAsync();

        Mechanics = new MultiSelectList(mechanics, "Id", "UserName");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(List<string> selectedMechanics)
    {
        if (!ModelState.IsValid)
        {
            // Załaduj dropdowny ponownie, żeby formularz się poprawnie wyświetlił
            await LoadDropdownsAsync();
            return Page();
        }

        Order.Status = "Nowe";
        Order.CreatedDate = DateTime.Now;

        _context.Orders.Add(Order);

        try
        {
            await _context.SaveChangesAsync();

            foreach (var mechanicId in selectedMechanics)
            {
                _context.OrderMechanics.Add(new OrderMechanic
                {
                    OrderId = Order.Id,
                    MechanicId = mechanicId
                });
            }
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Zlecenie zostało utworzone pomyślnie.";
            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Wystąpił błąd podczas zapisu: " + ex.Message);
            await LoadDropdownsAsync();
            return Page();
        }
    }

// Metoda pomocnicza do ładowania dropdownów
    private async Task LoadDropdownsAsync()
    {
        var vehicles = await _context.Vehicles.Include(v => v.Customer).ToListAsync();
        Vehicles = new SelectList(vehicles.Select(v => new
        {
            v.Id,
            Label = v.Brand + " " + v.Model + " (" + v.RegistrationNumber + ")"
        }), "Id", "Label");

        var mechanicRoleId = (await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Mechanik"))?.Id;
        var mechanics = await _context.Users
            .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == mechanicRoleId))
            .ToListAsync();

        Mechanics = new MultiSelectList(mechanics, "Id", "UserName");
    }

}
