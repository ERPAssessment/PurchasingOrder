namespace PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

internal class ChangePurchaseOrderStatusHandler(IApplicationDbContext dbContext) :
   ICommandHandler<ChangePurchaseOrderStatusCommand, ChangePurchaseOrderStatusResult>
{
  public async Task<ChangePurchaseOrderStatusResult> Handle(ChangePurchaseOrderStatusCommand request, CancellationToken cancellationToken)
  {
    PurchaseOrder order = await ApproveOrder(request.PurchaseOrderStatus, cancellationToken);

    dbContext.PurchaseOrders.Update(order);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new ChangePurchaseOrderStatusResult(true);
  }

  private async Task<PurchaseOrder> ApproveOrder(ChangePurchaseOrderStatusDto Dto, CancellationToken cancellationToken)
  {
    var orderId = PurchaseOrderId.Of(Dto.PurchaseOrderId);
    var order = await dbContext.PurchaseOrders
        .FindAsync([orderId], cancellationToken: cancellationToken);

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
