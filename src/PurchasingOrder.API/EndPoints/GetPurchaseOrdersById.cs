using PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

namespace PurchasingOrder.API.EndPoints;

public record GetPurchaseOrdersByIdResponse(PurchaseOrderDTO Order);

public class GetPurchaseOrdersById : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/GetPurchaseOrdersById/{Id}", async (string Id, ISender sender) =>
    {
      var result = await sender.Send(new GetPurchaseOrderByIdQuery(Id));

      var response = result.Adapt<GetPurchaseOrdersByIdResponse>();

      return Results.Ok(response);
    })
           .WithName("GetPurchaseOrdersById")
           .Produces<GetPurchaseOrdersByIdResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("GetPurchaseOrdersById")
           .WithDescription("GetPurchaseOrdersById");
  }
}
