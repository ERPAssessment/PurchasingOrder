using PurchasingOrder.Shared.Exceptions;

namespace PurchasingOrder.Application.Exceptions;
public class InvalidPurchaseOrderIdFormatException : NotFoundException
{
  public InvalidPurchaseOrderIdFormatException(string id) : base("PurchaseOrderIdFormat", id)
  {
  }
}
