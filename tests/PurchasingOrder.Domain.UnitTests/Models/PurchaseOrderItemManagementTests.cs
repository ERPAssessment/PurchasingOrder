namespace PurchasingOrder.Domain.UnitTests.Models;

public class PurchaseOrderItemManagementTests
{
  [Fact]
  public void AddPurchaseItem_ShouldAddItem_WhenOrderIsActiveAndItemIsNew()
  {
    // Arrange
    var order = CreateTestOrder();
    var serialNumber = PurchaseItemSerialNumber.Of("SN002");
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());
    var price = Money.Of(200m);

    // Act
    order.AddPurchaseItem(serialNumber, goodId, price);

    // Assert
    Assert.Equal(2, order.PurchaseItems.Count);
    var item = order.PurchaseItems[1];
    Assert.Equal(serialNumber, item.PurchaseItemSerialNumber);
    Assert.Equal(goodId, item.PurchaseGoodId);
    Assert.Equal(price, item.Price);
  }

  [Fact]
  public void AddPurchaseItem_ShouldThrowException_WhenSerialNumberAlreadyExists()
  {
    // Arrange
    var order = CreateTestOrder();
    var serialNumber = order.PurchaseItems[0].PurchaseItemSerialNumber;
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());
    var price = Money.Of(200m);

    // Act & Assert
    Assert.Throws<DomainException>(() => order.AddPurchaseItem(serialNumber, goodId, price));
  }


  [Fact]
  public void AddPurchaseItem_ShouldThrowException_WhenGoodIdAlreadyExists()
  {
    // Arrange
    var order = CreateTestOrder();
    var existingGoodId = order.PurchaseItems[0].PurchaseGoodId;
    var serialNumber = PurchaseItemSerialNumber.Of("SN002");
    var price = Money.Of(200m);

    // Act & Assert
    Assert.Throws<DomainException>(() => order.AddPurchaseItem(serialNumber, existingGoodId, price));
  }

  [Fact]
  public void AddPurchaseItem_ShouldThrowException_WhenOrderIsDeactivated()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Deactivate();
    var serialNumber = PurchaseItemSerialNumber.Of("SN002");
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());
    var price = Money.Of(200m);

    // Act & Assert
    Assert.Throws<DomainException>(() => order.AddPurchaseItem(serialNumber, goodId, price));
  }

  [Fact]
  public void TotalPrice_ShouldReturnSumOfItemPrices()
  {
    // Arrange
    var order = CreateTestOrder();
    var serialNumber2 = PurchaseItemSerialNumber.Of("SN002");
    var goodId2 = PurchaseGoodId.Of(Guid.NewGuid());
    var price2 = Money.Of(200m);
    order.AddPurchaseItem(serialNumber2, goodId2, price2);

    // Act
    var totalPrice = order.TotalPrice;

    // Assert
    Assert.Equal(Money.Of(300m), totalPrice);
  }
}
