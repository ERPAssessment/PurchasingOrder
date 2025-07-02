using PurchasingOrder.Shared.Exceptions;

namespace PurchasingOrder.Application.Exceptions;
public class PurchaseOrderItemsCannotEmptyException : NotFoundException
{
  public PurchaseOrderItemsCannotEmptyException() : base("Purchase Order cannot be have empty items")
  {
  }
}
