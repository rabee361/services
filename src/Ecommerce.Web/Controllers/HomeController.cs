using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Web.Models;

namespace Ecommerce.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    private static readonly List<ProductViewModel> PseudoProducts = new()
    {
        new ProductViewModel { Id = 1, Name = "Premium Wireless Headphones", Price = 299.99m, Category = "Electronics", ImageUrl = "/api/placeholder/400/400", IsFeatured = true, DateAdded = DateTime.Now.AddDays(-5) },
        new ProductViewModel { Id = 2, Name = "Modern Desk Lamp", Price = 89.00m, Category = "Home", ImageUrl = "/api/placeholder/400/400", IsFeatured = true, DateAdded = DateTime.Now.AddDays(-20) },
        new ProductViewModel { Id = 3, Name = "Ultra-Thin Laptop 15\"", Price = 1299.00m, Category = "Electronics", ImageUrl = "/api/placeholder/400/400", IsFeatured = false, DateAdded = DateTime.Now.AddDays(-2), DiscountPercentage = 15 },
        new ProductViewModel { Id = 4, Name = "Leather Minimalist Wallet", Price = 45.00m, Category = "Accessories", ImageUrl = "/api/placeholder/400/400", IsFeatured = true, DateAdded = DateTime.Now.AddDays(-10) },
        new ProductViewModel { Id = 5, Name = "Ergonomic Office Chair", Price = 450.00m, Category = "Furniture", ImageUrl = "/api/placeholder/400/400", IsFeatured = false, DateAdded = DateTime.Now.AddDays(-1), DiscountPercentage = 10 },
        new ProductViewModel { Id = 6, Name = "Smart Watch Series X", Price = 349.99m, Category = "Electronics", ImageUrl = "/api/placeholder/400/400", IsFeatured = true, DateAdded = DateTime.Now.AddDays(-30) },
        new ProductViewModel { Id = 7, Name = "Organic Cotton T-Shirt", Price = 25.00m, Category = "Apparel", ImageUrl = "/api/placeholder/400/400", IsFeatured = false, DateAdded = DateTime.Now.AddDays(-12), DiscountPercentage = 20 },
        new ProductViewModel { Id = 8, Name = "Bluetooth Speaker Water-Proof", Price = 120.00m, Category = "Electronics", ImageUrl = "/api/placeholder/400/400", IsFeatured = false, DateAdded = DateTime.Now.AddDays(-8) },
    };

    public IActionResult Index()
    {
        ViewBag.FeaturedProducts = PseudoProducts.Where(p => p.IsFeatured).Take(4).ToList();
        ViewBag.NewProducts = PseudoProducts.OrderByDescending(p => p.DateAdded).Take(4).ToList();
        ViewBag.DiscountedProducts = PseudoProducts.Where(p => p.DiscountPercentage.HasValue).Take(4).ToList();
        return View();
    }

    public IActionResult Gallery()
    {
        return View(PseudoProducts);
    }

    public IActionResult Categories()
    {
        // Pseudo category data
        var categories = new List<CategoryViewModel>
        {
            new CategoryViewModel { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets", ProductCount = 5 },
            new CategoryViewModel { Id = 2, Name = "Home", Description = "Home and living products", ProductCount = 3 },
            new CategoryViewModel { Id = 3, Name = "Accessories", Description = "Fashion and lifestyle accessories", ProductCount = 4 },
            new CategoryViewModel { Id = 4, Name = "Furniture", Description = "Furniture and decor", ProductCount = 2 },
            new CategoryViewModel { Id = 5, Name = "Apparel", Description = "Clothing and fashion", ProductCount = 6 }
        };
        
        return View(categories);
    }

    public IActionResult Category(int id)
    {
        // Get category info
        var categories = new List<CategoryViewModel>
        {
            new CategoryViewModel { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets", ProductCount = 5 },
            new CategoryViewModel { Id = 2, Name = "Home", Description = "Home and living products", ProductCount = 3 },
            new CategoryViewModel { Id = 3, Name = "Accessories", Description = "Fashion and lifestyle accessories", ProductCount = 4 },
            new CategoryViewModel { Id = 4, Name = "Furniture", Description = "Furniture and decor", ProductCount = 2 },
            new CategoryViewModel { Id = 5, Name = "Apparel", Description = "Clothing and fashion", ProductCount = 6 }
        };
        
        var category = categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        
        // Filter products by category
        var categoryProducts = PseudoProducts.Where(p => p.Category == category.Name).ToList();
        
        ViewBag.Category = category;
        return View(categoryProducts);
    }

    [HttpGet]
    public IActionResult GetMoreProducts(int skip, int take = 10)
    {
        // For infinite scroll demonstration
        var products = PseudoProducts.Concat(PseudoProducts).Concat(PseudoProducts).Skip(skip).Take(take).ToList();
        return Json(products);
    }

    public IActionResult Cart()
    {
        // Sample cart data - in real application, this would come from session/database
        var cart = new CartViewModel
        {
            Items = new List<CartItemViewModel>
            {
                new CartItemViewModel
                {
                    ProductId = 1,
                    ProductName = "Premium Wireless Headphones",
                    Price = 299.99m,
                    Quantity = 1,
                    ImageUrl = "/api/placeholder/400/400",
                    Category = "Electronics"
                },
                new CartItemViewModel
                {
                    ProductId = 3,
                    ProductName = "Ultra-Thin Laptop 15\"",
                    Price = 1299.00m,
                    Quantity = 1,
                    ImageUrl = "/api/placeholder/400/400",
                    Category = "Electronics"
                },
                new CartItemViewModel
                {
                    ProductId = 4,
                    ProductName = "Leather Minimalist Wallet",
                    Price = 45.00m,
                    Quantity = 2,
                    ImageUrl = "/api/placeholder/400/400",
                    Category = "Accessories"
                }
            }
        };
        
        return View(cart);
    }

    public IActionResult Checkout()
    {
        // Sample cart data for checkout
        var cart = new CartViewModel
        {
            Items = new List<CartItemViewModel>
            {
                new CartItemViewModel
                {
                    ProductId = 1,
                    ProductName = "Premium Wireless Headphones",
                    Price = 299.99m,
                    Quantity = 1,
                    ImageUrl = "/api/placeholder/400/400",
                    Category = "Electronics"
                },
                new CartItemViewModel
                {
                    ProductId = 3,
                    ProductName = "Ultra-Thin Laptop 15\"",
                    Price = 1299.00m,
                    Quantity = 1,
                    ImageUrl = "/api/placeholder/400/400",
                    Category = "Electronics"
                },
                new CartItemViewModel
                {
                    ProductId = 4,
                    ProductName = "Leather Minimalist Wallet",
                    Price = 45.00m,
                    Quantity = 2,
                    ImageUrl = "/api/placeholder/400/400",
                    Category = "Accessories"
                }
            }
        };
        
        return View(cart);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
