using PurchasingOrder.Application.DTOs;
using PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

namespace PurchasingOrder.Application.UnitTests.Commands;
public class ChangePurchaseOrderStatusHandlerTests
{
  private readonly Mock<IWritePurchaseOrderRepository> _repositoryMock;
  private readonly ChangePurchaseOrderStatusHandler _handler;

  public ChangePurchaseOrderStatusHandlerTests()
  {
    _repositoryMock = new Mock<IWritePurchaseOrderRepository>();
    _handler = new ChangePurchaseOrderStatusHandler(_repositoryMock.Object);
  }

  [Fact]
  public async Task Handle_ShouldActivateOrderAndReturnSuccess_WhenIsActiveIsTrue()
  {
    // Arrange
    var orderId = PurchaseOrderId.Of(Guid.NewGuid());
    var command = new ChangePurchaseOrderStatusCommand(new ChangePurchaseOrderStatusDto(orderId.Value, true));
    var order = CreateTestOrder(orderId);
    order.Deactivate(); // Set to Deactive to test Reactivate
    Assert.Equal(PurchaseDocumentStatus.Deactive, order.DocumentStatus);
    _repositoryMock.Setup(r => r.GetById(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Result);
    _repositoryMock.Verify(r => r.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.Is<PurchaseOrder>(o =>
        o.Id == orderId && o.DocumentStatus == PurchaseDocumentStatus.Active), It.IsAny<CancellationToken>()), Times.Once());
    Assert.Equal(PurchaseDocumentStatus.Active, order.DocumentStatus);
  }

  [Fact]
  public async Task Handle_ShouldDeactivateOrderAndReturnSuccess_WhenIsActiveIsFalse()
  {
    // Arrange
    var orderId = PurchaseOrderId.Of(Guid.NewGuid());
    var command = new ChangePurchaseOrderStatusCommand(new ChangePurchaseOrderStatusDto(orderId.Value, false));
    var order = CreateTestOrder(orderId);
    Assert.Equal(PurchaseDocumentStatus.Active, order.DocumentStatus); // Initially Active
    _repositoryMock.Setup(r => r.GetById(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Result);
    _repositoryMock.Verify(r => r.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.Is<PurchaseOrder>(o =>
        o.Id == orderId && o.DocumentStatus == PurchaseDocumentStatus.Deactive), It.IsAny<CancellationToken>()), Times.Once());
    Assert.Equal(PurchaseDocumentStatus.Deactive, order.DocumentStatus);
  }

  [Fact]
  public async Task Handle_ShouldThrowPurchaseOrderNotFoundException_WhenOrderDoesNotExist()
  {
    // Arrange
    var orderId = PurchaseOrderId.Of(Guid.NewGuid());
    var command = new ChangePurchaseOrderStatusCommand(new ChangePurchaseOrderStatusDto(orderId.Value, true));
    _repositoryMock.Setup(r => r.GetById(orderId, It.IsAny<CancellationToken>())).ReturnsAsync((PurchaseOrder)null);

    // Act & Assert
    await Assert.ThrowsAsync<PurchaseOrderNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    _repositoryMock.Verify(r => r.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.IsAny<PurchaseOrder>(), It.IsAny<CancellationToken>()), Times.Never());
  }

  [Fact]
  public async Task Handle_ShouldNotThrow_WhenActivatingAlreadyActiveOrder()
  {
    // Arrange
    var orderId = PurchaseOrderId.Of(Guid.NewGuid());
    var command = new ChangePurchaseOrderStatusCommand(new ChangePurchaseOrderStatusDto(orderId.Value, true));
    var order = CreateTestOrder(orderId);
    Assert.Equal(PurchaseDocumentStatus.Active, order.DocumentStatus); // Already Active
    _repositoryMock.Setup(r => r.GetById(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Result);
    _repositoryMock.Verify(r => r.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.Is<PurchaseOrder>(o =>
        o.Id == orderId && o.DocumentStatus == PurchaseDocumentStatus.Active), It.IsAny<CancellationToken>()), Times.Once());
    Assert.Equal(PurchaseDocumentStatus.Active, order.DocumentStatus);
  }

  [Fact]
  public async Task Handle_ShouldNotThrow_WhenDeactivatingAlreadyDeactiveOrder()
  {
    // Arrange
    var orderId = PurchaseOrderId.Of(Guid.NewGuid());
    var command = new ChangePurchaseOrderStatusCommand(new ChangePurchaseOrderStatusDto(orderId.Value, false));
    var order = CreateTestOrder(orderId);
    order.Deactivate(); // Set to Deactive
    Assert.Equal(PurchaseDocumentStatus.Deactive, order.DocumentStatus);
    _repositoryMock.Setup(r => r.GetById(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Result);
    _repositoryMock.Verify(r => r.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.Is<PurchaseOrder>(o =>
        o.Id == orderId && o.DocumentStatus == PurchaseDocumentStatus.Deactive), It.IsAny<CancellationToken>()), Times.Once());
    Assert.Equal(PurchaseDocumentStatus.Deactive, order.DocumentStatus);
  }
}