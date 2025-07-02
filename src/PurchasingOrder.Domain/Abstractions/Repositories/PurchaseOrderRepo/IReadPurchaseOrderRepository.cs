using PurchasingOrder.Domain.Specifications.Shared;

namespace PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

public interface IReadPurchaseOrderRepository
{
  Task<PurchaseOrder?> GetById(PurchaseOrderId Id, CancellationToken cancellationToken);
  Task<IEnumerable<PurchaseOrder>> FindAsync(Specification<PurchaseOrder> specification, int pageIndex, int pageSize, CancellationToken cancellationToken);
  Task<IEnumerable<PurchaseOrder>> GetPagedPurchaseOrders(int pageIndex, int pageSize, CancellationToken cancellationToken);
  Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
}
