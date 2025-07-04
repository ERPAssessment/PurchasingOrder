using ERP.Shared.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

namespace PurchasingOrder.Application.PurchaseOrders.EventHandlers.Integration;

public class ShippingOrderCreatedEventHandler(ISender sender, ILogger<SHOCreatedEvent> logger)
    : IConsumer<SHOCreatedEvent>

{
  public async Task Consume(ConsumeContext<SHOCreatedEvent> context)
  {
    logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

    var command = MapToUpdateOrderCommand(context.Message);
    await sender.Send(command);
  }

  private UpdatePurchaseOrderStateCommand MapToUpdateOrderCommand(SHOCreatedEvent message)
  {
    UpdatePurchaseOrderStateDto updateDTO = new(message.PurchaseOrderNumber,
                                                PurchasingOrder.Domain.Enums.PurchaseOrderState.BeingShipped);
    return new UpdatePurchaseOrderStateCommand(updateDTO);
  }
}
