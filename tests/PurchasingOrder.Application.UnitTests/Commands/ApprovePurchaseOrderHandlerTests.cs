
namespace PurchasingOrder.Application.UnitTests.Commands;

public class ApprovePurchaseOrderHandlerTests
{
  private readonly Mock<IWritePurchaseOrderRepository> _repositoryMock;
  private readonly ApprovePurchaseOrderCommandHandler _handler;

  public ApprovePurchaseOrderHandlerTests()
  {
    _repositoryMock = new Mock<IWritePurchaseOrderRepository>();
    _handler = new ApprovePurchaseOrderCommandHandler(_repositoryMock.Object);
  }

  [Fact]
  public async Task Handle_ShouldApproveOrderAndReturnSuccess_WhenOrderExists()
  {
    // Arrange
    var orderId = PurchaseOrderId.Of(Guid.NewGuid());
    var command = new ApprovePurchaseOrderCommand(orderId.Value);
    var order = CreateTestOrder(orderId);
    _repositoryMock.Setup(r => r.GetById(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.Result);
    _repositoryMock.Verify(r => r.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.Is<PurchaseOrder>(o =>
        o.Id == orderId && o.DocumentState == PurchaseOrderState.Approved), It.IsAny<CancellationToken>()), Times.Once());
    Assert.Equal(PurchaseOrderState.Approved, order.DocumentState);
    Assert.Contains(order.DomainEvents, e => e is OrderApprovedDomainEvent && ((OrderApprovedDomainEvent)e).Order == order);
  }

  [Fact]
  public async Task Handle_ShouldThrowPurchaseOrderNotFoundException_WhenOrderDoesNotExist()
  {
    // Arrange
    var orderId = PurchaseOrderId.Of(Guid.NewGuid());
    var command = new ApprovePurchaseOrderCommand(orderId.Value);
    _repositoryMock.Setup(r => r.GetById(orderId, It.IsAny<CancellationToken>())).ReturnsAsync((PurchaseOrder)null!);

    // Act & Assert
    await Assert.ThrowsAsync<PurchaseOrderNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    _repositoryMock.Verify(r => r.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.IsAny<PurchaseOrder>(), It.IsAny<CancellationToken>()), Times.Never());
  }

  [Fact]
  public async Task Handle_ShouldThrowValidationException_WhenIdIsEmpty()
  {
    // Arrange
    var command = new ApprovePurchaseOrderCommand(Guid.Empty);
    var validator = new ApprovePurchaseOrderValidator();

    // Act & Assert
    var validationResult = await validator.ValidateAsync(command);
    Assert.False(validationResult.IsValid);
    Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Purchase Order Id should not be empty");
    await Assert.ThrowsAsync<ValidationException>(() => validator.ValidateAndThrowAsync(command));
    _repositoryMock.Verify(r => r.GetById(It.IsAny<PurchaseOrderId>(), It.IsAny<CancellationToken>()), Times.Never());
    _repositoryMock.Verify(r => r.Update(It.IsAny<PurchaseOrder>(), It.IsAny<CancellationToken>()), Times.Never());
  }

  [Fact]
  public async Task Handle_ShouldThrowDomainException_WhenOrderIsNotInCreatedState()
  {
    // Arrange
    var orderId = PurchaseOrderId.Of(Guid.NewGuid());
    var command = new ApprovePurchaseOrderCommand(orderId.Value);
    var order = CreateTestOrder(orderId);
    order.Approve(); // Set to Approved, so Approve again should fail
    _repositoryMock.Setup(r => r.GetById(orderId, It.IsAny<CancellationToken>())).ReturnsAsync(order);

    // Act & Assert
    await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
    _repositoryMock.Verify(r => r.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once());
    _repositoryMock.Verify(r => r.Update(It.IsAny<PurchaseOrder>(), It.IsAny<CancellationToken>()), Times.Never());
  }
}