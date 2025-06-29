using PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

namespace PurchasingOrder.API.EndPoints;

public record CreatePurchaseOrdersResponse(Guid Id);
public record CreatePurchaseOrdersRequest(PurchaseOrderDTO POrder);

public class CreatePurchaseOrders : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapPost("/CreatePurchaseOrder", async (CreatePurchaseOrdersRequest request, ISender sender) =>
    {
      var command = request.Adapt<CreatePurchaseOrderCommand>();

      var result = await sender.Send(command);

      var response = result.Adapt<CreatePurchaseOrdersResponse>();

      return Results.Created($"/CreatePurchaseOrder/{response.Id}", response);
    })
           .WithName("CreatePurchaseOrder")
           .Produces<GetPurchaseGoodsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("CreatePurchaseOrder")
           .WithDescription("CreatePurchaseOrder");
  }
}
