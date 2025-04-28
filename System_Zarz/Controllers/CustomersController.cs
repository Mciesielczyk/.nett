using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _context.Customers.ToListAsync();
        return Ok(customers);
    }
    
    [HttpPut("updatePhone/{fullName}")]
    public async Task<IActionResult> UpdatePhoneNumber(string fullName, [FromBody] string newPhoneNumber)
    {
        // Znajdowanie klienta o pełnym imieniu i nazwisku
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.FullName == fullName);

        if (customer == null)
        {
            return NotFound($"Customer with name {fullName} not found.");
        }

        // Aktualizacja numeru telefonu
        customer.Phone = newPhoneNumber;

        // Zapisanie zmian w bazie danych
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();

        return Ok($"Phone number for {fullName} has been updated to {newPhoneNumber}.");
    }
}