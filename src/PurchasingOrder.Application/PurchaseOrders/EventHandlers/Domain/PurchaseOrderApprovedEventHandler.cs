using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PurchasingOrder.Domain.Events;

namespace PurchasingOrder.Application.PurchaseOrders.EventHandlers.Domain;

public class PurchaseOrderApprovedEventHandler
 (IPublishEndpoint publishEndpoint,
   ILogger<PurchaseOrderApprovedEventHandler> logger)
    : INotificationHandler<OrderApprovedEvent>
{
  public Task Handle(OrderApprovedEvent notification, CancellationToken cancellationToken)
  {
    return Task.CompletedTask;
    //throw new NotImplementedException();
  }
}