namespace PurchasingOrder.Application.DTOs;
public record ValidatePurchaseOrderDto
  (
  string PurchaseOrderNumber,
  IEnumerable<ValidatePOItem> Items
  );


public record ValidatePOItem
  (
  string Id,
  string GoodCode,
  decimal Price
  );
