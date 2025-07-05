using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

namespace PurchasingOrder.Application.PurchaseOrders.Commands.ApprovePurchaseOrder;

internal class ApprovePurchaseOrderHandler(IWritePurchaseOrderRepository OrderRepository) :
   ICommandHandler<ApprovePurchaseOrderCommand, ApprovePurchaseOrderResult>
{
  public async Task<ApprovePurchaseOrderResult> Handle(ApprovePurchaseOrderCommand request, CancellationToken cancellationToken)
  {
    PurchaseOrder order = await ApproveOrder(request.Id, cancellationToken);

    await OrderRepository.Update(order, cancellationToken);

    return new ApprovePurchaseOrderResult(true);
  }

  private async Task<PurchaseOrder> ApproveOrder(Guid Id, CancellationToken cancellationToken)
  {
    var orderId = PurchaseOrderId.Of(Id);

    var order = await OrderRepository.GetById(orderId, cancellationToken);

    if (order is null)
    {
      throw new PurchaseOrderNotFoundException(Id.ToString());
    }

    order.Approve();

    return order;
  }
}
