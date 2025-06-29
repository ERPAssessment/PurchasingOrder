namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

public record GetPurchaseOrderByIdQuery(string Id)
    : IQuery<GetPurchaseOrderByIdResults>;

public record GetPurchaseOrderByIdResults(PurchaseOrderDTO Order);