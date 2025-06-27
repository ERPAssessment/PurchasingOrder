namespace PurchasingOrder.Domain.ValueObjects;
public record PurchaseItemSerialNumber : ValueObject
{
  public string Value { get; }
  private PurchaseItemSerialNumber(string value) => Value = value;
  public static PurchaseItemSerialNumber Of(string value)
  {
    ArgumentNullException.ThrowIfNull(value);
    if (value == string.Empty)
    {
      throw new DomainException("PurchaseItemSerialNumber cannot be empty.");
    }

    return new PurchaseItemSerialNumber(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }
}
