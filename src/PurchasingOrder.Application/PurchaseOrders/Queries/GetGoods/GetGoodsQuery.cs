using ERP.Shared.CQRS;
using ERP.Shared.Pagination;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetGoods;

public record GetGoodsQuery(PaginationRequest PaginationRequest)
    : IQuery<GetGoodsResult>;

public record GetGoodsResult(PaginatedResult<PurchaseGoodDto> Goods);