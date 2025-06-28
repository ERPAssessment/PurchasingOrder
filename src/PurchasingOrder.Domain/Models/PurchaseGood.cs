namespace PurchasingOrder.Domain.Models;
public class PurchaseGood : Entity<PurchaseGoodId>
{
  public PurchaseGoodCode Code { get; private set; } = default!;
  public string Name { get; private set; } = default!;


  public static PurchaseGood Create(PurchaseGoodId id, PurchaseGoodCode code, string name)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(name);

    var product = new PurchaseGood
    {
      Id = id,
      Name = name,
      Code = code
    };

    return product;
  }
}
