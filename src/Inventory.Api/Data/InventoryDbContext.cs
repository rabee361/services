using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Data;

public class Warehouse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int ManagerId { get; set; } // ID from Users.Api
}

public class Stock
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }
    
    // Navigation property
    public Warehouse? Warehouse { get; set; }
}

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }
    
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial warehouse
        modelBuilder.Entity<Warehouse>().HasData(
            new Warehouse { Id = 1, Name = "Main Warehouse", Location = "Downtown", ManagerId = 2 } // Manager ID matches seeded manager in Users.Api
        );
    }
}

