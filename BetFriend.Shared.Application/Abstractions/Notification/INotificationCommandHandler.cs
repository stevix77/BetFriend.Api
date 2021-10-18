using MediatR;

namespace BetFriend.Shared.Application.Abstractions.Notification
{
    public interface INotificationCommandHandler<TRequest> : INotificationHandler<TRequest>
            where TRequest: INotificationCommand
    {
    }
}
