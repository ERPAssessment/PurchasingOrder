namespace PurchasingOrder.Domain.Events;

public record OrderCreatedEvent(PurchaseOrder Order) : IDomainEvent;
