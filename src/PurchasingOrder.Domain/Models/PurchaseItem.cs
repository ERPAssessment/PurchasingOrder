namespace PurchasingOrder.Domain.Models;
public class PurchaseItem : Entity<PurchaseItemId>
{
  internal PurchaseItem(
          PurchaseOrderId purchaseOrderId,
          PurchaseItemSerialNumber purchaseItemSerialNumber,
          PurchaseGoodId purchaseGoodId,
          Money price)
  {
    Id = PurchaseItemId.Of(Guid.NewGuid());
    PurchaseOrderId = purchaseOrderId;
    PurchaseGoodId = purchaseGoodId;
    PurchaseItemSerialNumber = purchaseItemSerialNumber;
    Price = price;
  }

  public PurchaseOrderId PurchaseOrderId { get; private set; } = default!;
  public PurchaseGoodId PurchaseGoodId { get; private set; } = default!;
  public PurchaseItemSerialNumber PurchaseItemSerialNumber { get; private set; } = default!;
  public Money Price { get; private set; } = default!;
}
