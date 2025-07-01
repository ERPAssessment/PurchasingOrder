using PurchasingOrder.API;
using PurchasingOrder.Application;
using PurchasingOrder.Infrastructure;
using PurchasingOrder.Infrastructure.Data.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Host.UseSerilog();

var app = builder.Build();


app.UseApiServices();

if (app.Environment.IsDevelopment())
{
  await app.InitialiseDatabaseAsync();
}

app.Run();
