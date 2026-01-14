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
}
