namespace BetFriend.Shared.Application.Abstractions.Command
{
    using MediatR;

    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<TRequest> : IRequest<TRequest> { }
}
