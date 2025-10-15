using Order.Domain;
using Order.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public class OrderService(IOrderRepository orderRepo) : IOrderService
    {
        public async Task<CreateOrderResponse> CreateAsync(CreateOrderRequest req, CancellationToken ct)
        {
            // 幂等：存在则直接返回
            if (await orderRepo.ExistsAsync(req.OrderId, ct))
                return new CreateOrderResponse(req.OrderId);

            var items = req.Items.Select(i => new OrderItem(i.ProductId, i.Quantity));
            var order = new Orders(req.OrderId, req.CustomerName, req.CreatedAt, items);

            await orderRepo.AddAsync(order, ct);
            return new CreateOrderResponse(order.OrderId);
        }

        public async Task<GetOrderResponse?> GetAsync(Guid orderId, CancellationToken ct)
        {
            var entity = await orderRepo.GetByIdAsync(orderId, ct);
            if (entity is null) return null;

            var items = entity.Items
                .Select(i => new GetOrderItemDto(i.ProductId, i.Quantity))
                .ToList();

            return new GetOrderResponse(entity.OrderId, entity.CustomerName, entity.CreatedAt, items);
        }
    }
}
