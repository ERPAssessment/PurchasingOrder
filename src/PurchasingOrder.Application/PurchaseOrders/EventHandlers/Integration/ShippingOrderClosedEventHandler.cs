using ERP.Shared.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

namespace PurchasingOrder.Application.PurchaseOrders.EventHandlers.Integration;

public class ShippingOrderClosedEventHandler(ISender sender, ILogger<SHOClosedEvent> logger)
    : IConsumer<SHOClosedEvent>

{
  public async Task Consume(ConsumeContext<SHOClosedEvent> context)
  {
    logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

    var command = MapToUpdateOrderCommand(context.Message);
    await sender.Send(command);
  }

  private UpdatePurchaseOrderStateCommand MapToUpdateOrderCommand(SHOClosedEvent message)
  {
    UpdatePurchaseOrderStateDto updateDTO = new(message.PurchaseOrderNumber,
                                                PurchasingOrder.Domain.Enums.PurchaseOrderState.Closed);
    return new UpdatePurchaseOrderStateCommand(updateDTO);
  }
}
