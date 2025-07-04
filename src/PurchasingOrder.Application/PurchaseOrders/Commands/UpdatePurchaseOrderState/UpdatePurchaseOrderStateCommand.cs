namespace PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

public record UpdatePurchaseOrderStateCommand(UpdatePurchaseOrderStateDto PurchaseOrderState)
    : ICommand<UpdatePurchaseOrderStateResult>;

public record UpdatePurchaseOrderStateResult(bool Result);
