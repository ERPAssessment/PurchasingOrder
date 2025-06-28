namespace PurchasingOrder.Shared.Pagination;
public record PaginationRequest(int PageIndex = 0, int PageSize = 10);
