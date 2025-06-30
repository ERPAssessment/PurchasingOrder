namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdValidator : AbstractValidator<GetPurchaseOrderByIdQuery>
{
  public GetPurchaseOrderByIdValidator()
  {
    RuleFor(x => x.Id).NotEmpty().WithMessage("Purchase Order Id should not be empty");
  }
}