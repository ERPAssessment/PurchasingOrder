using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PurchasingOrder.Domain.Events;

namespace PurchasingOrder.Application.EventHandlers.Domain;

public class PurchaseOrderCreatedEventHandler
  (IPublishEndpoint publishEndpoint,
   ILogger<PurchaseOrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
  public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}
