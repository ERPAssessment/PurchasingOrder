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
    if (order.PurchaseItems.Count < 1)
    {
      throw new PurchaseOrderItemsCannotEmptyException();
    }

    var FirstItem = order.PurchaseItems[0];

    var PO = PurchaseOrder.CreatePurchaseOrder(PurchaseOrderId.Of(Guid.NewGuid()),
                                               await PurchaseOrderNumberGenerator.Generate(),
                                               DateTime.UtcNow,
                                               SerialNumberGenerator.Generate(),
                                               PurchaseGoodId.Of(FirstItem.PurchaseGoodId),
                                               Money.Of(FirstItem.Price)
                                             );


    order.PurchaseItems.Remove(FirstItem);

    foreach (var ItemDto in order.PurchaseItems)
    {
      PO.AddPurchaseItem(SerialNumberGenerator.Generate(),
                         PurchaseGoodId.Of(ItemDto.PurchaseGoodId),
                         Money.Of(ItemDto.Price));
    }
    return PO;
  }
}
