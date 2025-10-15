using Order.Domain.Entities;

namespace Order.Domain;

public interface IOrderRepository
{
    /// <summary>
    /// Adding an order
    /// </summary>
    Task AddAsync(Orders order, CancellationToken ct);

    /// <summary>
    /// Verify if an order exists or not (via OrderId)
    /// </summary>
    Task<bool> ExistsAsync(Guid orderId, CancellationToken ct);

    Task<Orders?> GetByIdAsync(Guid orderId, CancellationToken ct);
}
