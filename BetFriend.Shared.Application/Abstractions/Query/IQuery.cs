namespace BetFriend.Shared.Application.Abstractions.Query
{
    using MediatR;
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
