namespace PurchasingOrder.Application.PurchaseOrders.Commands.ValidatePurchaseOrder;

public record ValidatePurchaseOrderCommand(ValidatePurchaseOrderDto PODto)
    : ICommand<ValidatePurchaseOrderResult>;

public record ValidatePurchaseOrderResult(bool isValid, string msg);
