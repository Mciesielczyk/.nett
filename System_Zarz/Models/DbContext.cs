using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System_Zarz.Data;
using Task = System_Zarz.Data.Task;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<OrderMechanic> OrderMechanics { get; set; }
    public DbSet<OrderTask> OrderTasks { get; set; }
    public DbSet<OrderPart> OrderParts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Kaskadowe usuwanie powiązań w relacjach wiele do wielu
        builder.Entity<OrderMechanic>()
            .HasKey(om => new { om.OrderId, om.MechanicId });
        builder.Entity<OrderMechanic>()
            .HasOne(om => om.Order)
            .WithMany(o => o.Mechanics)
            .HasForeignKey(om => om.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // kaskada
        builder.Entity<OrderMechanic>()
            .HasOne(om => om.Mechanic)
            .WithMany()
            .HasForeignKey(om => om.MechanicId)
            .OnDelete(DeleteBehavior.Cascade); // możesz zmienić jeśli chcesz inny efekt

        builder.Entity<OrderTask>()
            .HasKey(ot => new { ot.OrderId, ot.TaskId });
        builder.Entity<OrderTask>()
            .HasOne(ot => ot.Order)
            .WithMany(o => o.Tasks)
            .HasForeignKey(ot => ot.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // kaskada
        builder.Entity<OrderTask>()
            .HasOne(ot => ot.Task)
            .WithMany(t => t.OrderTasks)
            .HasForeignKey(ot => ot.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderPart>()
            .HasKey(op => new { op.OrderId, op.PartId });
        builder.Entity<OrderPart>()
            .HasOne(op => op.Order)
            .WithMany(o => o.Parts)
            .HasForeignKey(op => op.OrderId)
            .OnDelete(DeleteBehavior.Cascade); // kaskada
        builder.Entity<OrderPart>()
            .HasOne(op => op.Part)
            .WithMany(p => p.OrderParts)
            .HasForeignKey(op => op.PartId)
            .OnDelete(DeleteBehavior.Cascade);

        // Przykład: usuwanie klienta usuwa pojazdy
        builder.Entity<Vehicle>()
            .HasOne(v => v.Customer)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Usuwanie pojazdu usuwa zlecenia
        builder.Entity<Order>()
            .HasOne(o => o.Vehicle)
            .WithMany(v => v.Orders)
            .HasForeignKey(o => o.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Komentarze powiązane ze zleceniem - kaskada
        builder.Entity<Comment>()
            .HasOne(c => c.Order)
            .WithMany(o => o.Comments)
            .HasForeignKey(c => c.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Czynności (Task) i części (Part) same nie są usuwane, bo mogą być używane w innych zleceniach
    }
}