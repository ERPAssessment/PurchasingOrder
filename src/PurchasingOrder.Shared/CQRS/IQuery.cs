using MediatR;

namespace PurchasingOrder.Shared.CQRS;
public interface IQuery<out TResponse> : IRequest<TResponse>  
    where TResponse : notnull
{
}
