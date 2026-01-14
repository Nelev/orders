using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orders.Model;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> GetOrders()
    {
        List<Order> orders = await _context.Orders.ToListAsync();
        return await _context.Orders.ToListAsync();
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    { 
       order.Id = Guid.NewGuid(); 
        _context.Orders.Add(order); 
        await _context.SaveChangesAsync(); 
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order); 
    }
    [HttpGet("{id}")] public async Task<IActionResult> GetOrderById(Guid id) { var order = await _context.Orders.FindAsync(id); if (order == null) return NotFound(); return Ok(order); }
}
