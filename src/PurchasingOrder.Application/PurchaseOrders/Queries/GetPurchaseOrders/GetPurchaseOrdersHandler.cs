using PurchasingOrder.Application.Extenstions;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;
using PurchasingOrder.Domain.Enums;
using PurchasingOrder.Domain.Specifications.PurchaseOrderSpecs;
using PurchasingOrder.Domain.Specifications.Shared;
using PurchasingOrder.Shared.Pagination;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

public class GetPurchaseOrdersHandler(IReadPurchaseOrderRepository purchaseOrderRepository)
    : IQueryHandler<GetPurchaseOrdersQuery, GetPurchaseOrdersResults>
{
  public async Task<GetPurchaseOrdersResults> Handle(GetPurchaseOrdersQuery query, CancellationToken cancellationToken)
  {
    var pageIndex = query.PaginationRequest.PageIndex;
    var pageSize = query.PaginationRequest.PageSize;
    var specification = BuildSpecification(query);

    return await GetPurchaseOrdersData(specification, pageIndex, pageSize, cancellationToken);
  }

  private async Task<GetPurchaseOrdersResults> GetPurchaseOrdersData(
      Specification<PurchaseOrder>? specification,
      int pageIndex,
      int pageSize,
      CancellationToken cancellationToken)
  {
    IEnumerable<PurchaseOrder> orders;
    long totalCount;

    if (specification != null)
    {
      orders = await purchaseOrderRepository.FindAsync(specification, pageIndex, pageSize, cancellationToken);
      totalCount = orders.Count();
    }
    else
    {
      totalCount = await purchaseOrderRepository.GetTotalCountAsync(cancellationToken);
      orders = await purchaseOrderRepository.GetPagedPurchaseOrders(pageIndex, pageSize, cancellationToken);
    }

    var dtoList = orders.ToPurchaseOrdersDtoList();
    var paginatedResult = new PaginatedResult<PurchaseOrderDTO>(pageIndex, pageSize, totalCount, dtoList);

    return new GetPurchaseOrdersResults(paginatedResult);
  }


  private Specification<PurchaseOrder>? BuildSpecification(GetPurchaseOrdersQuery query)
  {
    var specifications = new List<Specification<PurchaseOrder>>();

    if (query.StartDate.HasValue || query.EndDate.HasValue)
    {
      specifications.Add(new PurchaseOrderByDateRangeSpecification(
          query.StartDate ?? DateTime.MinValue,
          query.EndDate ?? DateTime.MaxValue));
    }

    if (!string.IsNullOrEmpty(query.State)
      && Enum.TryParse<PurchaseOrderState>(query.State, true, out var stateEnum))
    {
      specifications.Add(new PurchaseOrderByStateSpecification(stateEnum));
    }

    if (specifications.Count == 0)
      return null;

    if (specifications.Count == 1)
      return specifications[0];

    var combinedSpec = specifications[0];
    for (int i = 1; i < specifications.Count; i++)
    {
      combinedSpec = combinedSpec.And(specifications[i]);
    }

    return combinedSpec;
  }
}