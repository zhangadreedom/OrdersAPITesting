using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Order.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Order.UnitTests
{
    public class OrderIntegrationTests : IClassFixture<TestWebAppFactory>
    {
        private readonly HttpClient _client;

        public OrderIntegrationTests(TestWebAppFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task CreateOrderTest()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var req = new CreateOrderRequest(
                orderId,
                "Li4",
                DateTimeOffset.UtcNow,
                new List<CreateOrderItem> { new(Guid.NewGuid(), 2) });

            // Act
            var resp = await _client.PostAsJsonAsync("/api/Orders", req);

            // Assert
            resp.StatusCode.Should().Be(HttpStatusCode.Created);
            resp.Headers.Location!.ToString().Should().Contain($"/api/Orders/{orderId}");

            var body = await resp.Content.ReadFromJsonAsync<CreateOrderResponse>();
            body!.OrderId.Should().Be(orderId);
        }

    }
}
