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
  public async Task Handle(OrderApprovedEvent domainEvent, CancellationToken cancellationToken)
  {
    _ = publishEndpoint;
    logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
    await Task.CompletedTask;
  }
}