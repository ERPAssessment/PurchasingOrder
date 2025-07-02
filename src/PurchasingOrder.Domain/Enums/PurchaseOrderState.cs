using System.Text.Json.Serialization;

namespace PurchasingOrder.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurchaseOrderState
{
  Draft = 1,
  Created = 2,
  Approved = 3,
  BeingShipped = 4,
  Closed = 5
}