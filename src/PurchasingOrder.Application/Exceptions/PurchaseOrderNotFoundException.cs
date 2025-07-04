using ERP.Shared.Exceptions;

namespace PurchasingOrder.Application.Exceptions;
public class PurchaseOrderNotFoundException : NotFoundException
{
  public PurchaseOrderNotFoundException(string id) : base("PurchaseOrder", id)
  {
  }
}
