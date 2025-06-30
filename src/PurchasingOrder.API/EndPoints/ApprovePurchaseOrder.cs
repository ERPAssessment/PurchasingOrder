using PurchasingOrder.Application.PurchaseOrders.Commands.ApprovePurchaseOrder;

namespace PurchasingOrder.API.EndPoints;

public record ApprovePurchaseOrderResponse(bool Result);
public record ApprovePurchaseOrderRequest(Guid Id);

public class ApprovePurchaseOrder : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapPost("/ApprovePurchaseOrder", async (ApprovePurchaseOrderRequest request, ISender sender) =>
    {
      var command = request.Adapt<ApprovePurchaseOrderCommand>();

      var result = await sender.Send(command);

      var response = result.Adapt<ApprovePurchaseOrderResponse>();

      return Results.Created($"/GetPurchaseOrdersById/{request.Id}", response);
    })
           .WithName("ApprovePurchaseOrder")
           .Produces<GetPurchaseGoodsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("ApprovePurchaseOrder")
           .WithDescription("ApprovePurchaseOrder");
  }
}
