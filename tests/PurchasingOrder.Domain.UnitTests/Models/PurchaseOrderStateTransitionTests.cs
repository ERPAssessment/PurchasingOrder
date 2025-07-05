namespace PurchasingOrder.Domain.UnitTests.Models;

public class PurchaseOrderStateTransitionTests
{
  [Fact]
  public void Approve_ShouldSetStateToApproved_WhenStateIsCreatedAndActive()
  {
    // Arrange
    var order = CreateTestOrder();
    // Act
    order.Approve();

    // Assert
    Assert.Equal(PurchaseOrderState.Approved, order.DocumentState);
    Assert.Equal(2, order.DomainEvents.Count);
    Assert.IsType<OrderApprovedEvent>(order.DomainEvents[1]);
  }

  [Fact]
  public void Approve_ShouldThrowException_WhenStateIsNotCreated()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Approve();

    // Act & Assert
    Assert.Throws<DomainException>(() => order.Approve());
  }

  [Fact]
  public void Approve_ShouldThrowException_WhenOrderIsDeactivated()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Deactivate();

    // Act & Assert
    Assert.Throws<DomainException>(() => order.Approve());
  }

  [Fact]
  public void Ship_ShouldSetStateToBeingShipped_WhenStateIsApprovedAndActive()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Approve();

    // Act
    order.Ship();

    // Assert
    Assert.Equal(PurchaseOrderState.BeingShipped, order.DocumentState);
    Assert.Equal(3, order.DomainEvents.Count);
    Assert.IsType<OrderShippedEvent>(order.DomainEvents[2]);
  }

  [Fact]
  public void Ship_ShouldThrowException_WhenStateIsNotApproved()
  {
    // Arrange
    var order = CreateTestOrder();

    // Act & Assert
    Assert.Throws<DomainException>(() => order.Ship());
  }

  [Fact]
  public void Ship_ShouldThrowException_WhenOrderIsDeactivated()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Approve();
    order.Deactivate();

    // Act & Assert
    Assert.Throws<DomainException>(() => order.Ship());
  }

  [Fact]
  public void Close_ShouldSetStateToClosed_WhenStateIsBeingShippedAndActive()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Approve();
    order.Ship();

    // Act
    order.Close();

    // Assert
    Assert.Equal(PurchaseOrderState.Closed, order.DocumentState);
    Assert.Equal(4, order.DomainEvents.Count);
    Assert.IsType<OrderClosedEvent>(order.DomainEvents[3]);
  }

  [Fact]
  public void Close_ShouldThrowException_WhenStateIsNotBeingShipped()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Approve();

    // Act & Assert
    Assert.Throws<DomainException>(() => order.Close());
  }

  [Fact]
  public void Close_ShouldThrowException_WhenOrderIsDeactivated()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Approve();
    order.Ship();
    order.Deactivate();

    // Act & Assert
    Assert.Throws<DomainException>(() => order.Close());
  }
}
