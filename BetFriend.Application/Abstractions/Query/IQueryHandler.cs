using MediatR;

namespace BetFriend.Bet.Application.Abstractions.Query
{
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
                                                        where TRequest : IQuery<TResponse>
    {
    }
}
