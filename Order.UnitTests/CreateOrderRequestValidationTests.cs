using FluentAssertions;
using Order.Application;
using Order.Application.Validation;

namespace Order.UnitTests
{
    public class CreateOrderRequestValidationTests
    {
        private readonly CreateOrderValidator _validator = new();
        [Fact]
        public void TestItemsEmpty()
        {
            var req = new CreateOrderRequest(Guid.NewGuid(), "ZhangSan", DateTimeOffset.UtcNow, new List<CreateOrderItem>());
            var result = _validator.Validate(req);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "Items");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void TestEmptyCustomerName(string? name)
        {
            var req = new CreateOrderRequest(Guid.NewGuid(), name!, DateTimeOffset.UtcNow,
                new List<CreateOrderItem> { new(Guid.NewGuid(), 1) });

            var result = _validator.Validate(req);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "CustomerName");
        }

        [Fact]
        public void TestQuantityLessThanOrEqual0()
        {
            var req = new CreateOrderRequest(Guid.NewGuid(), "ZhangSan", DateTimeOffset.UtcNow,
                new List<CreateOrderItem> { new(Guid.NewGuid(), 0) });

            var result = _validator.Validate(req);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Items[0].Quantity");

            var req2 = new CreateOrderRequest(Guid.NewGuid(), "ZhangSan", DateTimeOffset.UtcNow,
                new List<CreateOrderItem> { new(Guid.NewGuid(), -1) });

            var result2 = _validator.Validate(req);

            result2.IsValid.Should().BeFalse();
            result2.Errors.Should().Contain(e => e.PropertyName == "Items[0].Quantity");
        }

        [Fact]
        public void GoodRequest()
        {
            var req = new CreateOrderRequest(Guid.NewGuid(), "ZhangSan", DateTimeOffset.UtcNow,
                new List<CreateOrderItem> { new(Guid.NewGuid(), 2) });

            var result = _validator.Validate(req);

            result.IsValid.Should().BeTrue();
        }
    }
}