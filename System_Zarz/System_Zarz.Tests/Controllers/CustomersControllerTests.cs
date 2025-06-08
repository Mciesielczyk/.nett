using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System_Zarz.Controllers;
using System_Zarz.Data;
using System_Zarz.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

public class CustomersApiControllerTests
{
    /*
    [Fact]
    public async Task GetCustomers_ReturnsAllCustomers()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_GetCustomers")
            .Options;

        // Tworzymy i wypełniamy bazę testowymi danymi
        using (var context = new ApplicationDbContext(options))
        {
            context.Customers.Add(new Customer { Id = 1, FullName = "Jan Kowalski", Email = "jan@example.com" });
            context.Customers.Add(new Customer { Id = 2, FullName = "Anna Nowak", Email = "anna@example.com" });
            context.SaveChanges();
        }

        // Używamy nowego kontekstu do testu
        using (var context = new ApplicationDbContext(options))
        {
            var controller = new CustomersApiController(context, NullLogger<CustomersApiController>.Instance);

            // Act
            var result = await controller.GetCustomers();

            // Assert
            var okResult = Assert.IsType<ActionResult<List<Customer>>>(result);
            var customers = Assert.IsType<List<Customer>>(okResult.Value);
            Assert.Equal(2, customers.Count);
        }
    }

    [Fact]
    public async Task GetCustomer_WithValidId_ReturnsCustomer()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_GetCustomer_ValidId")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.Customers.Add(new Customer { Id = 1, FullName = "Jan Kowalski", Email = "jan@example.com" });
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var controller = new CustomersApiController(context, NullLogger<CustomersApiController>.Instance);

            var result = await controller.GetCustomer(1);

            var actionResult = Assert.IsType<ActionResult<Customer>>(result);
            var customer = Assert.IsType<Customer>(actionResult.Value);
            Assert.Equal(1, customer.Id);
            Assert.Equal("Jan Kowalski", customer.FullName);
        }
    }

    [Fact]
    public async Task GetCustomer_WithInvalidId_ReturnsNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_GetCustomer_InvalidId")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            // Nie dodajemy żadnych klientów
        }

        using (var context = new ApplicationDbContext(options))
        {
            var controller = new CustomersApiController(context, NullLogger<CustomersApiController>.Instance);

            var result = await controller.GetCustomer(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }

    [Fact]
    public async Task CreateCustomer_WithValidCustomer_ReturnsCreatedCustomer()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_CreateCustomer")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            var controller = new CustomersApiController(context, NullLogger<CustomersApiController>.Instance);

            var newCustomer = new Customer
            {
                FullName = "Nowy Klient",
                Email = "nowy@example.com",
                Phone = "123456789"
            };

            var result = await controller.CreateCustomer(newCustomer);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdCustomer = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal("Nowy Klient", createdCustomer.FullName);
        }
    }

    [Fact]
    public async Task UpdateCustomer_WithValidData_ReturnsNoContent()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_UpdateCustomer")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.Customers.Add(new Customer { Id = 1, FullName = "Jan Kowalski", Email = "jan@example.com" });
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var controller = new CustomersApiController(context, NullLogger<CustomersApiController>.Instance);

            var updatedCustomer = new Customer
            {
                Id = 1,
                FullName = "Jan Nowak",
                Email = "jan.nowak@example.com",
                Phone = "987654321"
            };

            var result = await controller.UpdateCustomer(1, updatedCustomer);

            Assert.IsType<NoContentResult>(result);

            var customerInDb = await context.Customers.FindAsync(1);
            Assert.Equal("Jan Nowak", customerInDb.FullName);
        }
    }

    [Fact]
    public async Task DeleteCustomer_WithValidId_ReturnsNoContent()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_DeleteCustomer")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.Customers.Add(new Customer { Id = 1, FullName = "Jan Kowalski", Email = "jan@example.com" });
            context.SaveChanges();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var controller = new CustomersApiController(context, NullLogger<CustomersApiController>.Instance);

            var result = await controller.DeleteCustomer(1);

            Assert.IsType<NoContentResult>(result);

            var customer = await context.Customers.FindAsync(1);
            Assert.Null(customer);
        }
    }

    [Fact]
    public async Task DeleteCustomer_WithInvalidId_ReturnsNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_DeleteCustomer_InvalidId")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            // Brak klientów
        }

        using (var context = new ApplicationDbContext(options))
        {
            var controller = new CustomersApiController(context, NullLogger<CustomersApiController>.Instance);

            var result = await controller.DeleteCustomer(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
     */
}