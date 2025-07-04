using PurchasingOrder.Domain.Enums;

namespace PurchasingOrder.Application.DTOs;

public record UpdatePurchaseOrderStateDto(
    string PurchaseOrderNumber,
    PurchaseOrderState State
);