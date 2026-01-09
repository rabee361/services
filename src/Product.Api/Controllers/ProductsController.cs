using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Api.Data;
using ProductEntity = Product.Api.Data.Product;

namespace Product.Api.Controllers;

// DTO for category assignment
public class AssignCategoryRequest
{
    public int? CategoryId { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductDbContext _context;

    public ProductsController(ProductDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductEntity>>> GetProducts()
    {
        return await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductEntity>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
            
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<ProductEntity>> CreateProduct(ProductEntity product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductEntity product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Assign or update a product's category
    /// </summary>
    [HttpPatch("{id}/category")]
    public async Task<IActionResult> AssignCategory(int id, [FromBody] AssignCategoryRequest request)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }

        // Validate category exists if not null
        if (request.CategoryId.HasValue)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId.Value);
            if (!categoryExists)
            {
                return BadRequest(new { message = $"Category with ID {request.CategoryId.Value} not found" });
            }
        }

        product.CategoryId = request.CategoryId;
        await _context.SaveChangesAsync();

        // Return updated product with category
        var updatedProduct = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        return Ok(updatedProduct);
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
