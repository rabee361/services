namespace Ecommerce.Admin.Models;

public enum UserRole { Client, Manager, Admin }

public class UserViewModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int? CategoryId { get; set; }
    public CategoryViewModel? Category { get; set; }
}

public class CategoryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class WarehouseViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int ManagerId { get; set; }
}

public class StockViewModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public int Quantity { get; set; }
    public WarehouseViewModel? Warehouse { get; set; }
    public string? ProductName { get; set; }
}

public class LoginViewModel
{
    public string UsernameOrEmail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
