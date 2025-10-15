using Microsoft.AspNetCore.Mvc;
using Order.Application;
using Order.Application.Services;
using Microsoft.Extensions.Logging;

namespace Order.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService service, ILogger<OrdersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken ct)
        {
            _logger.LogInformation("Received new order from {CustomerName}, items count = {ItemCount}",
                request.CustomerName, request.Items.Count);

            var result = await _service.CreateAsync(request, ct);

            _logger.LogInformation("Order {OrderId} created successfully", result.OrderId);

            return CreatedAtAction(nameof(GetById), new { id = result.OrderId }, result);
        }

        [HttpGet("{id:guid}", Name = nameof(GetById))]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            _logger.LogDebug("Fetching order {OrderId}", id);

            var order = await _service.GetAsync(id, ct);
            if (order is null)
            {
                _logger.LogWarning("Order {OrderId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Order {OrderId} retrieved: Customer={CustomerName}, ItemCount={Count}",
                order.OrderId, order.CustomerName, order.Items.Count);

            return Ok(order);
        }
    }
}
