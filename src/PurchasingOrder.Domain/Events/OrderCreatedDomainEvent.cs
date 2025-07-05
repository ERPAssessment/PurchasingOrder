namespace PurchasingOrder.Domain.Events;

public sealed record OrderCreatedDomainEvent(PurchaseOrder Order) : IDomainEvent;
