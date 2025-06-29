using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PurchasingOrder.Domain.Events;

namespace PurchasingOrder.Application.EventHandlers.Domain;

public class PurchaseOrderApprovedEventHandler
 (IPublishEndpoint publishEndpoint,
   ILogger<PurchaseOrderApprovedEventHandler> logger)
    : INotificationHandler<OrderApprovedEvent>
{
  public Task Handle(OrderApprovedEvent notification, CancellationToken cancellationToken)
  {
    throw new NotImplementedException();
  }
}