using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseGoodRepo;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

namespace PurchasingOrder.Application.PurchaseOrders.Commands.ValidatePurchaseOrder;

internal class ValidatePurchaseOrderCommandHandler(
  IReadPurchaseOrderRepository OrderRepository,
  IReadPurchaseGoodRepository GoodRepository) :
ICommandHandler<ValidatePurchaseOrderCommand, ValidatePurchaseOrderResult>
{
  public async Task<ValidatePurchaseOrderResult> Handle(ValidatePurchaseOrderCommand request, CancellationToken cancellationToken)
  {
    var orderNumber = PurchaseOrderNumber.Of(request.PODto.PurchaseOrderNumber);
    var pOrder = await OrderRepository.GetByPurchaseOrderNumber(orderNumber, cancellationToken);

    if (pOrder is null)
    {
      return new ValidatePurchaseOrderResult(false, $"Purchase order {orderNumber} not found");
    }

    if (pOrder.DocumentStatus == Domain.Enums.PurchaseDocumentStatus.Deactive)
    {
      return new ValidatePurchaseOrderResult(false, $"Purchase order has been deactivated.");
    }

    foreach (var item in request.PODto.Items!)
    {
      if (item is null)
      {
        return new ValidatePurchaseOrderResult(false, "Invalid item in purchase order");
      }

      if (!Guid.TryParse(item.Id, out var itemId))
      {
        return new ValidatePurchaseOrderResult(false, $"Invalid item ID: {item.Id}");
      }

      var pItem = pOrder.PurchaseItems.FirstOrDefault(x => x.Id == PurchaseItemId.Of(itemId));
      if (pItem is null)
      {
        return new ValidatePurchaseOrderResult(false, $"Item {itemId} not found in purchase order");
      }

      // Floor to 1 decimal place for price comparison
      var flooredItemPrice = Math.Floor(item.Price * 10) / 10;
      var flooredStoredPrice = Math.Floor(pItem.Price.Amount * 10) / 10;

      if (flooredStoredPrice != flooredItemPrice)
      {
        return new ValidatePurchaseOrderResult(false,
            $"Price mismatch for item {itemId}. Expected: {flooredStoredPrice:F1}, Received: {flooredItemPrice:F1}");
      }

      if (string.IsNullOrWhiteSpace(item.GoodCode))
      {
        return new ValidatePurchaseOrderResult(false, $"Invalid good code for item {itemId}");
      }

      var pGood = await GoodRepository.GetByPurchaseGoodByCode(PurchaseGoodCode.Of(item.GoodCode), cancellationToken);
      if (pGood is null)
      {
        return new ValidatePurchaseOrderResult(false, $"Good {item.GoodCode} not found for item {itemId}");
      }
    }

    return new ValidatePurchaseOrderResult(true, "Purchase order validated successfully");
  }
}
