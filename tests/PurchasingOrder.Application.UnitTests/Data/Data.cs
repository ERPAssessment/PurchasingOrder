namespace PurchasingOrder.Application.UnitTests.ModelData;

public static class DataSet
{
  public static PurchaseOrder CreateTestOrder(PurchaseOrderId id)
  {
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var issuedDate = DateTime.Now;
    var serialNumber = PurchaseItemSerialNumber.Of("SN001");
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());
    var price = Money.Of(100m);
    return PurchaseOrder.CreatePurchaseOrder(id, poNumber, issuedDate, serialNumber, goodId, price);
  }
}
