using PurchasingOrder.Application.Data;
using PurchasingOrder.Application.DTOs;
using PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

namespace PurchasingOrder.Application.UnitTests.Commands;
public class CreatePurchaseOrderHandlerTests
{
  private readonly Mock<IWritePurchaseOrderRepository> _repositoryMock;
  private readonly Mock<IPurchaseItemSerialNumberGenerator> _serialNumberGeneratorMock;
  private readonly Mock<IPurchaseOrderNumberGenerator> _poNumberGeneratorMock;
  private readonly CreatePurchaseOrderCommandHandler _handler;

  public CreatePurchaseOrderHandlerTests()
  {
    _repositoryMock = new Mock<IWritePurchaseOrderRepository>();
    _serialNumberGeneratorMock = new Mock<IPurchaseItemSerialNumberGenerator>();
    _poNumberGeneratorMock = new Mock<IPurchaseOrderNumberGenerator>();
    _handler = new CreatePurchaseOrderCommandHandler(_repositoryMock.Object, _serialNumberGeneratorMock.Object, _poNumberGeneratorMock.Object);
  }

  [Fact]
  public async Task Handle_ShouldThrowValidationException_WhenOrdersListIsEmpty()
  {
    // Arrange
    var command = new CreatePurchaseOrderCommand(new List<CreatePurchaseOrderDto>());
    var validator = new CreatePurchaseOrderCommandValidator();

    // Act & Assert
    var validationResult = await validator.ValidateAsync(command);
    Assert.False(validationResult.IsValid);
    Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Orders list should not be empty");
    await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => validator.ValidateAndThrowAsync(command));
    _repositoryMock.Verify(r => r.Add(It.IsAny<IReadOnlyList<PurchaseOrder>>(), It.IsAny<CancellationToken>()), Times.Never());
    _poNumberGeneratorMock.Verify(g => g.Generate(), Times.Never());
    _serialNumberGeneratorMock.Verify(g => g.Generate(), Times.Never());
  }

  [Fact]
  public async Task Handle_ShouldThrowValidationException_WhenPurchaseItemsListIsEmpty()
  {
    // Arrange
    var command = new CreatePurchaseOrderCommand(new List<CreatePurchaseOrderDto>
            {
                new CreatePurchaseOrderDto(new List<CreatePurchaseItemDto>())
            });
    var validator = new CreatePurchaseOrderCommandValidator();

    // Act & Assert
    var validationResult = await validator.ValidateAsync(command);
    Assert.False(validationResult.IsValid);
    Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Purchase Order Items should not be empty");
    await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => validator.ValidateAndThrowAsync(command));
    _repositoryMock.Verify(r => r.Add(It.IsAny<IReadOnlyList<PurchaseOrder>>(), It.IsAny<CancellationToken>()), Times.Never());
    _poNumberGeneratorMock.Verify(g => g.Generate(), Times.Never());
    _serialNumberGeneratorMock.Verify(g => g.Generate(), Times.Never());
  }

  [Fact]
  public async Task Handle_ShouldThrowDomainException_WhenPurchaseGoodIdIsDuplicated()
  {
    // Arrange
    var orderId = Guid.NewGuid();
    var poNumber = PurchaseOrderNumber.Of("PO123");
    var serialNumber1 = PurchaseItemSerialNumber.Of("SN001");
    var serialNumber2 = PurchaseItemSerialNumber.Of("SN002");
    var duplicateGoodId = Guid.NewGuid();
    var command = new CreatePurchaseOrderCommand(new List<CreatePurchaseOrderDto>
            {
                new CreatePurchaseOrderDto(new List<CreatePurchaseItemDto>
                {
                    new CreatePurchaseItemDto(duplicateGoodId, 100m),
                    new CreatePurchaseItemDto(duplicateGoodId, 200m) // Duplicate PurchaseGoodId
                })
            });

    _poNumberGeneratorMock.Setup(g => g.Generate()).ReturnsAsync(poNumber);
    _serialNumberGeneratorMock.SetupSequence(g => g.Generate())
        .Returns(serialNumber1)
        .Returns(serialNumber2);

    // Act & Assert
    await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
    _repositoryMock.Verify(r => r.Add(It.IsAny<IReadOnlyList<PurchaseOrder>>(), It.IsAny<CancellationToken>()), Times.Never());
    _poNumberGeneratorMock.Verify(g => g.Generate(), Times.Once());
    _serialNumberGeneratorMock.Verify(g => g.Generate(), Times.Exactly(2));
  }
}
