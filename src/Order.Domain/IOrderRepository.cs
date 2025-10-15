using Order.Domain.Entities;

namespace Order.Domain;

public interface IOrderRepository
{
    /// <summary>
    /// Adding anorder
    /// </summary>
    Task AddAsync(Orders order, CancellationToken ct);

    /// <summary>
    /// Verify if an order exists or not (via OrderId)
    /// </summary>
    Task<bool> ExistsAsync(Guid orderId, CancellationToken ct);
}
