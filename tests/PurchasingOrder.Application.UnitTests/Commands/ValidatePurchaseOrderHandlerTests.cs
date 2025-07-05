using PurchasingOrder.Application.DTOs;
using PurchasingOrder.Application.PurchaseOrders.Commands.ValidatePurchaseOrder;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseGoodRepo;

namespace PurchasingOrder.Application.UnitTests.Commands
{
  public class ValidatePurchaseOrderHandlerTests
  {
    private readonly Mock<IReadPurchaseOrderRepository> _orderRepositoryMock;
    private readonly Mock<IReadPurchaseGoodRepository> _goodRepositoryMock;
    private readonly ValidatePurchaseOrderHandler _handler;

    public ValidatePurchaseOrderHandlerTests()
    {
      _orderRepositoryMock = new Mock<IReadPurchaseOrderRepository>();
      _goodRepositoryMock = new Mock<IReadPurchaseGoodRepository>();
      _handler = new ValidatePurchaseOrderHandler(_orderRepositoryMock.Object, _goodRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidResult_WhenOrderNotFound()
    {
      // Arrange
      var poNumber = PurchaseOrderNumber.Of("PO123");
      var command = new ValidatePurchaseOrderCommand(new ValidatePurchaseOrderDto(
          "PO123",
          new List<ValidatePOItem> { new ValidatePOItem(Guid.NewGuid().ToString(), "GC001", 100.0m) }));

      _orderRepositoryMock.Setup(r => r.GetByPurchaseOrderNumber(poNumber, It.IsAny<CancellationToken>())).ReturnsAsync((PurchaseOrder)null);

      // Act
      var result = await _handler.Handle(command, CancellationToken.None);

      // Assert
      Assert.False(result.isValid);
      Assert.Equal($"Purchase order {poNumber} not found", result.msg);
      _orderRepositoryMock.Verify(r => r.GetByPurchaseOrderNumber(poNumber, It.IsAny<CancellationToken>()), Times.Once());
      _goodRepositoryMock.Verify(r => r.GetByPurchaseGoodByCode(It.IsAny<PurchaseGoodCode>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenPurchaseOrderNumberIsEmpty()
    {
      // Arrange
      var command = new ValidatePurchaseOrderCommand(new ValidatePurchaseOrderDto(
          "",
          new List<ValidatePOItem> { new ValidatePOItem(Guid.NewGuid().ToString(), "GC001", 100.0m) }));
      var validator = new ValidatePurchaseOrderCommandValidator();

      // Act & Assert
      var validationResult = await validator.ValidateAsync(command);
      Assert.False(validationResult.IsValid);
      Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Purchase order number is required");
      await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => validator.ValidateAndThrowAsync(command));
      _orderRepositoryMock.Verify(r => r.GetByPurchaseOrderNumber(It.IsAny<PurchaseOrderNumber>(), It.IsAny<CancellationToken>()), Times.Never());
      _goodRepositoryMock.Verify(r => r.GetByPurchaseGoodByCode(It.IsAny<PurchaseGoodCode>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenItemsListIsEmpty()
    {
      // Arrange
      var command = new ValidatePurchaseOrderCommand(new ValidatePurchaseOrderDto(
          "PO123",
          new List<ValidatePOItem>()));
      var validator = new ValidatePurchaseOrderCommandValidator();

      // Act & Assert
      var validationResult = await validator.ValidateAsync(command);
      Assert.False(validationResult.IsValid);
      Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Purchase order items list should not be empty");
      await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => validator.ValidateAndThrowAsync(command));
      _orderRepositoryMock.Verify(r => r.GetByPurchaseOrderNumber(It.IsAny<PurchaseOrderNumber>(), It.IsAny<CancellationToken>()), Times.Never());
      _goodRepositoryMock.Verify(r => r.GetByPurchaseGoodByCode(It.IsAny<PurchaseGoodCode>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenItemIdIsInvalid()
    {
      // Arrange
      var command = new ValidatePurchaseOrderCommand(new ValidatePurchaseOrderDto(
          "PO123",
          new List<ValidatePOItem> { new ValidatePOItem("invalid-guid", "GC001", 100.0m) }));
      var validator = new ValidatePurchaseOrderCommandValidator();

      // Act & Assert
      var validationResult = await validator.ValidateAsync(command);
      Assert.False(validationResult.IsValid);
      Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Item ID must be a valid GUID");
      await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => validator.ValidateAndThrowAsync(command));
      _orderRepositoryMock.Verify(r => r.GetByPurchaseOrderNumber(It.IsAny<PurchaseOrderNumber>(), It.IsAny<CancellationToken>()), Times.Never());
      _goodRepositoryMock.Verify(r => r.GetByPurchaseGoodByCode(It.IsAny<PurchaseGoodCode>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenPriceIsNegative()
    {
      // Arrange
      var command = new ValidatePurchaseOrderCommand(new ValidatePurchaseOrderDto(
          "PO123",
          new List<ValidatePOItem> { new ValidatePOItem(Guid.NewGuid().ToString(), "GC001", -100.0m) }));
      var validator = new ValidatePurchaseOrderCommandValidator();

      // Act & Assert
      var validationResult = await validator.ValidateAsync(command);
      Assert.False(validationResult.IsValid);
      Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Item price must be non-negative");
      await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => validator.ValidateAndThrowAsync(command));
      _orderRepositoryMock.Verify(r => r.GetByPurchaseOrderNumber(It.IsAny<PurchaseOrderNumber>(), It.IsAny<CancellationToken>()), Times.Never());
      _goodRepositoryMock.Verify(r => r.GetByPurchaseGoodByCode(It.IsAny<PurchaseGoodCode>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenGoodCodeIsEmpty()
    {
      // Arrange
      var command = new ValidatePurchaseOrderCommand(new ValidatePurchaseOrderDto(
          "PO123",
          new List<ValidatePOItem> { new ValidatePOItem(Guid.NewGuid().ToString(), "", 100.0m) }));
      var validator = new ValidatePurchaseOrderCommandValidator();

      // Act & Assert
      var validationResult = await validator.ValidateAsync(command);
      Assert.False(validationResult.IsValid);
      Assert.Contains(validationResult.Errors, e => e.ErrorMessage == "Good code is required");
      await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => validator.ValidateAndThrowAsync(command));
      _orderRepositoryMock.Verify(r => r.GetByPurchaseOrderNumber(It.IsAny<PurchaseOrderNumber>(), It.IsAny<CancellationToken>()), Times.Never());
      _goodRepositoryMock.Verify(r => r.GetByPurchaseGoodByCode(It.IsAny<PurchaseGoodCode>(), It.IsAny<CancellationToken>()), Times.Never());
    }
  }
}
