using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System_Zarz.Data;
using Task = System.Threading.Tasks.Task;

public class AssignTasksModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public AssignTasksModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public int OrderId { get; set; }

    [BindProperty]
    public List<int> SelectedTaskIds { get; set; } = new();

    public SelectList Orders { get; set; }
    public MultiSelectList AllTasks { get; set; }

    public async Task OnGetAsync()
    {
        await LoadSelectsAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (OrderId == 0 || !SelectedTaskIds.Any())
        {
            ModelState.AddModelError(string.Empty, "Musisz wybrać zlecenie i co najmniej jedną czynność.");
            await LoadSelectsAsync();
            return Page();
        }

        foreach (var taskId in SelectedTaskIds)
        {
            bool exists = await _context.OrderTasks.AnyAsync(ot => ot.OrderId == OrderId && ot.TaskId == taskId);
            if (!exists)
            {
                _context.OrderTasks.Add(new OrderTask
                {
                    OrderId = OrderId,
                    TaskId = taskId
                });
            }
        }

        // TempData["SuccessMessage"] = "Czynności zostały przypisane.";
        Message = "Czynności zostały przypisane."; // nowa właściwość w modelu strony

        await LoadSelectsAsync();

        return Page(); // nie przekierowujemy, zostajemy na stronie
    }

    private async Task LoadSelectsAsync()
    {
        Orders = new SelectList(await _context.Orders
            .Include(o => o.Vehicle)
            .Select(o => new
            {
                o.Id,
                Label = $"#{o.Id} - {o.Vehicle.RegistrationNumber}"
            })
            .ToListAsync(), "Id", "Label");

        AllTasks = new MultiSelectList(await _context.Tasks.ToListAsync(), "Id", "Description");
    }
    [TempData]
    public string? Message { get; set; }

}
