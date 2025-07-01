using PurchasingOrder.Application.PurchaseOrders.Commands.CreatePurchaseOrder;

namespace PurchasingOrder.API.EndPoints;

public record CreatePurchaseOrdersResponse(List<Guid> OrderIds);
public record CreatePurchaseOrdersRequest(List<CreatePurchaseOrderDto> Orders);

public class CreatePurchaseOrders : ICarterModule
{
  public void AddRoutes(IEndpointRouteBuilder app)
  {
    app.MapPost("/CreatePurchaseOrders", async (CreatePurchaseOrdersRequest request, ISender sender) =>
    {
      var command = request.Adapt<CreatePurchaseOrderCommand>();
      var result = await sender.Send(command);
      var response = result.Adapt<CreatePurchaseOrdersResponse>();
      return Results.Created($"/GetPurchaseOrders", response);
    })
           .WithName("CreatePurchaseOrders")
           .Produces<CreatePurchaseOrdersResponse>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Create Purchase Orders")
           .WithDescription("Create multiple purchase orders in a single request");
  }
}