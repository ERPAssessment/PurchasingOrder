namespace PurchasingOrder.Domain.Events;

public record OrderClosedEvent(PurchaseOrder Order) : IDomainEvent;
