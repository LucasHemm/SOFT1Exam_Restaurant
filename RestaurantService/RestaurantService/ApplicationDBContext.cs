using Microsoft.EntityFrameworkCore;
using RestaurantService.Models;

namespace RestaurantService;

public class ApplicationDbContext : DbContext
{

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Address> Addresses { get; set; }

    // Constructor that accepts DbContextOptions
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public ApplicationDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure SQL Server if no options are provided (to avoid overriding options in tests)
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=RestaurantService;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>()
            .HasOne(mi => mi.Restaurant)
            .WithMany(r => r.MenuItems)
            .HasForeignKey("RestaurantId"); // Specify the FK without adding it to the entity
    }


}