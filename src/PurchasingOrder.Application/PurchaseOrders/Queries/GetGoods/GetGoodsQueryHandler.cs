using ERP.Shared.Pagination;
using PurchasingOrder.Application.Extenstions;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseGoodRepo;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetGoods;

internal class GetPurchaseOrdersQueryHandler(IReadPurchaseGoodRepository PurchaseGoodRepository)
    : IQueryHandler<GetGoodsQuery, GetGoodsResult>
{
  public async Task<GetGoodsResult> Handle(GetGoodsQuery query, CancellationToken cancellationToken)
  {
    var pageIndex = query.PaginationRequest.PageIndex;
    var pageSize = query.PaginationRequest.PageSize;

    var totalCount = await PurchaseGoodRepository.GetTotalCountAsync(cancellationToken);

    var goods = await PurchaseGoodRepository.GetPagedGoodsAsync(pageIndex, pageSize, cancellationToken);

    return new GetGoodsResult(
        new PaginatedResult<PurchaseGoodDto>(
            pageIndex,
            pageSize,
            totalCount,
            goods.ToGoodsDtoList()));
  }
}

