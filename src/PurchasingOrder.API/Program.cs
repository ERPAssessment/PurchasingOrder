
using PurchasingOrder.API;
using PurchasingOrder.Application;
using PurchasingOrder.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);


var app = builder.Build();


app.UseApiServices();



app.Run();
