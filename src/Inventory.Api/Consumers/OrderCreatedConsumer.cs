using MassTransit;
using Shared.Contracts;
using Inventory.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly InventoryDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCreatedConsumer(InventoryDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        var stock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.ProductId == message.ProductId);

        if (stock != null && stock.Quantity >= message.Quantity)
        {
            stock.Quantity -= message.Quantity;
            await _dbContext.SaveChangesAsync();

            await _publishEndpoint.Publish(new InventoryUpdatedEvent
            {
                ProductId = stock.ProductId,
                NewStock = stock.Quantity
            });
        }
    }
}
