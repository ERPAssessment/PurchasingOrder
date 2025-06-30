namespace PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

internal class CreatePurchaseOrderHandler(IApplicationDbContext dbContext) :
   ICommandHandler<CreatePurchaseOrderCommand, CreatePurchaseOrderResult>
{
  public async Task<CreatePurchaseOrderResult> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
  {
    var order = CreateNewOrder(request.Order);

    dbContext.PurchaseOrders.Add(order);
    await dbContext.SaveChangesAsync(cancellationToken);

    return new CreatePurchaseOrderResult(order.Id.Value);
  }

  private PurchaseOrder CreateNewOrder(CreatePurchaseOrderDto order)
  {
    var PO = PurchaseOrder.CreatePurchaseOrder(PurchaseOrderId.Of(Guid.NewGuid()),
                                             PurchaseOrderNumber.Of(Guid.NewGuid().ToString()), // Will be a generator
                                             DateTime.UtcNow
                                             );

    foreach (var ItemDto in order.PurchaseItems)
    {
      PO.AddPurchaseItem(PurchaseItemSerialNumber.Of(Guid.NewGuid().ToString()),   // will be a generator
                         PurchaseGoodId.Of(ItemDto.PurchaseGoodId),
                         Money.Of(ItemDto.Price));
    }

    return PO;
  }
}
