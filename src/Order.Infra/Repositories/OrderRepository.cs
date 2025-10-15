using Microsoft.EntityFrameworkCore;
using Order.Domain;
using Order.Domain.Entities;

namespace Order.Infra.Repositories
{
    public class OrderRepository(OrderDbContext db) : IOrderRepository
    {
        public async Task AddAsync(Orders orders, CancellationToken ct)
        {
            await db.Orders.AddAsync(orders, ct);
            await db.SaveChangesAsync(ct);
        }

        public Task<bool> ExistsAsync(Guid orderId, CancellationToken ct)
            => db.Orders.AnyAsync(o => o.OrderId == orderId, ct);

        public Task<Orders?> GetByIdAsync(Guid orderId, CancellationToken ct)
            => db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.OrderId == orderId, ct);
    }
}
