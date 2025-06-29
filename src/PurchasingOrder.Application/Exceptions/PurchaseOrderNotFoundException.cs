using PurchasingOrder.Shared.Exceptions;

namespace PurchasingOrder.Application.Exceptions;
public class PurchaseOrderNotFoundException : NotFoundException
{
  public PurchaseOrderNotFoundException(Guid id) : base("PurchaseOrder", id)
  {
  }
}
