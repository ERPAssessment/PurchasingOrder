using PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

namespace PurchasingOrder.API.EndPoints;

public record GetPurchaseOrdersResponse(PaginatedResult<PurchaseOrderDTO> Orders);

public class GetPurchaseOrders : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/GetPurchaseOrders", async ([AsParameters] PaginationRequest request, ISender sender) =>
    {

      var result = await sender.Send(new GetPurchaseOrdersQuery(request));

      var response = result.Adapt<GetPurchaseOrdersResponse>();

      return Results.Ok(response);
    })
           .WithName("GetPurchaseOrders")
           .Produces<GetPurchaseGoodsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("GetPurchaseOrders")
           .WithDescription("GetPurchaseOrders");
  }
}
