using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System_Zarz.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Tasks
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public System_Zarz.Data.Task NewTask { get; set; } = new();

        [BindProperty]
        public int SelectedPartId { get; set; }

        [BindProperty]
        public int PartQuantity { get; set; } = 1;

        public List<System_Zarz.Data.Task> TasksList { get; set; } = new();
        public List<Part> PartsList { get; set; } = new();

        public async Task OnGetAsync()
        {
            TasksList = await _context.Tasks
                .Include(t => t.TaskParts)
                .ThenInclude(tp => tp.Part)
                .AsNoTracking()
                .ToListAsync();


            PartsList = await _context.Parts.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            if (PartQuantity <= 0)
            {
                ModelState.AddModelError(nameof(PartQuantity), "Ilość części musi być większa niż 0");
                await OnGetAsync();
                return Page();
            }

            _context.Tasks.Add(NewTask);
            await _context.SaveChangesAsync();

            var taskPart = new TaskPart
            {
                TaskId = NewTask.Id,
                PartId = SelectedPartId,
                Quantity = PartQuantity
            };

            _context.TaskParts.Add(taskPart);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}