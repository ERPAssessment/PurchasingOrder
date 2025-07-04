namespace PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

public record ChangePurchaseOrderStatusCommand(ChangePurchaseOrderStatusDto PurchaseOrderStatus)
    : ICommand<ChangePurchaseOrderStatusResult>;

public record ChangePurchaseOrderStatusResult(bool Result);
