using PurchasingOrder.Application.Extenstions;
using PurchasingOrder.Domain.Abstractions.Repositories.PurchaseOrderRepo;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdHandler(IReadPurchaseOrderRepository orderRepository)
    : IQueryHandler<GetPurchaseOrderByIdQuery, GetPurchaseOrderByIdResults>
{
  public async Task<GetPurchaseOrderByIdResults> Handle(GetPurchaseOrderByIdQuery query, CancellationToken cancellationToken)
  {
    var orderId = PurchaseOrderId.Of(Guid.Parse(query.Id));
    var order = await orderRepository.GetById(orderId, cancellationToken);

    if (order == null)
    {
      throw new PurchaseOrderNotFoundException(query.Id);
    }

    return new GetPurchaseOrderByIdResults(order.ToPurchaseOrderDto());
  }
}