global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace PurchasingOrder.Infrastructure.Data.Extensions;

public static class DatabaseExtentions
{
  public static async Task InitialiseDatabaseAsync(this WebApplication app)
  {
    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    context.Database.MigrateAsync().GetAwaiter().GetResult();

    await SeedAsync(context);
  }

  private static async Task SeedAsync(ApplicationDbContext context)
  {
    await SeedGoodsAsync(context);
    await SeedPurchaseOrdersWithItemsAsync(context);
  }

  private static async Task SeedGoodsAsync(ApplicationDbContext context)
  {
    if (!await context.PurchaseGoods.AnyAsync())
    {
      await context.PurchaseGoods.AddRangeAsync(InitialData.Goods);
      await context.SaveChangesAsync();
    }
  }


  private static async Task SeedPurchaseOrdersWithItemsAsync(ApplicationDbContext context)
  {
    if (!await context.PurchaseOrders.AnyAsync())
    {
      await context.PurchaseOrders.AddRangeAsync(InitialData.PurchaseOrdersWithItems);
      await context.SaveChangesAsync();
    }
  }
}
