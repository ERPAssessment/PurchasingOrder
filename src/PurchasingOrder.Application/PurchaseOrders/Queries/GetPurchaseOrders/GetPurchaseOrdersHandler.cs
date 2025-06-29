using Microsoft.EntityFrameworkCore;
using PurchasingOrder.Application.Extenstions;
using PurchasingOrder.Shared.Pagination;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

public class GetPurchaseOrdersHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPurchaseOrdersQuery, GetPurchaseOrdersResults>
{
  public async Task<GetPurchaseOrdersResults> Handle(GetPurchaseOrdersQuery query, CancellationToken cancellationToken)
  {
    var pageIndex = query.PaginationRequest.PageIndex;
    var pageSize = query.PaginationRequest.PageSize;

    var totalCount = await dbContext.PurchaseOrders.LongCountAsync(cancellationToken);

    var orders = await dbContext.PurchaseOrders
                   .Include(o => o.PurchaseItems)
                   .OrderByDescending(o => o.IssuedDate)
                   .Skip(pageSize * pageIndex)
                   .Take(pageSize)
                   .ToListAsync(cancellationToken);

    return new GetPurchaseOrdersResults(
        new PaginatedResult<PurchaseOrderDTO>(
            pageIndex,
            pageSize,
            totalCount,
            orders.ToPurchaseOrdersDtoList()));
  }
}

