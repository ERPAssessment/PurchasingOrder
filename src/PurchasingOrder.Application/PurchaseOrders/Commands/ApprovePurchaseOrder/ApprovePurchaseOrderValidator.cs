
namespace PurchasingOrder.Application.PurchaseOrders.Commands.ApprovePurchaseOrder;

public class ApprovePurchaseOrderValidator : AbstractValidator<ApprovePurchaseOrderCommand>
{
  public ApprovePurchaseOrderValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Purchase Order Id should not be empty");
  }
}