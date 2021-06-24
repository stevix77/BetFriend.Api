using MediatR;

namespace BetFriend.Application.Abstractions.Query
{
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
                                                        where TRequest : IQuery<TResponse>
    {
    }
}
