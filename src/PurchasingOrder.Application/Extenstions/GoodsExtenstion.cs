namespace PurchasingOrder.Application.Extenstions;

public static class GoodsExtenstion
{
  public static IEnumerable<PurchaseGoodDto> ToGoodsDtoList(this IEnumerable<PurchaseGood> goods)
  {
    return goods.Select(good => new PurchaseGoodDto(
        Id: good.Id.Value,
        Code: good.Code.Code,
        Name: good.Name
    ));
  }

}
