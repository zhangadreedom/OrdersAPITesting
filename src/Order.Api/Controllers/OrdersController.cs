using Microsoft.AspNetCore.Mvc;
using Order.Application;
using Order.Application.Services;

namespace Order.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IOrderService svc) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest req, CancellationToken ct)
        {
            var res = await svc.CreateAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id = res.OrderId }, res);
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id) => Ok(new { OrderId = id });
    }
}
