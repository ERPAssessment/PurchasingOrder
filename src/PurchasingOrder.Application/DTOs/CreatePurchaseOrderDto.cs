namespace PurchasingOrder.Application.DTOs;

public record CreatePurchaseOrderDto(
    List<PurchaseItemDto> PurchaseItems
);
