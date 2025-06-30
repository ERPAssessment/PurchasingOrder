namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrderById;

public record GetPurchaseOrderByIdQuery(string Id)
    : IQuery<GetPurchaseOrderByIdResults>;

public record GetPurchaseOrderByIdResults(PurchaseOrderDTO Order);