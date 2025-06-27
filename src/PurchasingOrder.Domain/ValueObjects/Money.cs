namespace PurchasingOrder.Domain.ValueObjects;
public record Money : ValueObject
{
  public decimal Amount { get; private set; }
  private Money(decimal value) => Amount = value;

  public static Money Of(decimal value)
  {
    if (value < 0)
      throw new ArgumentException("Money cannot be negative");

    return new Money(value);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Amount;
  }
}
