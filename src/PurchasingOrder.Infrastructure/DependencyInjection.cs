using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PurchasingOrder.Application.Data;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseGoodRepo;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;
using PurchasingOrder.Infrastructure.Data;
using PurchasingOrder.Infrastructure.Data.Interceptors;
using PurchasingOrder.Infrastructure.Data.Repositories;

namespace PurchasingOrder.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices
         (this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("Database");

    // Add services to the container.
    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

    services.AddDbContext<ApplicationDbContext>((sp, options) =>
    {
      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
      options.UseSqlServer(connectionString);
    });

    services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
    services.AddScoped<IWritePurchaseOrderRepository, WritePurchaseOrderRepository>();
    services.AddScoped<IReadPurchaseOrderRepository, ReadPurchaseOrderRepository>();
    services.AddScoped<IReadPurchaseGoodRepository, ReadPurchaseGoodRepository>();

    return services;
  }
}