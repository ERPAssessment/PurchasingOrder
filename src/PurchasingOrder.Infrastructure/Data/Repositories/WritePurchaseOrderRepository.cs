using PurchasingOrder.Application.Data;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

namespace PurchasingOrder.Infrastructure.Data.Repositories;

internal class WritePurchaseOrderRepository
  (IApplicationDbContext dbContext)
  : IWritePurchaseOrderRepository
{
  public async Task<IEnumerable<PurchaseOrder>> Add(IEnumerable<PurchaseOrder> purchaseOrders, CancellationToken cancellationToken)
  {
    dbContext.PurchaseOrders.AddRange(purchaseOrders);
    await dbContext.SaveChangesAsync(cancellationToken);
    return purchaseOrders;
  }

  public async Task<PurchaseOrder?> GetById(PurchaseOrderId Id, CancellationToken cancellationToken)
  {
    var order = await dbContext.PurchaseOrders
                         .FindAsync([Id], cancellationToken: cancellationToken);

    return order;
  }

  public async Task<PurchaseOrder> Update(PurchaseOrder purchaseOrder, CancellationToken cancellationToken)
  {
    dbContext.PurchaseOrders.Update(purchaseOrder);
    await dbContext.SaveChangesAsync(cancellationToken);
    return purchaseOrder;
  }
}
