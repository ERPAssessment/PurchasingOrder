using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using PurchasingOrder.Application.Data;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseGoodRepo;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;
using PurchasingOrder.Infrastructure.Data;
using PurchasingOrder.Infrastructure.Data.Generators.OrderItem;
using PurchasingOrder.Infrastructure.Data.Generators.OrderNumberGenerator;
using PurchasingOrder.Infrastructure.Data.Interceptors;
using PurchasingOrder.Infrastructure.Data.Repositories;
using PurchasingOrder.Infrastructure.DependenciesInjection;

namespace PurchasingOrder.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices
         (this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("Database");

    services.AddFeatureManagement();

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

    services.AddSingleton<IPurchaseItemSerialNumberGenerator, GuidPurchaseItemSerialNumberGenerator>();
    services.AddScoped<IPurchaseOrderNumberGenerator, PurchaseOrderNumberGenerator>();
    services.AddDIHealthChecks(configuration);

    return services;
  }
}