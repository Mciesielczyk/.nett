using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;

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

    // Znajdowanie klienta po id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return NotFound(); // Zwróć 404 jeśli nie znaleziono
        }
        return Ok(customer);
    }
    
    // Dodanie nowego klienta
    [HttpPost]
    public async Task<IActionResult> AddCustomer([FromBody] Customer newCustomer)
    {
        _context.Customers.Add(newCustomer);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCustomers), new { id = newCustomer.Id  }, newCustomer);
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
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return NotFound();

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}