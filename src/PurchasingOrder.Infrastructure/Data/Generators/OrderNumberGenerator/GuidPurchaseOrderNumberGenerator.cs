namespace PurchasingOrder.Infrastructure.Data.Generators;

internal static class GuidPurchaseOrderNumberGenerator
{
  public static PurchaseOrderNumber Generate()
  {
    return PurchaseOrderNumber.Of($"PO_{Guid.NewGuid()}");
  }
}