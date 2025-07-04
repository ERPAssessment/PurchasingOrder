namespace PurchasingOrder.Infrastructure.Data.Extensions;

internal class InitialData
{
  public static IEnumerable<PurchaseGood> Goods =>
   new List<PurchaseGood>
   {
      PurchaseGood.Create(PurchaseGoodId.Of(new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d")), PurchaseGoodCode.Of("PG001"), "Laptop Pro"),
      PurchaseGood.Create(PurchaseGoodId.Of(new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e")), PurchaseGoodCode.Of("PG002"), "Wireless Mouse"),
      PurchaseGood.Create(PurchaseGoodId.Of(new Guid("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f")), PurchaseGoodCode.Of("PG003"), "USB-C Hub"),
      PurchaseGood.Create(PurchaseGoodId.Of(new Guid("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f80")), PurchaseGoodCode.Of("PG004"), "External SSD")
   };

  public static IEnumerable<PurchaseOrder> PurchaseOrdersWithItems
  {
    get
    {
      var purchaseOrder1 = PurchaseOrder.CreatePurchaseOrder(
          PurchaseOrderId.Of(Guid.NewGuid()),
          PurchaseOrderNumber.Of("PO_001"),
          DateTime.UtcNow.AddDays(-10),
          PurchaseItemSerialNumber.Of("ITEM_000"),
          PurchaseGoodId.Of(new Guid("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f")),
          Money.Of(953)
  );

      purchaseOrder1.AddPurchaseItem(
          PurchaseItemSerialNumber.Of("ITEM_001"),
          PurchaseGoodId.Of(new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d")),
          Money.Of(1200));

      purchaseOrder1.AddPurchaseItem(
          PurchaseItemSerialNumber.Of("ITEM_002"),
          PurchaseGoodId.Of(new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e")),
          Money.Of(50));

      var purchaseOrder2 = PurchaseOrder.CreatePurchaseOrder(
          PurchaseOrderId.Of(Guid.NewGuid()),
          PurchaseOrderNumber.Of("PO_002"),
          DateTime.UtcNow.AddDays(-5),
          PurchaseItemSerialNumber.Of("ITEM_005"),
          PurchaseGoodId.Of(new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e")),
          Money.Of(123));

      purchaseOrder2.AddPurchaseItem(
          PurchaseItemSerialNumber.Of("ITEM_003"),
          PurchaseGoodId.Of(new Guid("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f")),
          Money.Of(75));

      purchaseOrder2.AddPurchaseItem(
          PurchaseItemSerialNumber.Of("ITEM_004"),
          PurchaseGoodId.Of(new Guid("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f80")),
          Money.Of(200));

      var purchaseOrder3 = PurchaseOrder.CreatePurchaseOrder(
       PurchaseOrderId.Of(Guid.NewGuid()),
       PurchaseOrderNumber.Of("PO_003"),
       DateTime.UtcNow.AddDays(-2),
       PurchaseItemSerialNumber.Of("ITEM_006"),
       PurchaseGoodId.Of(new Guid("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f80")),
       Money.Of(250));

      purchaseOrder3.AddPurchaseItem(
          PurchaseItemSerialNumber.Of("ITEM_007"),
          PurchaseGoodId.Of(new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d")),
          Money.Of(1500));

      purchaseOrder3.AddPurchaseItem(
          PurchaseItemSerialNumber.Of("ITEM_008"),
          PurchaseGoodId.Of(new Guid("c3d4e5f6-a7b8-4c9d-0e1f-2a3b4c5d6e7f")),
          Money.Of(800));


      purchaseOrder3.Approve();

      var purchaseOrder4 = PurchaseOrder.CreatePurchaseOrder(
                        PurchaseOrderId.Of(Guid.NewGuid()),
                        PurchaseOrderNumber.Of("PO_004"),
                        DateTime.UtcNow.AddDays(-1),
                        PurchaseItemSerialNumber.Of("ITEM_009"),
                        PurchaseGoodId.Of(new Guid("a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d")),
                        Money.Of(1350));

      purchaseOrder4.AddPurchaseItem(
          PurchaseItemSerialNumber.Of("ITEM_010"),
          PurchaseGoodId.Of(new Guid("b2c3d4e5-f6a7-4b8c-9d0e-1f2a3b4c5d6e")),
          Money.Of(65));

      purchaseOrder4.AddPurchaseItem(
          PurchaseItemSerialNumber.Of("ITEM_011"),
          PurchaseGoodId.Of(new Guid("d4e5f6a7-b8c9-4d0e-1f2a-3b4c5d6e7f80")),
          Money.Of(300));

      purchaseOrder4.Approve();

      return new List<PurchaseOrder> { purchaseOrder1, purchaseOrder2, purchaseOrder3, purchaseOrder4 };
    }
  }

};
