using ERP.Shared.Pagination;
using PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrders;

namespace PurchasingOrder.API.EndPoints;

public record GetPurchaseOrdersRequest(int PageIndex = 0,
    int PageSize = 10,
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string? State = null);


public record GetPurchaseOrdersResponse(PaginatedResult<PurchaseOrderDTO> Orders);


public class GetPurchaseOrders : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/GetPurchaseOrders", async ([AsParameters] GetPurchaseOrdersRequest request, ISender sender) =>
    {
      var query = new GetPurchaseOrdersQuery(
          new PaginationRequest(request.PageIndex, request.PageSize),
          request.StartDate,
          request.EndDate,
          request.State
      );

      var result = await sender.Send(query);

      var response = result.Adapt<GetPurchaseOrdersResponse>();

      return Results.Ok(response);
    })
           .WithName("GetPurchaseOrders")
           .Produces<GetPurchaseOrdersResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("GetPurchaseOrders")
           .WithDescription("GetPurchaseOrders");
  }
}
