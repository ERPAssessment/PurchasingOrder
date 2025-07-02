namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdValidator : AbstractValidator<GetPurchaseOrderByIdQuery>
{
  public GetPurchaseOrderByIdValidator()
  {
    RuleFor(x => x.Id)
               .NotEmpty()
               .WithMessage("Purchase Order Id should not be empty")
               .Must(BeValidGuid)
               .WithMessage("Purchase Order Id must be a valid GUID format");

  }

  private bool BeValidGuid(string id)
  {
    if (string.IsNullOrEmpty(id))
      return false;

    return Guid.TryParse(id, out _);
  }
}