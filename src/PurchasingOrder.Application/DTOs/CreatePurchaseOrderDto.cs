namespace PurchasingOrder.Application.DTOs;

public record CreatePurchaseOrderDto(
    List<CreatePurchaseItemDto> PurchaseItems
);
