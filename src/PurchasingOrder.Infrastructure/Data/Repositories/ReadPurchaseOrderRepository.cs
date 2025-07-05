using PurchasingOrder.Application.Data;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;
using PurchasingOrder.Domain.Specifications.Shared;

namespace PurchasingOrder.Infrastructure.Data.Repositories;

internal class ReadPurchaseOrderRepository
  (IApplicationDbContext dbContext)
  : IReadPurchaseOrderRepository
{
  public async Task<PurchaseOrder?> GetById(PurchaseOrderId Id, CancellationToken cancellationToken)
  {
    var order = await dbContext.PurchaseOrders
                      .AsNoTracking()
                      .Include(o => o.PurchaseItems)
                      .FirstOrDefaultAsync(po => po.Id == Id, cancellationToken);

    return order;
  }

  public async Task<IEnumerable<PurchaseOrder>> FindAsync(Specification<PurchaseOrder> specification,
                                                    int pageIndex, int pageSize,
                                                    CancellationToken cancellationToken)
  {
    return await dbContext.PurchaseOrders
               .AsNoTracking()
               .Include(o => o.PurchaseItems)
               .Where(specification.ToExpression())
               .OrderByDescending(o => o.IssuedDate)
               .Skip(pageSize * pageIndex)
               .Take(pageSize)
               .ToListAsync(cancellationToken);
  }

  public async Task<IEnumerable<PurchaseOrder>> GetPagedPurchaseOrders(int pageIndex, int pageSize, CancellationToken cancellationToken)
  {
    return await dbContext.PurchaseOrders
              .AsNoTracking()
              .Include(o => o.PurchaseItems)
              .OrderByDescending(o => o.IssuedDate)
              .Skip(pageSize * pageIndex)
              .Take(pageSize)
              .ToListAsync(cancellationToken);
  }


  public async Task<long> GetTotalCountAsync(CancellationToken cancellationToken)
  {
    return await dbContext.PurchaseOrders
                  .AsNoTracking()
                  .LongCountAsync(cancellationToken);
  }

  public async Task<PurchaseOrder?> GetByPurchaseOrderNumber(PurchaseOrderNumber PoNumber, CancellationToken cancellationToken)
  {
    var order = await dbContext.PurchaseOrders
                      .AsNoTracking()
                      .Include(o => o.PurchaseItems)
                      .FirstOrDefaultAsync(po => po.PONumber == PoNumber, cancellationToken);

    return order;
  }
}
