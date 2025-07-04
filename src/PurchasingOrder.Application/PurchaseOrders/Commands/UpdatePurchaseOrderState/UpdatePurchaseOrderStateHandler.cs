using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

namespace PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

internal class UpdatePurchaseOrderStateHandler(IWritePurchaseOrderRepository OrderRepository) :
   ICommandHandler<UpdatePurchaseOrderStateCommand, UpdatePurchaseOrderStateResult>
{
  public async Task<UpdatePurchaseOrderStateResult> Handle(UpdatePurchaseOrderStateCommand request, CancellationToken cancellationToken)
  {
    PurchaseOrder order = await ChangeOrderState(request.PurchaseOrderState, cancellationToken);

    await OrderRepository.Update(order, cancellationToken);

    return new UpdatePurchaseOrderStateResult(true);
  }

  private async Task<PurchaseOrder> ChangeOrderState(UpdatePurchaseOrderStateDto Dto, CancellationToken cancellationToken)
  {
    var PoNumber = PurchaseOrderNumber.Of(Dto.PurchaseOrderNumber);

    var order = await OrderRepository.GetByPurchaseNumber(PoNumber, cancellationToken);

    if (order is null)
    {
      throw new PurchaseOrderNotFoundException(Dto.PurchaseOrderNumber);
    }


    Action stateAction = Dto.State switch
    {
      Domain.Enums.PurchaseOrderState.Approved => order.Approve,
      Domain.Enums.PurchaseOrderState.BeingShipped => order.Ship,
      Domain.Enums.PurchaseOrderState.Closed => order.Close,
      _ => throw new InvalidOperationException($"Invalid state transition from {order.DocumentState} to {Dto.State}.")
    };

    stateAction.Invoke();

    return order;
  }
}
