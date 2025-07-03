using PurchasingOrder.Application.PurchaseOrders.Commands.ChangePurchaseOrderStatus;

namespace PurchasingOrder.API.EndPoints;

public record ChangePurchaseOrderStatusResponse(bool Result);
public record ChangePurchaseOrderStatusRequest(ChangePurchaseOrderStatusDto PurchaseOrderStatus);

public class ChangePurchaseOrderStatus : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapPost("/ChangePurchaseOrderStatus", async (ChangePurchaseOrderStatusRequest request, ISender sender) =>
    {
      var command = request.Adapt<ChangePurchaseOrderStatusCommand>();

      var result = await sender.Send(command);

      var response = result.Adapt<ChangePurchaseOrderStatusResponse>();

      return Results.Created($"/GetPurchaseOrderById/{request.PurchaseOrderStatus.PurchaseOrderId}", response);
    })
           .WithName("ChangePurchaseOrderStatus")
           .Produces<ChangePurchaseOrderStatusResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("ChangePurchaseOrderStatus")
           .WithDescription("ChangePurchaseOrderStatus");
  }
}
