namespace BetFriend.Shared.Application.Abstractions.Command
{
    using MediatR;

    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<TResponse> : IRequest<TResponse> { }
}
