using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // Tutaj dodasz swoje DbSety, które reprezentują tabele w bazie
    public DbSet<Customer> Customers { get; set; }
    //public DbSet<Vehicle> Vehicles { get; set; }
   // public DbSet<ServiceOrder> ServiceOrders { get; set; }
    // Dodaj kolejne DbSety w zależności od swojej aplikacji
}