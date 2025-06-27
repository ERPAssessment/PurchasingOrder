namespace PurchasingOrder.Domain.Models;
public class PurchaseItem : Entity<PurchaseItemId>
{
  internal PurchaseItem(PurchaseOrderId orderId, PurchaseItemSerialNumber serialNumber, PurchaseItemCode code, Money price)
  {
    Id = PurchaseItemId.Of(Guid.NewGuid());
    OrderId = orderId;
    SerialNumber = serialNumber;
    Code = code;
    Price = price;
  }

  public PurchaseOrderId OrderId { get; private set; } = default!;
  public PurchaseItemSerialNumber SerialNumber { get; private set; } = default!;
  public PurchaseItemCode Code { get; private set; } = default!;
  public Money Price { get; private set; } = default!;
}
