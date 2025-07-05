using PurchasingOrder.Domain.Events;
using PurchasingOrder.Domain.Models;
using PurchasingOrder.Domain.ValueObjects;
using static PurchasingOrder.Domain.UnitTests.ModelData.DataSet;

namespace PurchasingOrder.Domain.UnitTests.Models;

public class PurchaseOrderDomainEvents
{
  [Fact]
  public void CreatePurchaseOrder_ShouldRaiseOrderCreatedEventWithCorrectOrder()
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
    var domainEvent = order.DomainEvents[0];
    Assert.IsType<OrderCreatedEvent>(domainEvent);
    var createdEvent = (OrderCreatedEvent)domainEvent;
    Assert.Equal(order, createdEvent.Order);
  }


  [Fact]
  public void Approve_ShouldRaiseOrderApprovedEventWithCorrectOrder()
  {
    // Arrange
    var order = CreateTestOrder();

    // Act
    order.Approve();

    // Assert
    var domainEvent = order.DomainEvents[1];
    Assert.IsType<OrderApprovedEvent>(domainEvent);
    var approvedEvent = (OrderApprovedEvent)domainEvent;
    Assert.Equal(order, approvedEvent.Order);
  }

  [Fact]
  public void Ship_ShouldRaiseOrderShippedEventWithCorrectOrder()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Approve();

    // Act
    order.Ship();

    // Assert
    var domainEvent = order.DomainEvents[2];
    Assert.IsType<OrderShippedEvent>(domainEvent);
    var shippedEvent = (OrderShippedEvent)domainEvent;
    Assert.Equal(order, shippedEvent.Order);
  }

  [Fact]
  public void Close_ShouldRaiseOrderClosedEventWithCorrectOrder()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Approve();
    order.Ship();

    // Act
    order.Close();

    // Assert
    var domainEvent = order.DomainEvents[3];
    Assert.IsType<OrderClosedEvent>(domainEvent);
    var closedEvent = (OrderClosedEvent)domainEvent;
    Assert.Equal(order, closedEvent.Order);
  }
}
