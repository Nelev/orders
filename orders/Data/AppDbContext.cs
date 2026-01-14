using Microsoft.EntityFrameworkCore;
using orders.Model;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Add your DbSets here
    public DbSet<Order> Orders { get; set; }
}

