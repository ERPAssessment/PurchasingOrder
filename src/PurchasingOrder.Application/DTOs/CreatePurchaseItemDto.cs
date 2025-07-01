namespace PurchasingOrder.Application.DTOs;

public record CreatePurchaseItemDto(
    Guid PurchaseGoodId,
    decimal Price
);