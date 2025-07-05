using Grpc.Core;
using PurchasingOrder.API.Protos;
using PurchasingOrder.Application.PurchaseOrders.Commands.ValidatePurchaseOrder;

namespace PurchasingOrder.API.GRPCServices;

public class OrderProtoServiceImp(
  ILogger<OrderProtoServiceImp> logger,
  ISender sender) : OrderProtoService.OrderProtoServiceBase
{
  public async override Task<msgIsValidAndEligibleOrderResponse> IsValidAndEligibleOrder(msgIsValidAndEligibleOrderRequest request,
                                                                                ServerCallContext context)
  {
    try
    {
      var command = new ValidatePurchaseOrderCommand(RequestToDto(request));
      var result = await sender.Send(command);

      return new msgIsValidAndEligibleOrderResponse
      {
        Success = result.isValid,
        ErrorMessage = result.msg
      };
    }
    catch (Exception e)
    {
      logger.LogError(e.Message);
      return new msgIsValidAndEligibleOrderResponse
      {
        Success = false,
        ErrorMessage = e.Message
      };
    }
  }

  private ValidatePurchaseOrderDto RequestToDto(msgIsValidAndEligibleOrderRequest request)
  {
    return new ValidatePurchaseOrderDto(
        PurchaseOrderNumber: request.PurchaseOrderNumber,
        Items: request.Items.Select(item => new ValidatePOItem(
            Id: item.Id,
            GoodCode: item.GoodCode,
            Price: (decimal)item.Price
        )).ToList()
    );
  }

}
