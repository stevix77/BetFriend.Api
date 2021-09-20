using MediatR;

namespace BetFriend.Bet.Application.Abstractions.Command
{
    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<TRequest> : IRequest<TRequest> { }
}
