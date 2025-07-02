namespace PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

public interface IWritePurchaseOrderRepository
{
  Task<IEnumerable<PurchaseOrder>> Add(IEnumerable<PurchaseOrder> purchaseOrders, CancellationToken cancellationToken);
  Task<PurchaseOrder> Update(PurchaseOrder purchaseOrder, CancellationToken cancellationToken);
  Task<PurchaseOrder?> GetById(PurchaseOrderId Id, CancellationToken cancellationToken);
}
