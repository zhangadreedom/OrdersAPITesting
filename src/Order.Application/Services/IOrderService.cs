using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public interface IOrderService
    {
        /// <summary>Creating an order, if exists, returns an ID</summary>
        Task<CreateOrderResponse> CreateAsync(CreateOrderRequest req, CancellationToken ct);

        /// <summary>Searching order by ID; if not found, return null</summary>
        Task<GetOrderResponse?> GetAsync(Guid orderId, CancellationToken ct);
    }
}
