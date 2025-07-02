using PurchasingOrder.Domain.Specifications.Shared;
using System.Linq.Expressions;

namespace PurchasingOrder.Domain.Specifications.PurchaseOrderSpecs;

public class PurchaseOrderByStateSpecification : Specification<PurchaseOrder>
{
  private readonly PurchaseOrderState _state;

  public PurchaseOrderByStateSpecification(PurchaseOrderState state)
  {
    _state = state;
  }

  public override Expression<Func<PurchaseOrder, bool>> ToExpression()
  {
    return po => po.DocumentState == _state;
  }
}
