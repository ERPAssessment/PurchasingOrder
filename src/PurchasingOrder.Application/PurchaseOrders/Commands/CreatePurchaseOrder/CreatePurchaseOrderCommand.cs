namespace PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

public record CreatePurchaseOrderCommand(CreatePurchaseOrderDto Order)
    : ICommand<CreatePurchaseOrderResult>;

public record CreatePurchaseOrderResult(Guid Id);
