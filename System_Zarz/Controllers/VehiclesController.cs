using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System_Zarz.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace System_Zarz.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Mechanik,Recepcjonista")]
    public class VehiclesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public VehiclesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/vehicles
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _context.Vehicles.Include(v => v.Customer).ToListAsync();
            return Ok(vehicles);
        }

        // GET: api/vehicles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles.Include(v => v.Customer).FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }

        // POST: api/vehicles
        [HttpPost]
        [RequestSizeLimit(10_000_000)] // limit uploadu 10MB, zmień wedle potrzeby
        public async Task<IActionResult> CreateVehicle([FromForm] VehicleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = new Vehicle
            {
                CustomerId = dto.CustomerId,
                Brand = dto.Brand,
                Model = dto.Model,
                VIN = dto.VIN,
                RegistrationNumber = dto.RegistrationNumber,
                Year = dto.Year
            };

            if (dto.UploadPhoto != null && dto.UploadPhoto.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.UploadPhoto.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.UploadPhoto.CopyToAsync(stream);

                vehicle.ImagePath = uniqueFileName;
            }

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, vehicle);
        }

        // PUT: api/vehicles/{id}
        [HttpPut("{id}")]
        [RequestSizeLimit(10_000_000)]
        public async Task<IActionResult> UpdateVehicle(int id, [FromForm] VehicleCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();

            vehicle.CustomerId = dto.CustomerId;
            vehicle.Brand = dto.Brand;
            vehicle.Model = dto.Model;
            vehicle.VIN = dto.VIN;
            vehicle.RegistrationNumber = dto.RegistrationNumber;
            vehicle.Year = dto.Year;

            if (dto.UploadPhoto != null && dto.UploadPhoto.Length > 0)
            {
                // opcjonalnie usuń stare zdjęcie
                if (!string.IsNullOrEmpty(vehicle.ImagePath))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, "uploads", vehicle.ImagePath);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(dto.UploadPhoto.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await dto.UploadPhoto.CopyToAsync(stream);

                vehicle.ImagePath = uniqueFileName;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/vehicles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
                return NotFound();

            if (!string.IsNullOrEmpty(vehicle.ImagePath))
            {
                var path = Path.Combine(_env.WebRootPath, "uploads", vehicle.ImagePath);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class VehicleCreateDto
    {
        public int CustomerId { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string VIN { get; set; } = null!;
        public string RegistrationNumber { get; set; } = null!;
        public int Year { get; set; }
        public IFormFile? UploadPhoto { get; set; }
    }
}
