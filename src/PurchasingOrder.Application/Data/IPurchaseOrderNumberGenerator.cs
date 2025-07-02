namespace PurchasingOrder.Application.Data;

public interface IPurchaseOrderNumberGenerator
{
  Task<PurchaseOrderNumber> Generate();
}
