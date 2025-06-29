namespace PurchasingOrder.Application.DTOs;

public record PurchaseGoodDto(
    Guid Id,
    string Code,
    string Name
);
