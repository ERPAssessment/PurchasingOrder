using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

namespace PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

internal class ChangePurchaseOrderStatusHandler(IWritePurchaseOrderRepository OrderRepository) :
   ICommandHandler<ChangePurchaseOrderStatusCommand, ChangePurchaseOrderStatusResult>
{
  public async Task<ChangePurchaseOrderStatusResult> Handle(ChangePurchaseOrderStatusCommand request, CancellationToken cancellationToken)
  {
    PurchaseOrder order = await ChangeOrderStatus(request.PurchaseOrderStatus, cancellationToken);

    await OrderRepository.Update(order, cancellationToken);

    return new ChangePurchaseOrderStatusResult(true);
  }

  private async Task<PurchaseOrder> ChangeOrderStatus(ChangePurchaseOrderStatusDto Dto, CancellationToken cancellationToken)
  {
    var orderId = PurchaseOrderId.Of(Dto.PurchaseOrderId);

    var order = await OrderRepository.GetById(orderId, cancellationToken);

    if (order is null)
    {
      throw new PurchaseOrderNotFoundException(orderId.ToString());
    }

    if (Dto.IsActive)
    {
      order.Reactivate();
    }
    else
    {
      order.Deactivate();
    }

    return order;
  }
}
