namespace PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
  public CreatePurchaseOrderCommandValidator()
  {
    RuleFor(x => x.Order.PurchaseItems).NotEmpty().WithMessage("Purchase Order Items should not be empty");
  }
}