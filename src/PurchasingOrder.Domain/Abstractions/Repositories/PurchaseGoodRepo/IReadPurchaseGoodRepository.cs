namespace PurchasingOrder.Domain.Abstractions.Repositories.PurchaseGoodRepo;

public interface IReadPurchaseGoodRepository
{
  Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
  Task<List<PurchaseGood>> GetPagedGoodsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
}
