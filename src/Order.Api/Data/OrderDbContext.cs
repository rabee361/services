using Microsoft.EntityFrameworkCore;

namespace Order.Api.Data;

public class Order
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string UserId { get; set; }
    public string Status { get; set; }
}

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    public DbSet<Order> Orders { get; set; }
}
