namespace PurchasingOrder.Domain.Events;

public sealed record OrderShippedDomainEvent(PurchaseOrder Order) : IDomainEvent;
