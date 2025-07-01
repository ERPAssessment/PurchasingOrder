namespace PurchasingOrder.Application.DTOs;

public record ChangePurchaseOrderStatusDto(
    Guid PurchaseOrderId,
    bool IsActive
);