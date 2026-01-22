using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orders.Model;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMediator _mediator;

    public OrdersController(AppDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await _mediator.Send(new GetOrdersQuery());
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        order.Id = Guid.NewGuid();
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id)
    {
        Order order = await _mediator.Send(new GetOrderByIdQuery(id));
        if (order == null)
            return NotFound();
        return Ok(order);
    }
}
