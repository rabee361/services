using Microsoft.EntityFrameworkCore;

namespace Users.Api.Data;

public enum UserRole
{
    Client,
    Manager,
    Admin
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // In a production app, use password hashing
    public UserRole Role { get; set; } = UserRole.Client;
}

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial Admin and Manager users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@glow.com", Password = "admin", Role = UserRole.Admin },
            new User { Id = 2, Username = "manager", Email = "manager@glow.com", Password = "manager", Role = UserRole.Manager }
        );
    }
}

