using Microsoft.EntityFrameworkCore;
using PurchasingOrder.Application.Extenstions;

namespace PurchasingOrder.Application.PurchaseOrders.Queries.GetPurchaseOrderById;

public class GetPurchaseOrderByIdHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetPurchaseOrderByIdQuery, GetPurchaseOrderByIdResults>
{
  public async Task<GetPurchaseOrderByIdResults> Handle(GetPurchaseOrderByIdQuery query, CancellationToken cancellationToken)
  {
    var orders = await dbContext.PurchaseOrders
        .Include(o => o.PurchaseItems)
        .AsNoTracking()
        .ToListAsync(cancellationToken); // async DB call

    var order = orders.FirstOrDefault(o => o.Id.Value.ToString() == query.Id); // client-side filtering

    if (order == null)
    {
      throw new PurchaseOrderNotFoundException(query.Id);
    }

    return new GetPurchaseOrderByIdResults(order.ToPurchaseOrderDto());
  }
}