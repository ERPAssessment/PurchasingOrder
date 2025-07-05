using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace PurchasingOrder.Infrastructure.DependenciesInjection;

internal static class DIEx
{
  internal static void AddDIHealthChecks(this IServiceCollection services, IConfiguration configuration)
  {
    // Get RabbitMQ config values
    var rabbitHost = configuration["MessageBroker:Host"];
    var rabbitUser = configuration["MessageBroker:UserName"];
    var rabbitPassword = configuration["MessageBroker:Password"];

    // Combine into connection string
    var rabbitMqConnectionString = $"{rabbitHost}?username={rabbitUser}&password={rabbitPassword}";

    services.AddSingleton(sp =>
    {
      var factory = new ConnectionFactory
      {

        Uri = new Uri(rabbitMqConnectionString),
      };
      return factory.CreateConnectionAsync().GetAwaiter().GetResult();
    })

    .AddHealthChecks()
    .AddSqlServer(configuration.GetConnectionString("Database")!);
  }
}
