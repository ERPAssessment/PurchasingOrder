using ERP.Shared.CQRS;

namespace PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

public record CreatePurchaseOrderCommand(List<CreatePurchaseOrderDto> Orders)
    : ICommand<CreatePurchaseOrderResult>;

public record CreatePurchaseOrderResult(List<Guid> OrderIds);
