using PurchasingOrder.Application.Data;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseGoodRepo;

namespace PurchasingOrder.Infrastructure.Data.Repositories;

internal class ReadPurchaseGoodRepository
  (IApplicationDbContext dbContext)
  : IReadPurchaseGoodRepository
{
  public async Task<List<PurchaseGood>> GetPagedGoodsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken)
  {
    return await dbContext.PurchaseGoods
                  .AsNoTracking()
                  .OrderBy(o => o.Name)
                  .Skip(pageSize * pageIndex)
                  .Take(pageSize)
                  .ToListAsync(cancellationToken);
  }

  public async Task<long> GetTotalCountAsync(CancellationToken cancellationToken)
  {
    return await dbContext.PurchaseGoods
                  .AsNoTracking()
                  .LongCountAsync(cancellationToken);
  }
}
