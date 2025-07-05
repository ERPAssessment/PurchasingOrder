namespace PurchasingOrder.Domain.UnitTests.Models;

public class PurchaseOrderCreationTests
{
  [Fact]
  public void CreatePurchaseOrder_ShouldThrowException_WhenMoneyIsNegative()
  {
    // Arrange
    var id = PurchaseOrderId.Of(Guid.NewGuid());
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var issuedDate = DateTime.Now;
    var serialNumber = PurchaseItemSerialNumber.Of("SN001");
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());

    // Act & Assert
    Assert.Throws<ArgumentException>(() => PurchaseOrder.CreatePurchaseOrder(id, poNumber, issuedDate, serialNumber, goodId, Money.Of(-100m)));
  }

  [Fact]
  public void CreatePurchaseOrder_ShouldThrowException_WhenPurchaseOrderIdIsEmpty()
  {
    // Arrange
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var issuedDate = DateTime.Now;
    var serialNumber = PurchaseItemSerialNumber.Of("SN001");
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());
    var price = Money.Of(100m);

    // Act & Assert
    Assert.Throws<DomainException>(() => PurchaseOrder.CreatePurchaseOrder(PurchaseOrderId.Of(Guid.Empty), poNumber, issuedDate, serialNumber, goodId, price));
  }

  [Fact]
  public void CreatePurchaseOrder_ShouldCreateOrderWithOneItem()
  {
    // Arrange
    var id = PurchaseOrderId.Of(Guid.NewGuid());
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var issuedDate = DateTime.Now;
    var serialNumber = PurchaseItemSerialNumber.Of("SN001");
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());
    var price = Money.Of(100m);

    // Act
    var order = PurchaseOrder.CreatePurchaseOrder(id, poNumber, issuedDate, serialNumber, goodId, price);

    // Assert
    Assert.Equal(id, order.Id);
    Assert.Equal(poNumber, order.PONumber);
    Assert.Equal(issuedDate, order.IssuedDate);
    Assert.Equal(PurchaseOrderState.Created, order.DocumentState);
    Assert.Equal(PurchaseDocumentStatus.Active, order.DocumentStatus);
    Assert.Single(order.PurchaseItems);
    var item = order.PurchaseItems[0];
    Assert.Equal(serialNumber, item.PurchaseItemSerialNumber);
    Assert.Equal(goodId, item.PurchaseGoodId);
    Assert.Equal(price, item.Price);
    Assert.Single(order.DomainEvents);
    Assert.IsType<OrderCreatedEvent>(order.DomainEvents[0]);
  }
}