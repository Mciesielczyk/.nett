using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using Task = System.Threading.Tasks.Task;

public class OrdersListModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public OrdersListModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Order> Orders { get; set; }

    public async Task OnGetAsync()
    {
        Orders = await _context.Orders
            .Include(o => o.Vehicle)
            .Include(o => o.Mechanics).ThenInclude(om => om.Mechanic)
            .Include(o => o.Tasks)
            .Include(o => o.Parts)
            .Include(o => o.Comments)
            .ToListAsync();
    }
}