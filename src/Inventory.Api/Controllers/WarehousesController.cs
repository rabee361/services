using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Api.Data;

namespace Inventory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly InventoryDbContext _context;

    public WarehousesController(InventoryDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
    {
        return await _context.Warehouses.ToListAsync();
    }

    [HttpGet("manager/{managerId}")]
    public async Task<ActionResult<Warehouse>> GetWarehouseByManager(int managerId)
    {
        var warehouse = await _context.Warehouses.FirstOrDefaultAsync(w => w.ManagerId == managerId);
        if (warehouse == null) return NotFound();
        return warehouse;
    }

    [HttpPost]
    public async Task<ActionResult<Warehouse>> CreateWarehouse(Warehouse warehouse)
    {
        _context.Warehouses.Add(warehouse);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetWarehouses), new { id = warehouse.Id }, warehouse);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWarehouse(int id, Warehouse warehouse)
    {
        if (id != warehouse.Id) return BadRequest();
        _context.Entry(warehouse).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWarehouse(int id)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);
        if (warehouse == null) return NotFound();
        _context.Warehouses.Remove(warehouse);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
