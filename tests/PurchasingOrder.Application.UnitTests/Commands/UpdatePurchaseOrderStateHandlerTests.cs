using PurchasingOrder.Application.DTOs;
using PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

namespace PurchasingOrder.Application.UnitTests.Commands;

public class UpdatePurchaseOrderStateHandlerTests
{
  private readonly Mock<IWritePurchaseOrderRepository> _repositoryMock;
  private readonly UpdatePurchaseOrderStateCommandHandler _handler;

  public UpdatePurchaseOrderStateHandlerTests()
  {
    _repositoryMock = new Mock<IWritePurchaseOrderRepository>();
    _handler = new UpdatePurchaseOrderStateCommandHandler(_repositoryMock.Object);
  }

  private PurchaseOrder CreateTestOrder(PurchaseOrderNumber poNumber)
  {
    var id = PurchaseOrderId.Of(Guid.NewGuid());
    var issuedDate = DateTime.UtcNow;
    var serialNumber = PurchaseItemSerialNumber.Of("SN001");
    var goodId = PurchaseGoodId.Of(Guid.NewGuid());
    var price = Money.Of(100m);
    return PurchaseOrder.CreatePurchaseOrder(id, poNumber, issuedDate, serialNumber, goodId, price);
  }

  [Fact]
  public async Task Handle_ShouldSetApprovedStateAndReturnSuccess_WhenOrderExists()
  {
    // Arrange
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var command = new UpdatePurchaseOrderStateCommand(new UpdatePurchaseOrderStateDto("PO123", PurchaseOrderState.Approved));
    var order = CreateTestOrder(poNumber);
    _repositoryMock.Setup(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Result);
    _repositoryMock.Verify(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.Is<PurchaseOrder>(o =>
        o.PONumber == poNumber && o.DocumentState == PurchaseOrderState.Approved), It.IsAny<CancellationToken>()), Times.Once());
    Assert.Equal(PurchaseOrderState.Approved, order.DocumentState);
    Assert.Contains(order.DomainEvents, e => e is OrderApprovedDomainEvent && ((OrderApprovedDomainEvent)e).Order == order);
  }

  [Fact]
  public async Task Handle_ShouldSetBeingShippedStateAndReturnSuccess_WhenOrderExists()
  {
    // Arrange
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var command = new UpdatePurchaseOrderStateCommand(new UpdatePurchaseOrderStateDto("PO123", PurchaseOrderState.BeingShipped));
    var order = CreateTestOrder(poNumber);
    order.Approve(); // Required for valid transition to BeingShipped
    _repositoryMock.Setup(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Result);
    _repositoryMock.Verify(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.Is<PurchaseOrder>(o =>
        o.PONumber == poNumber && o.DocumentState == PurchaseOrderState.BeingShipped), It.IsAny<CancellationToken>()), Times.Once());
    Assert.Equal(PurchaseOrderState.BeingShipped, order.DocumentState);
    Assert.Contains(order.DomainEvents, e => e is OrderShippedDomainEvent && ((OrderShippedDomainEvent)e).Order == order);
  }

  [Fact]
  public async Task Handle_ShouldSetClosedStateAndReturnSuccess_WhenOrderExists()
  {
    // Arrange
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var command = new UpdatePurchaseOrderStateCommand(new UpdatePurchaseOrderStateDto("PO123", PurchaseOrderState.Closed));
    var order = CreateTestOrder(poNumber);
    order.Approve(); // Required for valid transition
    order.Ship();    // Required for valid transition to Closed
    _repositoryMock.Setup(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Result);
    _repositoryMock.Verify(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.Is<PurchaseOrder>(o =>
        o.PONumber == poNumber && o.DocumentState == PurchaseOrderState.Closed), It.IsAny<CancellationToken>()), Times.Once());
    Assert.Equal(PurchaseOrderState.Closed, order.DocumentState);
    Assert.Contains(order.DomainEvents, e => e is OrderClosedDomainEvent && ((OrderClosedDomainEvent)e).Order == order);
  }

  [Fact]
  public async Task Handle_ShouldThrowPurchaseOrderNotFoundException_WhenOrderDoesNotExist()
  {
    // Arrange
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var command = new UpdatePurchaseOrderStateCommand(new UpdatePurchaseOrderStateDto("PO123", PurchaseOrderState.Approved));
    _repositoryMock.Setup(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>())).ReturnsAsync((PurchaseOrder)null!);

    // Act & Assert
    await Assert.ThrowsAsync<PurchaseOrderNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    _repositoryMock.Verify(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.IsAny<PurchaseOrder>(), It.IsAny<CancellationToken>()), Times.Never());
  }

  [Fact]
  public async Task Handle_ShouldThrowDomainException_WhenTransitionIsInvalid()
  {
    // Arrange
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var command = new UpdatePurchaseOrderStateCommand(new UpdatePurchaseOrderStateDto("PO123", PurchaseOrderState.BeingShipped));
    var order = CreateTestOrder(poNumber); // In Created state, cannot transition directly to BeingShipped
    _repositoryMock.Setup(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act & Assert
    await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
    _repositoryMock.Verify(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.IsAny<PurchaseOrder>(), It.IsAny<CancellationToken>()), Times.Never());
  }

  [Fact]
  public async Task Handle_ShouldThrowInvalidOperationException_WhenStateIsInvalid()
  {
    // Arrange
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var invalidState = (PurchaseOrderState)999; // Invalid enum value
    var command = new UpdatePurchaseOrderStateCommand(new UpdatePurchaseOrderStateDto("PO123", invalidState));
    var order = CreateTestOrder(poNumber);
    _repositoryMock.Setup(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    _repositoryMock.Verify(r => r.GetByPurchaseNumber(poNumber, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.IsAny<PurchaseOrder>(), It.IsAny<CancellationToken>()), Times.Never());
  }
}