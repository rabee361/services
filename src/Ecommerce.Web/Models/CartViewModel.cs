namespace Ecommerce.Web.Models;

public class CartViewModel
{
    public List<CartItemViewModel> Items { get; set; } = new();
    
    public decimal Subtotal => Items.Sum(i => i.Subtotal);
    public decimal Tax => Subtotal * 0.1m; // 10% tax
    public decimal Shipping => Subtotal > 100 ? 0 : 10m; // Free shipping over $100
    public decimal Total => Subtotal + Tax + Shipping;
    public int TotalItems => Items.Sum(i => i.Quantity);
}
