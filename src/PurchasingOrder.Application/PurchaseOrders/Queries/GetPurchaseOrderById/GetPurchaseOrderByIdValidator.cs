
using PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

namespace PurchasingOrder.Application.PurchaseOrders.Commands.ApprovePurchaseOrder;

public class GetPurchaseOrderByIdValidator : AbstractValidator<GetPurchaseOrderByIdQuery>
{
  public GetPurchaseOrderByIdValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Purchase Order Id should not be empty");
  }
}