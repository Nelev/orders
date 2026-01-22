using MediatR;
using orders.Model;
public sealed record GetOrderByIdQuery(Guid id) : IRequest<Order>;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order>
{
    private readonly AppDbContext _context;
    public GetOrderByIdHandler(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders.FindAsync(request.id, cancellationToken);
    }
}