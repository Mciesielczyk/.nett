using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System_Zarz.Models;
using System.Threading.Tasks;
using System.IO;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace System_Zarz.Pages.Vehicles
{
    [Authorize(Roles = "Admin,Mechanik")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Vehicle Vehicle { get; set; }

        [BindProperty]
        public IFormFile UploadPhoto { get; set; }

        public List<SelectListItem> CustomersList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Vehicle = await _context.Vehicles.FindAsync(id);
            if (Vehicle == null)
                return NotFound();

            CustomersList = await _context.Customers
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.FullName // lub np. $"{c.FirstName} {c.LastName}"
                }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var vehicleToUpdate = await _context.Vehicles.FindAsync(Vehicle.Id);

            if (vehicleToUpdate == null)
                return NotFound();

            vehicleToUpdate.VIN = Vehicle.VIN;
            vehicleToUpdate.RegistrationNumber = Vehicle.RegistrationNumber;
            vehicleToUpdate.CustomerId = Vehicle.CustomerId;

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

                vehicleToUpdate.ImagePath = uniqueFileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
