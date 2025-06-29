using Microsoft.EntityFrameworkCore;
using PurchasingOrder.Application.Extenstions;
using PurchasingOrder.Shared.Pagination;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetGoods;

public class GetGoodsHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetGoodsQuery, GetGoodsResult>
{
  public async Task<GetGoodsResult> Handle(GetGoodsQuery query, CancellationToken cancellationToken)
  {
    // get orders with pagination
    // return result

    var pageIndex = query.PaginationRequest.PageIndex;
    var pageSize = query.PaginationRequest.PageSize;

    var totalCount = await dbContext.PurchaseGoods.LongCountAsync(cancellationToken);

    var goods = await dbContext.PurchaseGoods
                   .OrderBy(o => o.Name)
                   .Skip(pageSize * pageIndex)
                   .Take(pageSize)
                   .ToListAsync(cancellationToken);

    return new GetGoodsResult(
        new PaginatedResult<PurchaseGoodDto>(
            pageIndex,
            pageSize,
            totalCount,
            goods.ToGoodsDtoList()));
  }
}

