using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;

namespace System_Zarz.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Mechanik")]
    public class SparePartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SparePartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parts = await _context.SparePart.ToListAsync();
            return Ok(parts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var part = await _context.SparePart.FindAsync(id);
            if (part == null) return NotFound();
            return Ok(part);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SparePart model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.SparePart.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SparePart model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _context.SparePart.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = model.Name;
            existing.Type = model.Type;
            existing.UnitPrice = model.UnitPrice;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.SparePart.FindAsync(id);
            if (existing == null) return NotFound();

            _context.SparePart.Remove(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
