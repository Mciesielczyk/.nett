using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using System_Zarz.Models;

namespace System_Zarz.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Recepcjonista")]
public class CustomersApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomersApiController> _logger;

    public CustomersApiController(ApplicationDbContext context, ILogger<CustomersApiController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Customer>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Customer>>> GetCustomers()
    {
        _logger.LogInformation("Pobieranie wszystkich klientów.");
        var customers = await _context.Customers.ToListAsync();
        _logger.LogInformation("Zwrócono {Count} klientów.", customers.Count);
        return customers;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Customer>> GetCustomer(int id)
    {
        _logger.LogInformation("Pobieranie klienta o Id={Id}.", id);
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            _logger.LogWarning("Klient o Id={Id} nie znaleziony.", id);
            return NotFound();
        }
        return customer;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
    {
        _logger.LogInformation("Tworzenie nowego klienta: {FullName}", customer.FullName);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Niepoprawny model klienta: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(ModelState);
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Klient utworzony o Id={Id}.", customer.Id);
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
    {
        _logger.LogInformation("Aktualizacja klienta o Id={Id}.", id);

        if (id != customer.Id)
        {
            _logger.LogWarning("Niezgodność Id w URL i modelu: {Id} != {CustomerId}", id, customer.Id);
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Niepoprawny model podczas aktualizacji klienta: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(ModelState);
        }

        var existingCustomer = await _context.Customers.FindAsync(id);
        if (existingCustomer == null)
        {
            _logger.LogWarning("Klient o Id={Id} nie znaleziony do aktualizacji.", id);
            return NotFound();
        }

        existingCustomer.FullName = customer.FullName;
        existingCustomer.Email = customer.Email;
        existingCustomer.Phone = customer.Phone;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Klient o Id={Id} został zaktualizowany.", id);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!CustomerExists(id))
            {
                _logger.LogWarning("Klient o Id={Id} nie istnieje przy obsłudze wyjątku.", id);
                return NotFound();
            }
            _logger.LogError(ex, "Błąd podczas aktualizacji klienta o Id={Id}.", id);
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        _logger.LogInformation("Usuwanie klienta o Id={Id}.", id);

        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            _logger.LogWarning("Klient o Id={Id} nie znaleziony do usunięcia.", id);
            return NotFound();
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Klient o Id={Id} został usunięty.", id);
        return NoContent();
    }

    private bool CustomerExists(int id)
    {
        return _context.Customers.Any(e => e.Id == id);
    }
}
