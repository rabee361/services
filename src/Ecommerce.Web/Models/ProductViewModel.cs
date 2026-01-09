namespace Ecommerce.Web.Models;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double? DiscountPercentage { get; set; }
    public bool IsFeatured { get; set; }
    public DateTime DateAdded { get; set; }

    public decimal DiscountedPrice => DiscountPercentage.HasValue 
        ? Price * (decimal)(1 - (DiscountPercentage.Value / 100)) 
        : Price;
}
