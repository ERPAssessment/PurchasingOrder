namespace PurchasingOrder.Application.PurchaseOrders.Commands.ApprovePurchaseOrder;

internal class ApprovePurchaseOrderHandler(IApplicationDbContext dbContext) :
   ICommandHandler<ApprovePurchaseOrderCommand, ApprovePurchaseOrderResult>
{
  public async Task<ApprovePurchaseOrderResult> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
  {
    PurchaseOrder order = await ApproveOrder(request.Id, cancellationToken);

    dbContext.PurchaseOrders.Update(order);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new ApprovePurchaseOrderResult(true);
  }

  private async Task<PurchaseOrder> ApproveOrder(Guid Id, CancellationToken cancellationToken)
  {
    var orderId = PurchaseOrderId.Of(Id);
    var order = await dbContext.PurchaseOrders
        .FindAsync([orderId], cancellationToken: cancellationToken);

    if (order is null)
    {
      throw new PurchaseOrderNotFoundException(Id.ToString());
    }

    order.Approve();

    return order;
  }
}
