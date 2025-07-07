namespace PurchasingOrder.Application.PurchaseOrders.Commands.ValidatePurchaseOrder;

internal class ValidatePurchaseOrderCommandValidator : AbstractValidator<ValidatePurchaseOrderCommand>
{
  public ValidatePurchaseOrderCommandValidator()
  {
    RuleFor(x => x.PODto)
            .NotNull()
            .WithMessage("Purchase order data is required");

    RuleFor(x => x.PODto.PurchaseOrderNumber)
        .NotEmpty()
        .WithMessage("Purchase order number is required");

    RuleFor(x => x.PODto.Items)
        .NotEmpty()
        .WithMessage("Purchase order items list should not be empty");

    RuleForEach(x => x.PODto.Items).ChildRules(item =>
    {
      item.RuleFor(i => i.Id)
          .NotEmpty()
          .Must(BeValidGuid)
          .WithMessage("Item ID must be a valid GUID");

      item.RuleFor(i => i.Price)
          .GreaterThanOrEqualTo(0)
          .WithMessage("Item price must be non-negative");

      item.RuleFor(i => i.GoodCode)
          .NotEmpty()
          .WithMessage("Good code is required");
    });
  }

  private bool BeValidGuid(string id)
  {
    return Guid.TryParse(id, out _);
  }
}
