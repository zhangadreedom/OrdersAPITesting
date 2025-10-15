using Order.Domain;
using Order.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepo;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IOrderRepository repo, ILogger<OrderService> logger)
        {
            orderRepo = repo;
            _logger = logger;
        }

        public async Task<CreateOrderResponse> CreateAsync(CreateOrderRequest req, CancellationToken ct)
        {
            // return existing order if OrderId already exists
            if (await orderRepo.ExistsAsync(req.OrderId, ct))
                return new CreateOrderResponse(req.OrderId);

            _logger.LogInformation("Creating order for {Customer}", req.CustomerName);
            var items = req.Items.Select(i => new OrderItem(i.ProductId, i.Quantity));
            var order = new Orders(req.OrderId, req.CustomerName, req.CreatedAt, items);

            await orderRepo.AddAsync(order, ct);

            _logger.LogInformation("Order created: {OrderId}", order.OrderId);
            return new CreateOrderResponse(order.OrderId);
        }

        public async Task<GetOrderResponse?> GetAsync(Guid orderId, CancellationToken ct)
        {
            var entity = await orderRepo.GetByIdAsync(orderId, ct);
            if (entity is null) return null;

            _logger.LogInformation("Order Found: {OrderId}", entity.OrderId);
            var items = entity.Items
                .Select(i => new GetOrderItemDto(i.ProductId, i.Quantity))
                .ToList();
            

            _logger.LogInformation("Total items in order {OrderId}: {ItemCount}", entity.OrderId, items.Count);

            return new GetOrderResponse(entity.OrderId, entity.CustomerName, entity.CreatedAt, items);
        }
    }
}
