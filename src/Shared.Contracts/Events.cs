namespace Shared.Contracts;

public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public int ProductId { get; init; }
    public int Quantity { get; init; }
    public string UserId { get; init; }
}

public record InventoryUpdatedEvent
{
    public int ProductId { get; init; }
    public int NewStock { get; init; }
}
