using Microsoft.AspNetCore.Mvc;
using Ecommerce.Admin.Models;

namespace Ecommerce.Admin.Controllers;

public class ProductsController : BaseAdminController
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public ProductsController(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiUrl = (configuration["Services:ProductApi"] ?? "http://localhost:5025") + "/products";
    }

    public async Task<IActionResult> Index()
    {
        if (!IsAdmin) return RedirectToAction("Index", "Dashboard");
        ViewData["ActivePage"] = "Products";
        var response = await _httpClient.GetAsync(_apiUrl);
        var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>() ?? new();
        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        await _httpClient.PostAsJsonAsync(_apiUrl, model);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
        return RedirectToAction(nameof(Index));
    }
}
