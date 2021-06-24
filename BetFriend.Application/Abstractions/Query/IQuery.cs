using MediatR;

namespace BetFriend.Application.Abstractions.Query
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
