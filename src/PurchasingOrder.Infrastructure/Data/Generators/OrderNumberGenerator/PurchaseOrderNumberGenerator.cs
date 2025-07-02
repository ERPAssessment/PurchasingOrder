using Microsoft.FeatureManagement;
using PurchasingOrder.Application.Data;

namespace PurchasingOrder.Infrastructure.Data.Generators.OrderNumberGenerator;

internal class PurchaseOrderNumberGenerator(
  IFeatureManager featureManager
  ) : IPurchaseOrderNumberGenerator
{
  public async Task<PurchaseOrderNumber> Generate()
  {
    if (await featureManager.IsEnabledAsync("POTimestampGeneration"))
    {
      return TimestampPurchaseOrderNumberGenerator.Generate();
    }
    else
    {
      return GuidPurchaseOrderNumberGenerator.Generate();
    }
  }
}


