namespace PurchasingOrder.Domain.Events;

public record OrderShippedEvent(PurchaseOrder Order) : IDomainEvent;
