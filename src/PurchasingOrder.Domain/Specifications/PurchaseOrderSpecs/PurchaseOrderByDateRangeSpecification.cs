using PurchasingOrder.Domain.Specifications.Shared;
using System.Linq.Expressions;

namespace PurchasingOrder.Domain.Specifications.PurchaseOrderSpecs;

public class PurchaseOrderByDateRangeSpecification : Specification<PurchaseOrder>
{
  private readonly DateTime _startDate;
  private readonly DateTime _endDate;

  public PurchaseOrderByDateRangeSpecification(DateTime startDate, DateTime endDate)
  {
    if (startDate > endDate)
      throw new DomainException("Start date cannot be later than end date.");

    _startDate = startDate;
    _endDate = endDate;
  }

  public override Expression<Func<PurchaseOrder, bool>> ToExpression()
  {
    return po => po.IssuedDate >= _startDate && po.IssuedDate <= _endDate;
  }
}
