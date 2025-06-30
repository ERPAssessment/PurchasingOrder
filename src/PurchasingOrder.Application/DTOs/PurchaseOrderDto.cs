namespace PurchasingOrder.Application.DTOs;

public record PurchaseOrderDTO(
    Guid Id,
    DateTime IssuedDate,
    string PurchaseOrderNumber,
    string DocumentState,
    string DocumentStatus,
    decimal TotalPrice,
    List<PurchaseItemDto> PurchaseItems
);
