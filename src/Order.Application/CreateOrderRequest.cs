namespace Order.Application;

public record CreateOrderItem(Guid ProductId, int Quantity);

public record CreateOrderRequest(
    Guid OrderId,
    string CustomerName,
    DateTimeOffset CreatedAt,
    IReadOnlyList<CreateOrderItem> Items);
