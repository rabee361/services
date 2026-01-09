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
            // Path through Gateway: {GatewayUrl}/products/products maps to Backend: {BackendUrl}/api/products
            var response = await _httpClient.GetAsync($"{_baseUrl}/products");
            if (response.IsSuccessStatusCode)
            {
                var productDtos = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
                return productDtos?.Select(MapToViewModel).ToList() ?? new List<ProductViewModel>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products: {ex.Message}");
        }

        return new List<ProductViewModel>();
    }

    public async Task<List<CategoryViewModel>> GetCategoriesAsync()
    {
        try
        {
            // Path through Gateway: {GatewayUrl}/products/categories maps to Backend: {BackendUrl}/api/categories
            var response = await _httpClient.GetAsync($"{_baseUrl}/categories");
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

        return new List<CategoryViewModel>();
    }

    public async Task<List<ProductViewModel>> GetProductsByCategoryAsync(int categoryId)
    {
        try
        {
            // Path through Gateway: {GatewayUrl}/products/categories/{id}/products
            var response = await _httpClient.GetAsync($"{_baseUrl}/categories/{categoryId}/products");
            if (response.IsSuccessStatusCode)
            {
                var productDtos = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
                return productDtos?.Select(MapToViewModel).ToList() ?? new List<ProductViewModel>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products by category: {ex.Message}");
        }

        return new List<ProductViewModel>();
    }

    private ProductViewModel MapToViewModel(ProductDto dto)
    {
        return new ProductViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Category?.Description ?? "Excellent product",
            // Construct browser-safe relative URL through the Nginx/Gateway path
            ImageUrl = string.IsNullOrEmpty(dto.ImageUrl) ? "/api/placeholder/400/400" : $"/api/products{dto.ImageUrl}",
            Category = dto.Category?.Name ?? "General",
            DateAdded = DateTime.Now,
            IsFeatured = dto.Price > 500 // Sample logic
        };
    }
}

// Internal DTOs for cleaner mapping from the API response
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public CategoryDto? Category { get; set; }
}

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
