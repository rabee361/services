using Microsoft.EntityFrameworkCore;

namespace Product.Api.Data;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    // Foreign key for Category
    public int? CategoryId { get; set; }
    
    // Navigation property
    public Category? Category { get; set; }
}

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure Product-Category relationship
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
        // Seed some default categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets", CreatedAt = DateTime.UtcNow },
            new Category { Id = 2, Name = "Home", Description = "Home and living products", CreatedAt = DateTime.UtcNow },
            new Category { Id = 3, Name = "Accessories", Description = "Fashion and lifestyle accessories", CreatedAt = DateTime.UtcNow },
            new Category { Id = 4, Name = "Furniture", Description = "Furniture and decor", CreatedAt = DateTime.UtcNow },
            new Category { Id = 5, Name = "Apparel", Description = "Clothing and fashion", CreatedAt = DateTime.UtcNow }
        );
    }
}
