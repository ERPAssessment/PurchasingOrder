using PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrderById;

namespace PurchasingOrder.API.EndPoints;

public record GetPurchaseOrderByIdResponse(PurchaseOrderDTO Order);

public class GetPurchaseOrderById : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/GetPurchaseOrdersById/{Id}", async (string Id, ISender sender) =>
    {
      var result = await sender.Send(new GetPurchaseOrderByIdQuery(Id));

      var response = result.Adapt<GetPurchaseOrderByIdResponse>();

      return Results.Ok(response);
    })
           .WithName("GetPurchaseOrderById")
           .Produces<GetPurchaseOrderByIdResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("GetPurchaseOrderById")
           .WithDescription("GetPurchaseOrderById");
  }
}
