using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Data;

public class BooksDbContext : DbContext
{
    public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
}