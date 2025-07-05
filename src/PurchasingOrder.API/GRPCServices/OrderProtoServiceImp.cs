using Grpc.Core;
using PurchasingOrder.API.Protos;

namespace PurchasingOrder.API.GRPCServices;

public class OrderProtoServiceImp : OrderProtoService.OrderProtoServiceBase
{
  public override Task<msgIsValidAndEligibleOrderResponse> IsValidAndEligibleOrder(msgIsValidAndEligibleOrderRequest request,
                                                                                ServerCallContext context)
  {
    bool isValid = false;

    var response = new msgIsValidAndEligibleOrderResponse
    {
      Success = isValid
    };

    return Task.FromResult(response);
  }
}
