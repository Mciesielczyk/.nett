using Microsoft.AspNetCore.Mvc;
using System_Zarz.Data; // zmień namespace jeśli trzeba
using System_Zarz.Models; // namespace dla Customer

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Dodajemy klienta tylko raz
        if (!_context.Customers.Any())
        {
            var customer = new Customer
            {
                FullName = "Jan Kowalski",
                Email = "jan.kowalski@example.com",
                Phone = "123-456-789"
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        var allCustomers = _context.Customers.ToList();

        return Json(allCustomers);
        // trzeba stworzyć View, albo zwrócić JSON
    }
}