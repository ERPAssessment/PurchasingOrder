namespace PurchasingOrder.Domain.ValueObjects;
public record PurchaseGoodId : ValueObject
{
  public Guid Value { get; }
  private PurchaseGoodId(Guid value) => Value = value;
  public static PurchaseGoodId Of(Guid value)
  {
    ArgumentNullException.ThrowIfNull(value);
    if (value == Guid.Empty)
    {
      throw new DomainException("PurchaseGoodId cannot be empty.");
    }

    return new PurchaseGoodId(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
