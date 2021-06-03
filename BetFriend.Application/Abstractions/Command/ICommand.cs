using MediatR;

namespace BetFriend.Application.Abstractions.Command
{
    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<TRequest> : IRequest<TRequest> { }
}
