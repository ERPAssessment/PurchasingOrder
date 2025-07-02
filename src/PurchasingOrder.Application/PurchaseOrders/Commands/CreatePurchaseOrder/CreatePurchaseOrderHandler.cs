using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

namespace PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

internal class CreatePurchaseOrderHandler(
  IWritePurchaseOrderRepository OrderRepository,
  IPurchaseItemSerialNumberGenerator SerialNumberGenerator,
  IPurchaseOrderNumberGenerator PurchaseOrderNumberGenerator
  ) :
   ICommandHandler<CreatePurchaseOrderCommand, CreatePurchaseOrderResult>
{
  public async Task<CreatePurchaseOrderResult> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
  {
    var orders = new List<PurchaseOrder>();

    foreach (var orderDto in request.Orders)
    {
      var order = await CreateNewOrder(orderDto);
      orders.Add(order);
    }

    await OrderRepository.Add(orders, cancellationToken);

    var orderIds = orders.Select(o => o.Id.Value).ToList();
    return new CreatePurchaseOrderResult(orderIds);
  }

  private async Task<PurchaseOrder> CreateNewOrder(CreatePurchaseOrderDto order)
  {
    var PO = PurchaseOrder.CreatePurchaseOrder(PurchaseOrderId.Of(Guid.NewGuid()),
                                               await PurchaseOrderNumberGenerator.Generate(),
                                               DateTime.UtcNow
                                             );
    foreach (var ItemDto in order.PurchaseItems)
    {
      PO.AddPurchaseItem(SerialNumberGenerator.Generate(),
                         PurchaseGoodId.Of(ItemDto.PurchaseGoodId),
                         Money.Of(ItemDto.Price));
    }
    return PO;
  }
}
