namespace PurchasingOrder.Application.DTOs;

public record PurchaseItemDto(
    Guid Id,
    Guid PurchaseOrderId,
    Guid PurchaseGoodId,
    string SerialNumber,
    decimal Price
);