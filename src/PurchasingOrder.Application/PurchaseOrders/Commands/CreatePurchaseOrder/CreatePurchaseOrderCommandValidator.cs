namespace PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

public class CreatePurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
  public CreatePurchaseOrderCommandValidator()
  {
    RuleFor(x => x.Orders).NotEmpty().WithMessage("Orders list should not be empty");

    RuleForEach(x => x.Orders).ChildRules(order =>
    {
      order.RuleFor(o => o.PurchaseItems).NotEmpty().WithMessage("Purchase Order Items should not be empty");
    });
  }
}