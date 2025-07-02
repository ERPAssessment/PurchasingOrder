using PurchasingOrder.Application.Data;

namespace PurchasingOrder.Infrastructure.Data.Generators.OrderItem;

public class GuidPurchaseItemSerialNumberGenerator : IPurchaseItemSerialNumberGenerator
{
  public PurchaseItemSerialNumber Generate()
  {
    return PurchaseItemSerialNumber.Of($"SN_{Guid.NewGuid()}");
  }
}