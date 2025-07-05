namespace PurchasingOrder.Domain.Events;

public sealed record OrderClosedDomainEvent(PurchaseOrder Order) : IDomainEvent;
