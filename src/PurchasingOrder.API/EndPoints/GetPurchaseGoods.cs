using PurchasingOrder.Application.PurchaseOrders.Queries.GetGoods;

namespace PurchasingOrder.API.EndPoints;

public record GetPurchaseGoodsResponse(PaginatedResult<PurchaseGoodDto> Goods);

public class GetPurchaseGoods : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/GetPurchaseGoods", async ([AsParameters] PaginationRequest request, ISender sender) =>
    {
      var result = await sender.Send(new GetGoodsQuery(request));

      var response = result.Adapt<GetPurchaseGoodsResponse>();

      return Results.Ok(response);
    })
           .WithName("GetPurchaseGoods")
           .Produces<GetPurchaseGoodsResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Purchase Goods")
           .WithDescription("Get Purchase Goods");
  }
}
