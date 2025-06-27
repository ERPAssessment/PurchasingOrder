namespace PurchasingOrder.Domain.ValueObjects;
public record PurchaseItemCode : ValueObject
{
  public string Value { get; }
  private PurchaseItemCode(string value) => Value = value;
  public static PurchaseItemCode Of(string value)
  {
    ArgumentNullException.ThrowIfNull(value);
    if (value == string.Empty)
    {
      throw new DomainException("PurchaseItemCode cannot be empty.");
    }

    return new PurchaseItemCode(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
