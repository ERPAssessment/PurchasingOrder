using PurchasingOrder.Domain.Models;
using PurchasingOrder.Domain.ValueObjects;

namespace PurchasingOrder.Domain.UnitTests.ModelData;

public static class DataSet
{
  public static PurchaseOrder CreateTestOrder()
  {
    var id = PurchaseOrderId.Of(Guid.NewGuid());
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var issuedDate = DateTime.Now;
    var serialNumber = PurchaseItemSerialNumber.Of("SN001");
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());
    var price = Money.Of(100m);
    return PurchaseOrder.CreatePurchaseOrder(id, poNumber, issuedDate, serialNumber, goodId, price);
  }
}
