namespace PurchasingOrder.Domain.Abstractions.Repositories.PurchaseGoodRepo;

public interface IReadPurchaseGoodRepository
{
  public Task<PurchaseGood?> GetByPurchaseGoodByCode(PurchaseGoodCode PgCode, CancellationToken cancellationToken);
  Task<long> GetTotalCountAsync(CancellationToken cancellationToken);
  Task<List<PurchaseGood>> GetPagedGoodsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken);
}
