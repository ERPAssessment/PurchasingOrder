namespace PurchasingOrder.Domain.ValueObjects;

public record PurchaseOrderId : ValueObject
{
  public Guid Value { get; }
  private PurchaseOrderId(Guid value) => Value = value;
  public static PurchaseOrderId Of(Guid value)
  {
    ArgumentNullException.ThrowIfNull(value);
    if (value == Guid.Empty)
    {
      throw new DomainException("OrderId cannot be empty.");
    }

    return new PurchaseOrderId(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
