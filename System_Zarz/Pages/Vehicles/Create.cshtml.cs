using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System_Zarz.Data;
using System_Zarz.Models;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace System_Zarz.Pages.Vehicles
{
    [Authorize(Roles = "Admin,Mechanik,Recepcjonista")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; }


        //Opcjonalne pole 
        [BindProperty]
        public IFormFile? UploadPhoto { get; set; }

        public SelectList CustomersList { get; set; }

        public async Task OnGetAsync()
        {
            // Załaduj listę klientów do dropdowna
            var customers = await _context.Customers
                .OrderBy(c => c.FullName)  // lub inne pole
                .ToListAsync();

            CustomersList = new SelectList(customers, "Id", "FullName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Żeby nie stracić listy klientów przy błędzie
                return Page();
            }

            if (UploadPhoto != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(UploadPhoto.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadPhoto.CopyToAsync(fileStream);
                }

                Vehicle.ImagePath = uniqueFileName;
            }

            _context.Vehicles.Add(Vehicle);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
