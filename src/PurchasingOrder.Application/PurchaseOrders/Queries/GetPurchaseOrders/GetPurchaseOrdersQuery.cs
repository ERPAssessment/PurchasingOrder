using PurchasingOrder.Shared.Pagination;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

public record GetPurchaseOrdersQuery(PaginationRequest PaginationRequest)
    : IQuery<GetPurchaseOrdersResults>;

public record GetPurchaseOrdersResults(PaginatedResult<PurchaseOrderDTO> Orders);