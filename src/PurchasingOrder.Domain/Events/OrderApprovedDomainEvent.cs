namespace PurchasingOrder.Domain.Events;

public sealed record OrderApprovedDomainEvent(PurchaseOrder Order) : IDomainEvent;
