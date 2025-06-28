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

  public static PurchaseOrder NewPurchaseOrder(
        PurchaseOrderId id,
        PurchaseOrderNumber poNumber,
        DateTime issuedDate,
        PurchaseOrderState state,
        PurchaseDocumentStatus status)
  {
    return new PurchaseOrder
    {
      Id = id,
      PONumber = poNumber,
      IssuedDate = issuedDate,
      DocumentState = state,
      DocumentStatus = status
    };
  }

  public void Create()
  {
    if (DocumentStatus != PurchaseDocumentStatus.Active)
      throw new DomainException("Cannot create a deactivated purchase order");

    if (DocumentState != PurchaseOrderState.Draft)
      throw new DomainException("Only New POs can be created.");

    DocumentState = PurchaseOrderState.Created;
    AddDomainEvent(new OrderCreatedEvent(this));
  }

  public void Approve()
  {
    if (DocumentStatus != PurchaseDocumentStatus.Active)
      throw new DomainException("Cannot approve a deactivated purchase order");

    if (DocumentState != PurchaseOrderState.Created)
      throw new DomainException("Only Created POs can be approved.");

    DocumentState = PurchaseOrderState.Approved;
    AddDomainEvent(new OrderApprovedEvent(this));
  }

  public void Ship()
  {
    if (DocumentStatus != PurchaseDocumentStatus.Active)
      throw new DomainException("Cannot ship a deactivated purchase order");

    if (DocumentState != PurchaseOrderState.Approved)
      throw new DomainException("Only Approved POs can be shipped.");

    DocumentState = PurchaseOrderState.Shipped;
    AddDomainEvent(new OrderShippedEvent(this));
  }

  public void Close()
  {
    if (DocumentStatus != PurchaseDocumentStatus.Active)
      throw new DomainException("Cannot ship a deactivated purchase order");

    if (DocumentState != PurchaseOrderState.Closed)
      throw new DomainException("Only Shipped POs can be closed.");

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
    if (DocumentStatus != PurchaseDocumentStatus.Active)
      throw new DomainException("Cannot add items to a deactivated purchase order");

    if (DocumentState != PurchaseOrderState.Draft)
      throw new DomainException($"Cannot add items when purchase order is in {DocumentState} state");

    if (_purchaseItems.Any(item => item.PurchaseItemSerialNumber == serialNumber))
      throw new DomainException($"Item with serial number {serialNumber} already exists in this purchase order");

    //if (_purchaseItems.Any(item => item.Good == good))
    //  throw new DomainException($"Item with code {good.Code} already exists in this purchase order");

    var item = new PurchaseItem(this.Id, serialNumber, goodId, price);
    _purchaseItems.Add(item);
  }

}
