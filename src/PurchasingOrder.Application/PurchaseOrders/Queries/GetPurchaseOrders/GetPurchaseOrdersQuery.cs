using ERP.Shared.CQRS;
using ERP.Shared.Pagination;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

public record GetPurchaseOrdersQuery(PaginationRequest PaginationRequest,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? State = null)

    : IQuery<GetPurchaseOrdersResults>;

public record GetPurchaseOrdersResults(PaginatedResult<PurchaseOrderDTO> Orders);