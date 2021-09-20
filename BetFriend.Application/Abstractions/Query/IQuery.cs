using MediatR;

namespace BetFriend.Bet.Application.Abstractions.Query
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
