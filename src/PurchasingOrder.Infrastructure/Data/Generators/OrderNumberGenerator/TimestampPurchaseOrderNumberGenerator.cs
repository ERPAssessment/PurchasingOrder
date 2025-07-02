namespace PurchasingOrder.Infrastructure.Data.Generators;

internal static class TimestampPurchaseOrderNumberGenerator
{
  public static PurchaseOrderNumber Generate()
  {
    var now = DateTime.UtcNow;
    return PurchaseOrderNumber.Of($"PO_{now:yyyyMMddHHmmssfff}");
  }
}