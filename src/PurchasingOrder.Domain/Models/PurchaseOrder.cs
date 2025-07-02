namespace PurchasingOrder.Domain.Models;

public class PurchaseOrder : Aggregate<PurchaseOrderId>
{
  private readonly List<PurchaseItem> _purchaseItems = [];
  public IReadOnlyList<PurchaseItem> PurchaseItems => _purchaseItems.AsReadOnly();
  public PurchaseOrderNumber PONumber { get; private set; } = default!;
  public DateTime IssuedDate { get; private set; } = default!;
  public PurchaseOrderState DocumentState { get; private set; } = PurchaseOrderState.Draft;
  public PurchaseDocumentStatus DocumentStatus { get; private set; } = PurchaseDocumentStatus.Active;
  public Money TotalPrice
  {
    get => Money.Of(PurchaseItems.Sum(x => x.Price.Amount));
    private set { }
  }

  // Create Purchase Order with at least one item.
  public static PurchaseOrder CreatePurchaseOrder(
        PurchaseOrderId id,
        PurchaseOrderNumber poNumber,
        DateTime issuedDate,
        PurchaseItemSerialNumber serialNumber,
        PurchaseGoodId goodId,
        Money price)
  {
    var order = new PurchaseOrder
    {
      Id = id,
      PONumber = poNumber,
      IssuedDate = issuedDate,
      DocumentState = PurchaseOrderState.Created,
      DocumentStatus = PurchaseDocumentStatus.Active
    };

    order.AddPurchaseItem(serialNumber, goodId, price);
    order.AddDomainEvent(new OrderCreatedEvent(order));

    return order;
  }

  public void Approve()
  {
    CheckCanApprove();
    DocumentState = PurchaseOrderState.Approved;
    AddDomainEvent(new OrderApprovedEvent(this));
  }

  public void Ship()
  {
    CheckCanShip();

    DocumentState = PurchaseOrderState.BeingShipped;
    AddDomainEvent(new OrderShippedEvent(this));
  }

  public void Close()
  {
    CheckCanClose();

    DocumentState = PurchaseOrderState.Closed;
    AddDomainEvent(new OrderClosedEvent(this));
  }

  public void Deactivate()
  {
    DocumentStatus = PurchaseDocumentStatus.Deactive;
  }

  public void Reactivate()
  {
    DocumentStatus = PurchaseDocumentStatus.Active;
  }

  public void AddPurchaseItem(PurchaseItemSerialNumber serialNumber, PurchaseGoodId goodId, Money price)
  {
    CanAddPurchaseItem(serialNumber, goodId);

    var item = new PurchaseItem(Id, serialNumber, goodId, price);
    _purchaseItems.Add(item);
  }

  private void CanAddPurchaseItem(PurchaseItemSerialNumber serialNumber, PurchaseGoodId goodId)
  {
    CheckIsActivePurchaseOrder("Cannot add items to a deactivated purchase order");

    if (_purchaseItems.Any(item => item.PurchaseItemSerialNumber == serialNumber))
      throw new DomainException($"Item with serial number {serialNumber} already exists in this purchase order");

    if (_purchaseItems.Any(item => item.PurchaseGoodId == goodId))
      throw new DomainException($"Goods cannot be repeated on the same Purchase Order");
  }

  private void CheckIsActivePurchaseOrder(string message)
  {
    if (DocumentStatus != PurchaseDocumentStatus.Active)
      throw new DomainException(message);
  }

  private void CheckCanClose()
  {
    CheckIsActivePurchaseOrder("Cannot ship a deactivated purchase order");

    if (DocumentState != PurchaseOrderState.Closed)
      throw new DomainException("Only Shipped POs can be closed.");
  }

  private void CheckCanShip()
  {
    CheckIsActivePurchaseOrder("Cannot ship a deactivated purchase order");

    if (DocumentState != PurchaseOrderState.Approved)
      throw new DomainException("Only Approved POs can be shipped.");
  }
  private void CheckCanApprove()
  {
    CheckIsActivePurchaseOrder("Cannot approve a deactivated purchase order");

    if (DocumentState != PurchaseOrderState.Created)
      throw new DomainException("Only Created POs can be approved.");
  }
}
