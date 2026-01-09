using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Api.Data;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryDbContext _context;

    public InventoryController(InventoryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
    {
        return await _context.Stocks.Include(s => s.Warehouse).ToListAsync();
    }

    [HttpGet("warehouse/{warehouseId}")]
    public async Task<ActionResult<IEnumerable<Stock>>> GetStockByWarehouse(int warehouseId)
    {
        return await _context.Stocks
            .Where(s => s.WarehouseId == warehouseId)
            .ToListAsync();
    }


    [HttpPost]
    public async Task<ActionResult<Stock>> AddStock(Stock stock)
    {
        var existing = await _context.Stocks.FirstOrDefaultAsync(s => s.ProductId == stock.ProductId);
        if (existing != null)
        {
            existing.Quantity += stock.Quantity;
        }
        else
        {
            _context.Stocks.Add(stock);
        }
        await _context.SaveChangesAsync();
        return Ok(stock);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Stock>> GetStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        return stock;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStock(int id, Stock stock)
    {
        if (id != stock.Id)
        {
            return BadRequest();
        }

        _context.Entry(stock).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StockExists(id))
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
    public async Task<IActionResult> DeleteStock(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        if (stock == null)
        {
            return NotFound();
        }

        _context.Stocks.Remove(stock);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool StockExists(int id)
    {
        return _context.Stocks.Any(e => e.Id == id);
    }
}
