
namespace PurchasingOrder.Application.PurchaseOrders.Commands.ApprovePurchaseOrder;

public record ApprovePurchaseOrderCommand(Guid Id)
    : ICommand<ApprovePurchaseOrderResult>;

public record ApprovePurchaseOrderResult(bool Result);
