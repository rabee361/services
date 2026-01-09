using Ecommerce.Web.Models;

namespace Ecommerce.Web.Services;

public interface IProductService
{
    Task<List<ProductViewModel>> GetProductsAsync();
    Task<List<CategoryViewModel>> GetCategoriesAsync();
    Task<List<ProductViewModel>> GetProductsByCategoryAsync(int categoryId);
}

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public ProductService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["Services:ProductApi"] ?? "http://localhost:5025";
    }

    public async Task<List<ProductViewModel>> GetProductsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/products");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
                return products ?? new List<ProductViewModel>();
            }
        }
        catch (Exception ex)
        {
            // Fallback or log error
            Console.WriteLine($"Error fetching products: {ex.Message}");
        }

        // Return empty or pseudo data for now as per user's "pseudo data first" rule
        return new List<ProductViewModel>();
    }

    public async Task<List<CategoryViewModel>> GetCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/categories");
            if (response.IsSuccessStatusCode)
            {
                var categories = await response.Content.ReadFromJsonAsync<List<CategoryViewModel>>();
                return categories ?? new List<CategoryViewModel>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching categories: {ex.Message}");
        }

        // Return pseudo data as fallback
        return new List<CategoryViewModel>
        {
            new CategoryViewModel { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets", ProductCount = 5 },
            new CategoryViewModel { Id = 2, Name = "Home", Description = "Home and living products", ProductCount = 3 },
            new CategoryViewModel { Id = 3, Name = "Accessories", Description = "Fashion and lifestyle accessories", ProductCount = 4 },
            new CategoryViewModel { Id = 4, Name = "Furniture", Description = "Furniture and decor", ProductCount = 2 },
            new CategoryViewModel { Id = 5, Name = "Apparel", Description = "Clothing and fashion", ProductCount = 6 }
        };
    }

    public async Task<List<ProductViewModel>> GetProductsByCategoryAsync(int categoryId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/categories/{categoryId}/products");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();
                return products ?? new List<ProductViewModel>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products by category: {ex.Message}");
        }

        return new List<ProductViewModel>();
    }
}
