using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Order.Application;
using Order.Application.Services;
using Order.Domain;
using Order.Domain.Entities;

namespace Order.UnitTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepo = new();
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _service = new OrderService(_orderRepo.Object, new Mock<ILogger<OrderService>>().Object);
        }

        [Fact]
        public async Task CreateNewRequest()
        {
            var req = new CreateOrderRequest(Guid.NewGuid(), "ZhangSan", DateTimeOffset.UtcNow,
                new List<CreateOrderItem> { new(Guid.NewGuid(), 10) });

            _orderRepo.Setup(r => r.ExistsAsync(req.OrderId, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _orderRepo.Setup(r => r.AddAsync(It.IsAny<Orders>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var res = await _service.CreateAsync(req, CancellationToken.None);

            res.OrderId.Should().Be(req.OrderId);
            _orderRepo.Verify(r => r.AddAsync(It.Is<Orders>(o => o.OrderId == req.OrderId), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
