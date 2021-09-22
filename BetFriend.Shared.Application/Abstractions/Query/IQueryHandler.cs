namespace BetFriend.Shared.Application.Abstractions.Query
{
    using MediatR;

    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
                                                        where TRequest : IQuery<TResponse>
    {
    }
}
