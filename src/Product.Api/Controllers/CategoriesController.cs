using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Api.Data;

namespace Product.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ProductDbContext _context;

    public CategoriesController(ProductDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories
            .Include(c => c.Products)
            .ToListAsync();
    }

    /// <summary>
    /// Get a specific category by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
            
        if (category == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }
        
        return category;
    }

    /// <summary>
    /// Get all products in a specific category
    /// </summary>
    [HttpGet("{id}/products")]
    public async Task<ActionResult<IEnumerable<Data.Product>>> GetProductsByCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        var products = await _context.Products
            .Where(p => p.CategoryId == id)
            .ToListAsync();
            
        return products;
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        category.CreatedAt = DateTime.UtcNow;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest(new { message = "Category ID mismatch" });
        }

        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}
