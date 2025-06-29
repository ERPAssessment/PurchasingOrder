using PurchasingOrder.Application.DTOs;
namespace PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

public record CreatePurchaseOrderCommand(PurchaseOrderDTO Order)
    : ICommand<CreatePurchaseOrderResult>;

public record CreatePurchaseOrderResult(Guid Id);
