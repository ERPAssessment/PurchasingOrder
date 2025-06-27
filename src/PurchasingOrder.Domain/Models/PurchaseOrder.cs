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

  public static PurchaseOrder NewPurchaseOrder(PurchaseOrderId id,
                                               PurchaseOrderNumber poNumber,
                                               DateTime IssuedDate,
                                               PurchaseOrderState state,
                                               PurchaseDocumentStatus status)
  {
    var order = new PurchaseOrder()
    {
      Id = id,
      PONumber = poNumber,
      IssuedDate = IssuedDate,
      DocumentState = state,
      DocumentStatus = status,
    };

    return order;
  }

  public void Create()
  {
    if (DocumentState != PurchaseOrderState.Draft)
      throw new DomainException("Only New POs can be created.");

    DocumentState = PurchaseOrderState.Created;

    AddDomainEvent(new OrderCreatedEvent(this));
  }

  public void Approve()
  {
    if (DocumentState != PurchaseOrderState.Created)
      throw new DomainException("Only Created POs can be approved.");

    DocumentState = PurchaseOrderState.Approved;

    AddDomainEvent(new OrderApprovedEvent(this));
  }

  public void Ship()
  {
    if (DocumentState != PurchaseOrderState.Approved)
      throw new DomainException("Only Approved POs can be shipped.");

    DocumentState = PurchaseOrderState.Shipped;

    AddDomainEvent(new OrderShippedEvent(this));
  }

  public void Close()
  {
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

  public void AddPurchaseItem(PurchaseItemSerialNumber serialNumber, PurchaseItemCode code, Money price)
  {
    if (DocumentState != PurchaseOrderState.Draft)
      throw new DomainException($"Cannot add items when purchase order is in {DocumentState} state");

    //if (_purchaseItems.Any(item => item.Equals(code)))
    //  throw new DomainException($"Good with code {code} already exists in this purchase order");

    //if (_purchaseItems.Any(item => item.Equals(serialNumber)))
    //  throw new DomainException($"Good with code {code} already exists in this purchase order");


    var item = new PurchaseItem(PurchaseOrderId.Of(Guid.NewGuid()),
                                 serialNumber,
                                 code,
                                 price);

    _purchaseItems.Add(item);
  }

}
