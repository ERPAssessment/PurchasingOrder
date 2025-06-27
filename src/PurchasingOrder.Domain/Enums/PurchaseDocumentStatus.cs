using System.Text.Json.Serialization;

namespace PurchasingOrder.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PurchaseDocumentStatus
{
  Active = 1,
  Deactive = 2,
}