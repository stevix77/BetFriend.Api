using MediatR;

namespace BetFriend.Bet.Application.Abstractions.Command
{
    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : IRequest<TResult>
    { 
    }
}
