using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PurchasingOrder.Domain.Events;

namespace PurchasingOrder.Application.PurchaseOrders.EventHandlers.Domain;

public class PurchaseOrderCreatedEventHandler
  (IPublishEndpoint publishEndpoint,
   ILogger<PurchaseOrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
  public Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
  }
}
