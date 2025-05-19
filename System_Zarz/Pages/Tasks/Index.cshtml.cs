using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System_Zarz.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Task = System_Zarz.Data.Task;

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
        public Task NewTask { get; set; } = new();

        public List<Task> TasksList { get; set; } = new();

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            TasksList = await _context.Tasks.AsNoTracking().ToListAsync();
        }

        public async System.Threading.Tasks.Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TasksList = await _context.Tasks.AsNoTracking().ToListAsync();
                return Page();
            }

            _context.Tasks.Add(NewTask);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}