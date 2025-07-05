using PurchasingOrder.Domain.Enums;
using static PurchasingOrder.Domain.UnitTests.ModelData.DataSet;

namespace PurchasingOrder.Domain.UnitTests.Models;

public class PurchaseOrderStatusTests
{
  [Fact]
  public void Deactivate_ShouldSetDocumentStatusToDeactive()
  {
    // Arrange
    var order = CreateTestOrder();

    // Act
    order.Deactivate();

    // Assert
    Assert.Equal(PurchaseDocumentStatus.Deactive, order.DocumentStatus);
  }

  [Fact]
  public void Reactivate_ShouldSetDocumentStatusToActive()
  {
    // Arrange
    var order = CreateTestOrder();
    order.Deactivate();

    // Act
    order.Reactivate();

    // Assert
    Assert.Equal(PurchaseDocumentStatus.Active, order.DocumentStatus);
  }
}
