using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.Api.Data;
using OrderEntity = Order.Api.Data.Order;


namespace Order.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderDbContext _context;
    public OrdersController(OrderDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderEntity order)
    {
        order.Id = Guid.NewGuid();
        order.Status = "Pending";
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();



        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderEntity>>> GetOrders()
    {
        return await _context.Orders.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderEntity>> GetOrder(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(Guid id, OrderEntity order)
    {
        if (id != order.Id)
        {
            return BadRequest();
        }

        _context.Entry(order).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderExists(id))
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
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrderExists(Guid id)
    {
        return _context.Orders.Any(e => e.Id == id);
    }
}
