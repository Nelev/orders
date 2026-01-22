using MediatR;
using Microsoft.EntityFrameworkCore;
using orders.Model;
public  sealed record GetOrdersQuery() : IRequest<List<Order>>;

public class GetOrdersHandler(AppDbContext _context) : IRequestHandler<GetOrdersQuery, List<Order>> 
{ 
    public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken) 
    
    { 
        return await _context.Orders.ToListAsync(); 
    } 
}