using Microsoft.AspNetCore.Mvc;
using Ecommerce.Admin.Models;

namespace Ecommerce.Admin.Controllers;

public class InventoryController : BaseAdminController
{
    private readonly HttpClient _httpClient;
    private readonly string _inventoryApi;
    private readonly string _productApi;

    public InventoryController(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _inventoryApi = configuration["Services:InventoryApi"] ?? "http://localhost:5016";
        _productApi = configuration["Services:ProductApi"] ?? "http://localhost:5025";
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Inventory";
        
        List<StockViewModel> stocks = new();
        
        if (IsAdmin)
        {
            var response = await _httpClient.GetAsync($"{_inventoryApi}/inventory");
            stocks = await response.Content.ReadFromJsonAsync<List<StockViewModel>>() ?? new();
        }
        else if (IsManager)
        {
            // Get warehouse for this manager
            var wResponse = await _httpClient.GetAsync($"{_inventoryApi}/warehouses/manager/{CurrentUser.Id}");
            if (wResponse.IsSuccessStatusCode)
            {
                var warehouse = await wResponse.Content.ReadFromJsonAsync<WarehouseViewModel>();
                if (warehouse != null)
                {
                    var sResponse = await _httpClient.GetAsync($"{_inventoryApi}/inventory/warehouse/{warehouse.Id}");
                    stocks = await sResponse.Content.ReadFromJsonAsync<List<StockViewModel>>() ?? new();
                }
            }
        }

        // Fetch product names for display
        var productsResponse = await _httpClient.GetAsync($"{_productApi}/products");
        var products = await productsResponse.Content.ReadFromJsonAsync<List<ProductViewModel>>() ?? new();
        
        foreach (var stock in stocks)
        {
            stock.ProductName = products.FirstOrDefault(p => p.Id == stock.ProductId)?.Name ?? "Unknown Product";
        }

        return View(stocks);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStock(int id, int quantity)
    {
        var response = await _httpClient.GetAsync($"{_inventoryApi}/inventory/{id}");
        if (response.IsSuccessStatusCode)
        {
            var stock = await response.Content.ReadFromJsonAsync<StockViewModel>();
            if (stock != null)
            {
                stock.Quantity = quantity;
                await _httpClient.PutAsJsonAsync($"{_inventoryApi}/inventory/{id}", stock);
            }
        }
        return RedirectToAction(nameof(Index));
    }
}
