using ERP.Shared.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PurchasingOrder.API.GRPCServices;
using Serilog;


namespace PurchasingOrder.API;

public static class DependencyInjection
{
  public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddCarter();
    services.AddGrpc();
    services.AddGrpcHealthChecks()
                .AddCheck("grpcHealth", () => HealthCheckResult.Healthy());

    services.AddExceptionHandler<CustomExceptionHandler>();

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Debug()
        .CreateLogger();

    return services;
  }

  public static WebApplication UseApiServices(this WebApplication app)
  {
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger(options => options.OpenApiVersion =
Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    //app.UseAuthorization();

    app.MapCarter();

    app.MapGrpcService<OrderProtoServiceImp>();
    app.MapGrpcHealthChecksService().AllowAnonymous();

    app.UseExceptionHandler(options => { });
    app.UseHealthChecks("/health",
        new HealthCheckOptions
        {
          ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

    return app;
  }
}

