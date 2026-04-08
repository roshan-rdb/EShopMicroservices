using MassTransit;
using Microsoft.FeatureManagement;
namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandlers(IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandlers> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);

        if(await featureManager.IsEnabledAsync("OrderFullfillment"))
        {
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }

    }
}
