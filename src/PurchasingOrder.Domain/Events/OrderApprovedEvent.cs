namespace PurchasingOrder.Domain.Events;

public record OrderApprovedEvent(PurchaseOrder Order) : IDomainEvent;
