namespace PurchasingOrder.Application.Extenstions;

public static class PurchaseOrdersExtenstion
{
  public static IEnumerable<PurchaseOrderDTO> ToPurchaseOrdersDtoList(this IEnumerable<PurchaseOrder> ordersDB)
  {
    return ordersDB.Select(order => order.ToPurchaseOrderDto());
  }
  public static PurchaseOrderDTO ToPurchaseOrderDto(this PurchaseOrder order)
  {
    return new PurchaseOrderDTO(
        Id: order.Id.Value,
        PONumber: order.PONumber.Value,
        IssuedDate: order.IssuedDate,
        DocumentState: order.DocumentState.ToString(),
        DocumentStatus: order.DocumentStatus.ToString(),
        TotalPrice: order.TotalPrice.Amount,
        PurchaseItems: order.PurchaseItems.Select(item => new PurchaseItemDto(
            Id: item.Id.Value,
            PurchaseOrderId: item.PurchaseOrderId.Value,
            PurchaseGoodId: item.PurchaseGoodId.Value,
            SerialNumber: item.PurchaseItemSerialNumber.SerialNumber,
            Price: item.Price.Amount
        )).ToList()
    );
  }

}
