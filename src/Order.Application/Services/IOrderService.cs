using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public interface IOrderService
    {
        /// <summary>创建订单（幂等：OrderId 已存在时直接返回该 Id）</summary>
        Task<CreateOrderResponse> CreateAsync(CreateOrderRequest req, CancellationToken ct);

        /// <summary>按 Id 查询订单；未找到返回 null</summary>
        Task<GetOrderResponse?> GetAsync(Guid orderId, CancellationToken ct);
    }
}
