namespace Order.Application
{
    public record GetOrderItemDto(Guid ProductId, int Quantity);

    public record GetOrderResponse(
        Guid OrderId,
        string CustomerName,
        DateTimeOffset CreatedAt,
        IReadOnlyList<GetOrderItemDto> Items);
}
