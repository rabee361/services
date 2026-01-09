using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Web.Models;
using Ecommerce.Web.Services;

namespace Ecommerce.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;

    public HomeController(ILogger<HomeController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetProductsAsync();
        
        // If we have real products, use them. Otherwise, fallback to empty to show the design works with real data.
        ViewBag.FeaturedProducts = products.Take(4).ToList();
        ViewBag.NewProducts = products.OrderByDescending(p => p.Id).Take(4).ToList();
        ViewBag.DiscountedProducts = products.Where(p => p.DiscountPercentage.HasValue).Take(4).ToList();
        
        return View();
    }

    public async Task<IActionResult> Gallery()
    {
        var products = await _productService.GetProductsAsync();
        return View(products);
    }

    public async Task<IActionResult> Categories()
    {
        var categories = await _productService.GetCategoriesAsync();
        return View(categories);
    }

    public async Task<IActionResult> Category(int id)
    {
        var categories = await _productService.GetCategoriesAsync();
        var category = categories.FirstOrDefault(c => c.Id == id);
        
        if (category == null)
        {
            return NotFound();
        }
        
        var categoryProducts = await _productService.GetProductsByCategoryAsync(id);
        
        ViewBag.Category = category;
        return View(categoryProducts);
    }

    [HttpGet]
    public async Task<IActionResult> GetMoreProducts(int skip, int take = 10)
    {
        var products = await _productService.GetProductsAsync();
        var pagedProducts = products.Skip(skip).Take(take).ToList();
        return Json(pagedProducts);
    }

    public IActionResult Cart()
    {
        // Keep sample cart logic for now as requested
        var cart = new CartViewModel { Items = new List<CartItemViewModel>() };
        return View(cart);
    }

    public IActionResult Checkout()
    {
        var cart = new CartViewModel { Items = new List<CartItemViewModel>() };
        return View(cart);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
