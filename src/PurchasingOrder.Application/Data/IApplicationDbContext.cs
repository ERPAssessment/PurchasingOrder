using Microsoft.EntityFrameworkCore;
using PurchasingOrder.Domain.Models;

namespace PurchasingOrder.Application.Data;

public interface IApplicationDbContext
{
  public DbSet<PurchaseOrder> PurchaseOrders { get; }
  public DbSet<PurchaseItem> PurchaseItems { get; }
  public DbSet<PurchaseGood> PurchaseGoods { get; }

  Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}
