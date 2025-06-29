using System;
using System.Collections.Generic;

namespace PurchasingOrder.Application.DTOs;

public record PurchaseOrderDTO(
  Guid Id,
    string PONumber,
    DateTime IssuedDate,
    string DocumentState,
    string DocumentStatus,
    decimal TotalPrice,
    List<PurchaseItemDto> PurchaseItems
);
