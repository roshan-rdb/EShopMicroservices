namespace Ordering.Application.Orders.EventHandlers;

public class OrderUpdatedEventHandlers(ILogger<OrderUpdatedEventHandlers> logger) : INotificationHandler<OrderUpdatedEvent>
{
    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;

    }
}
