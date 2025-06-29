using PurchasingOrder.Application.Data;
using System.Reflection;

namespace PurchasingOrder.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
  : base(options) { }


  public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
  public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
  public DbSet<PurchaseGood> PurchaseGoods => Set<PurchaseGood>();


  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(builder);
  }
}
