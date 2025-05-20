using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System_Zarz.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Vehicles
{
    [Authorize(Roles = "Admin,Mechanik")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IList<Vehicle> Vehicles { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Vehicles = await _context.Vehicles
                .Include(v => v.Customer)
                .ToListAsync();
        }
    }
}